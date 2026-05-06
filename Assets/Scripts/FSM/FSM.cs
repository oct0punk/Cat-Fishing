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


public class PlayerFishing : FSM<PlayerController>
{
    Vector3 aimPos;


    public PlayerFishing(PlayerController tg) : base(tg)
    {
    }

    public override void OnEnd(PlayerController tgt)
    {
        if (Boot.con == null) return;
        Boot.con.onTouch.RemoveListener(Aim);
        Boot.con.onTouch.RemoveListener(Hook);
    }

    public override void OnEnter(PlayerController tgt)
    {
        EndFishing();
        if (Boot.con == null) return;
        Boot.con.onTouch.AddListener(Aim);
        Boot.con.onTouchUp.AddListener(Hook);
    }

    public override void Tick(PlayerController tgt)
    {
    }


    public void StartFishing()
    {
        tgt.isFishing = true;
        if (Log.ev) Debug.Log("Invoke onStartFishing event. Size: " + tgt.onStartFishing.GetPersistentEventCount());
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
        if (Log.inp) Debug.Log("Hook action");
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
        if (Log.inp) Debug.Log("Aim event");
        var cam = Boot.cam.main;
        var wpos = cam.ScreenToWorldPoint(Boot.con.tPos);
        aimPos = Helpers.LinePlaneIntersection(wpos, cam.transform.forward, Vector3.zero, Vector3.up);
    }
}

public class PlayerBattle : FSM<PlayerController>
{
    public PlayerBattle(PlayerController tg) : base(tg)
    {
    }

    public override void OnEnd(PlayerController tgt)
    {
        throw new NotImplementedException();
    }

    public override void OnEnter(PlayerController tgt)
    {
        throw new NotImplementedException();
    }

    public override void Tick(PlayerController tgt)
    {
        throw new NotImplementedException();
    }
}