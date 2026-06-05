using UnityEditor;
using UnityEngine;


public class Fish : MonoBehaviourWithEvents, IProduct
{
    FSM<Fish>               fsm;
    FishAppearingState      appearingState;
    FishIdleState           idleState;
    FishBiteState           biteState;
    FishFleeingState        fleeingState;
    FishBattleState         battleState;

    public float    hp;
    public FishData dat;


    private void Awake()
    {
        appearingState  = new FishAppearingState(this);
        idleState       = new FishIdleState(this);
        biteState       = new FishBiteState(this);
        fleeingState    = new FishFleeingState(this);
        battleState     = new FishBattleState(this);
        fsm = appearingState;
        fsm.OnEnter(this);
    }

    private void FixedUpdate()
    {
        fsm.Tick(this);
    }

    public void ChangeState(FSM<Fish> newState)
    {
        fsm.OnEnd(this);
        fsm = newState;
        fsm.OnEnter(this);
    }


    public void Init(FishData _data)
    {
        dat = _data;
        hp  = dat.hp;
        if (TryGetComponent(out SpriteRenderer rdr)) {
            rdr.sprite = dat.spr;
        }
    }

    public void Idle() => ChangeState(idleState);
    public void Bite() => ChangeState(biteState);
    public void Flee() => ChangeState(fleeingState);
    public void Battle() => ChangeState(battleState);
    public void Destroy() => Destroy(gameObject);   //For fsm


    public void MoveTo(Vector3 pos, float spd)
    {
        Vector3 dir = pos - transform.position;
        if (dir.magnitude < 0.1f) return;
        dir.Normalize();

        float ang = Mathf.Atan2(-dir.z, dir.x) * Mathf.Rad2Deg;

        transform.SetPositionAndRotation(Vector3.MoveTowards(
            transform.position, pos, spd * Time.deltaTime),
            Quaternion.Euler(0.0f, ang+90.0f, 0.0f));

        if (Boot.Logs.fish) Debug.DrawLine(transform.position, pos, Color.yellow);
    }
}
