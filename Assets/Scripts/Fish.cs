using UnityEditor;
using UnityEngine;


public class Fish : MonoBehaviourWithEvents, IProduct
{
    FishFSM fsm;
    public float    hp;
    public FishData dat;


    private void Awake()
    {
        fsm = new FishFSM(this);
    }

    private void FixedUpdate()
    {
        fsm.Tick();
    }

    public void Init(FishData _data)
    {
        dat = _data;
        hp  = dat.hp;
        if (TryGetComponent(out SpriteRenderer rdr)) {
            rdr.sprite = dat.spr;
        }
    }

    public void Idle()      => fsm.IdleState();
    public void Bite()      => fsm.BiteState();
    public void Flee()      => fsm.FleeingState();
    public void Battle()    => fsm.BattleState();
    public void Destroy()   => Destroy(gameObject);   //For fsm


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
