using UnityEngine;


public class PlayerFishingState : FSM<PlayerController>
{
    Vector3 aimPos;


    public PlayerFishingState(PlayerController tg) : base(tg)
    {
    }

    public override void OnEnd(PlayerController tgt)
    {
        var con = Boot.con;
        if (Boot.Logs.fsm) Debug.Log("OnEnd FishingState");
        if (con == null) return;
        con.onTouch.RemoveListener(Aim);
        con.onTouchUp.RemoveListener(Hook);
    }

    public override void OnEnter(PlayerController tgt)
    {
        var con = Boot.con;
        if (Boot.Logs.fsm) Debug.Log("OnEnter FishingState");
        EndFishing();
        if (con == null) return;
        con.onTouch.AddListener(Aim);
        con.onTouchUp.AddListener(Hook);
    }

    public override void Tick(PlayerController tgt)
    {
    }


    public void StartFishing()
    {
        tgt.isFishing = true;
        if (Boot.Logs.ev) Debug.Log("Invoke onStartFishing event. Size: " + tgt.onStartFishing.GetPersistentEventCount());
        tgt.hook.transform.SetPositionAndRotation(aimPos, Quaternion.identity);
        tgt.onStartFishing.Invoke();
    }
    public void EndFishing()
    {
        tgt.isFishing = false;
        tgt.hook.transform.SetPositionAndRotation(tgt.transform.position + Vector3.down * 2.0f, Quaternion.identity);
    }

    void Hook()
    {
        if (Boot.Logs.inp) Debug.Log("Hook action");
        if (!tgt.isFishing)
        {
            StartFishing();
        }
        else
        {
            if (Boot.game.State != GameState.Battle)
                EndFishing();
        }
    }
    void Aim()
    {
        if (Boot.cam == null) return;
        if (Boot.Logs.inp) Debug.Log("Aim event");
        var cam = Boot.cam.main;
        var wpos = cam.ScreenToWorldPoint(Boot.con.tPos);
        aimPos = Helpers.LinePlaneIntersection(wpos, cam.transform.forward, Vector3.zero, Vector3.up);
    }
}

public class PlayerBattleState : FSM<PlayerController>
{
    public float offset;
    Vector3 vel;
    Fish f;
    ControlsManager con;

    GameObject r;
    GameObject l;
    GameObject t;


    void show_cubes(bool val)
    {
        if (!Boot.Logs.ddg) val = false;
        r.SetActive(val);
        l.SetActive(val);
        t.SetActive(val);
    }

    public PlayerBattleState(PlayerController tg) : base(tg)
    {
        GameObject cr(string name)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.localScale = Vector3.one * .4f;
            go.transform.rotation = Quaternion.identity;
            go.name = name;
            return go;
        }
        r = cr("right");
        l = cr("left");
        t = cr("tar");
        show_cubes(false);
    }

    public override void OnEnd(PlayerController tgt)
    {
        if (Boot.Logs.fsm) Debug.Log("OnEnd BattleState");
        var con = Boot.con;

        con.onTouch.RemoveListener(Dodge);
        show_cubes(false);
    }

    public override void OnEnter(PlayerController tgt)
    {
        if (Boot.Logs.fsm) Debug.Log("OnEnter FishingState");
        f = Boot.bat.conf.fish;
        con = Boot.con;
        con.onTouch.AddListener(Dodge);
        offset = 0.0f;
        show_cubes(true);
    }

    public override void Tick(PlayerController tgt)
    {
        var h = f.transform.position - Boot.bat.dir * (tgt.lineL * Boot.bat.GetProg() + Boot.Datas.PlayerCatchRange);
        var tar = h + f.transform.right * offset;

        if (Boot.Logs.ddg)
        {
            show_cubes(true);
            var proj = Helpers.ProjPointOnSeg(tgt.transform.position, h, f.transform.right);
            l.transform.position = h + f.transform.right * Boot.Datas.PlayerDodgeMarge / 2;
            r.transform.position = h - f.transform.right * Boot.Datas.PlayerDodgeMarge / 2;
            t.transform.position = proj;

            Debug.DrawLine(tgt.transform.position, tar, Color.white);
            Debug.DrawLine(f.transform.position, h, Color.yellow);
            Debug.DrawLine(f.transform.position, f.transform.position
                                                  + f.transform.right * offset, Color.red);
            Debug.Log("offset: " + offset);
        }

        // tgt.transform.position = Vector3.SmoothDamp(tgt.transform.position, tar, ref vel, .1f);
        tgt.transform.position = Vector3.MoveTowards(tgt.transform.position, tar, Time.fixedDeltaTime * 20.0f);
    }

    void Dodge()
    {
        //if (Boot.con.IsLeftSidedTouch())
        //{
        //    offset -= .1f;
        //}
        //else
        //{
        //    offset += .1f;
        //}
        offset += (Boot.con.GetTouchScreenPos().x - .5f) * Boot.Datas.PlayerDodgeSpeed * Time.deltaTime;
        offset = Mathf.Clamp(offset,
            Boot.Datas.PlayerDodgeMarge * -.5f,
            Boot.Datas.PlayerDodgeMarge * .5f);
    }
}

public class PlayerCatchState : FSM<PlayerController>
{
    SpriteRenderer spr;

    public PlayerCatchState(PlayerController tg) : base(tg)
    {
        spr = tg.GetComponent<SpriteRenderer>();
    }

    public override void OnEnd(PlayerController tgt)
    {
        spr.color = Color.white;
    }

    public override void OnEnter(PlayerController tgt)
    {
        spr.color = Color.violet;
    }

    public override void Tick(PlayerController tgt) { }
}