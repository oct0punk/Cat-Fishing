using UnityEngine;


public class Fish : MonoBehaviourWithEvents, IProduct
{
    private float           spd = 10.0f;
    private Vector3         trgPos;
    private System.Action   bhv;
    private SpriteRenderer  spr;

    public float    hp;
    public FishData dat;


    private void Awake()
    {
        spr = GetComponentInChildren<SpriteRenderer>();
        if (spr)
        {
            var c = spr.color;
            c.a = 0.0f;
            spr.color = c;
        }


        trgPos = transform.position;
        bhv = NaturalBehaviour;
    }

    public void Init(FishData _data)
    {
        dat = _data;
        hp  = dat.hp;
        spd = dat.normalSpeed;
        if (spr) {
            spr.sprite = dat.spr;
        }
    }

    public void Flee()
    {
        bhv = FleeingBehaviour;
        spd = dat.chargeSpeed;
    }

    public void Pull()
    {
        bhv = PullBehaviour;
        spd = dat.pullingSpeed;
        spr.color = Color.plum;
    }

    private void Update()
    {
        bhv();
        MoveToTarget();
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
    }

    
    void NaturalBehaviour()
    {
        if (spr != null)
        {
            if (spr.color.a < 1.0f)
            {
                var c = spr.color;
                c.a += Time.deltaTime;
                spr.color = c;
                trgPos = transform.position + transform.forward * spd;
                return;
            }
        }

        var p = Boot.player;
        if (p != null) bhv = BiteBehaviour;
    }

    void BiteBehaviour()
    {
        var p = Boot.player;
        if (p == null) return;
        if (p.isFishing)
        {
            trgPos = p.hook.transform.position;

            if (Vector3.Distance(transform.position, p.hook.transform.position) <= .1f)
            {
                Boot.game.EnterBattle(new SBattleDatas(this));
            }
        }
        else
        {
            bhv = NaturalBehaviour;
        }
    }

    void PullBehaviour()
    {
        PlayerController p = Boot.player;
        if (p == null) {
            throw Boot.MissingPlayerReference();
        }
        Vector3 vec = transform.position - Boot.player.transform.position;
        Vector3 dir = transform.forward + vec.normalized;
        dir.Normalize();
        trgPos = transform.position + dir * dat.chargeSpeed;        

        p.hook.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        if (vec.magnitude > p.lineL)
        {
            p.transform.SetPositionAndRotation(
                transform.position - vec.normalized * p.lineL, 
                p.transform.rotation);
        }
    }

    void FleeingBehaviour()
    {
        if (spr != null)
        {
            if (spr.color.a > 0.0f)
            {
                var c = spr.color;
                c.a -= Time.deltaTime;
                spr.color = c;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        if (Boot.player == null || Boot.player.hook == null) return;
        Vector3 fleeDir = transform.position - Boot.player.hook.transform.position;
        trgPos = transform.position + fleeDir * dat.normalSpeed;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        Vector3 dir = trgPos - transform.position;
        if (dir.magnitude < 0.1f) return;
        dir.Normalize();

        float ang = Mathf.Atan2(-dir.z, dir.x) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(Vector3.MoveTowards(
            transform.position, trgPos, spd * Time.deltaTime),
            Quaternion.Euler(0.0f, ang+90.0f, 0.0f));

        if (Log.fish) Debug.DrawLine(transform.position, trgPos, Color.yellow);
    }

}
