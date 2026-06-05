using UnityEngine;


public class FishAppearingState : FSM<Fish>
{
    SpriteRenderer  rdr;
    Vector3         tarPos;

    public FishAppearingState(Fish fish) : base(fish)
    {
        rdr = fish.GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnEnd(Fish fish) {}

    public override void OnEnter(Fish fish)
    {
        var c = rdr.color;
        c.a = 0.0f;
        rdr.color = c;

        Vector2 rand    = Random.insideUnitCircle * 2.0f;
        tarPos          = fish.transform.position + new Vector3(rand.x, 0.0f, rand.y);
    }

    public override void Tick(Fish fish)
    {
        if (rdr.color.a < .5f)
        {
            var c = rdr.color;
            c.a += Time.deltaTime;
            rdr.color = c;
            fish.MoveTo(tarPos, fish.dat.normalSpeed * Time.deltaTime);
        }
        else
        {
            fish.Idle();
        }
    }
}

public class FishIdleState : FSM<Fish>
{
    public FishIdleState(Fish fish) : base(fish) { }

    public override void OnEnd(Fish fish) { }

    public override void OnEnter(Fish fish) { }

    public override void Tick(Fish fish)
    {
        if (Boot.player.isFishing) fish.Bite();
    }
}

public class FishBiteState : FSM<Fish>
{
    public Transform hook;

    public FishBiteState(Fish tg) : base(tg) { }

    public override void OnEnd(Fish fish) { }

    public override void OnEnter(Fish fish)
    {
        hook = Boot.player.hook.transform;
    }

    public override void Tick(Fish fish)
    {
        if (!Boot.player.isFishing)
        {
            fish.Idle();
            return;
        }
        
        fish.MoveTo(hook.position, fish.dat.chargeSpeed);

        float dist = Vector3.Distance(hook.position, fish.transform.position);
        if (dist < 0.01f)
        {
            Boot.game.BeforeBattle(new SBattleDatas(fish));
        }
    }
}

public class FishFleeingState : FSM<Fish>
{
    Vector3 tarPos;
    SpriteRenderer rdr;

    public FishFleeingState(Fish fish) : base(fish)
    {
        rdr = fish.GetComponentInChildren<SpriteRenderer>();
    }

    public override void OnEnd(Fish fish) { }

    public override void OnEnter(Fish fish)
    {
        var dir = fish.transform.position - Boot.player.hook.transform.position;
        dir.Normalize();
        tarPos = fish.transform.position + dir * fish.dat.chargeSpeed * 2f;
    }

    public override void Tick(Fish fish)
    {
        if (rdr != null)
        {
            if (rdr.color.a > 0.0f)
            {
                var c = rdr.color;
                c.a -= Time.deltaTime;
                rdr.color = c;
            }
            else
            {
                fish.Destroy();
                return;
            }
        }

        if (Boot.player == null || Boot.player.hook == null) return;
        fish.MoveTo(tarPos, fish.dat.chargeSpeed);
    }
}

public class FishBattleState : FSM<Fish>
{
    PlayerController p;
    Vector3 dir;

    public FishBattleState(Fish fish) : base(fish) { }

    public override void OnEnd(Fish fish) { }

    public override void OnEnter(Fish fish)
    {
        p = Boot.player;
        if (p == null)
        {
            throw Boot.MissingPlayerReference();
        }
        Vector3 vec = fish.transform.position - p.transform.position;
        //dir = fish.transform.forward + vec.normalized;
        dir = vec;
        dir.y = 0.0f;
        dir.Normalize();
        Boot.bat.OnFishChangingDirection(dir);
    }

    public override void Tick(Fish fish)
    {
        fish.MoveTo(fish.transform.position + dir * 3000.0f, fish.dat.pullingSpeed);
        p.hook.transform.SetPositionAndRotation(fish.transform.position, Quaternion.identity);
    }
}
