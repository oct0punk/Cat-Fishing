using System;
using UnityEngine;


public abstract class FSM<T>
{
    protected T tgt;
    public FSM(T tg) { tgt = tg; }
    public abstract void OnEnter(T tgt);
    public abstract void OnEnd(T tgt);
    public abstract void Tick(T tgt);
}


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
    float dodgeDur = 30.0f;
    float amp = .1f;

    public PlayerBattleState(PlayerController tg) : base(tg)
    {
        
    }

    public override void OnEnd(PlayerController tgt)
    {
        if (Boot.Logs.fsm) Debug.Log("OnEnd BattleState");
        var con = Boot.con;

        con.onTouchUp.RemoveListener(Dodge);
        offset = 0.0f;
    }

    public override void OnEnter(PlayerController tgt)
    {
        if (Boot.Logs.fsm) Debug.Log("OnEnter FishingState");
        var con = Boot.con;
        con.onTouchUp.AddListener(Dodge);
    }

    public override void Tick(PlayerController tgt)
    {
        Fish f = Boot.bat.conf.fish;

        float move = Mathf.Abs(offset);
        float sign = Mathf.Sign(offset);
        var dir = f.transform.position - tgt.transform.position;
        var ang = Mathf.Atan2(dir.x, dir.z) + 90.0f*sign;
        var moveDir = new Vector3(Mathf.Cos(ang), 0.0f, Mathf.Sin(ang));
        //tgt.transform.position += tgt.transform.right * sign*Time.deltaTime*dodgeDur;
        offset -= sign *  Mathf.Max(0.0f, (move - Time.deltaTime * dodgeDur));
    }

    void Dodge()
    {
        if (Boot.con.IsLeftSidedTouch())
        {
            offset = -amp;
        }
        else
        {
            offset = amp;
        }
    }
}