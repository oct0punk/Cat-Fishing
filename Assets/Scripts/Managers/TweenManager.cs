using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public abstract class Tween
{
    public Transform tr { get; protected set; }
    public Tween(Transform t) { tr = t; }
    public abstract void Tick();
    public abstract void OnEnd();
}

public struct SShake
{
    public float sp;
    public float am;

    public SShake(float sp, float am)
    {
        this.sp = sp;
        this.am = am;
    }
}


public class Shake : Tween
{
    float      sp = 20.0f;
    float      am = 5.0f;
    Vector3 initialPos;


    public Shake(Transform t) : base(t) 
    { 
        initialPos = tr.localPosition; 
    }

    public void Init(SShake sh)
    {
        sp = sh.sp;
        am = sh.am;
    }

    public override void Tick()
    {
        Vector3 pos = Vector3.zero;
        float t = Time.timeSinceLevelLoad * sp;
        pos.x = (Mathf.PerlinNoise(t, 0.0f) - .5f);
        pos.y = (Mathf.PerlinNoise(0.0f, t) - .5f);
        pos *= am;
        
        tr.SetLocalPositionAndRotation(initialPos + pos, tr.localRotation);
    }

    public override void OnEnd()
    {
        tr.SetLocalPositionAndRotation(initialPos, tr.localRotation);
    }
}

public class Oscillate : Tween
{
    float sp = 16.0f;
    float a  = 25.0f;
    float t = 0.0f;


    public Oscillate(Transform t) : base(t) {}


    public override void Tick()
    {
        if (tr == null) return;

        t += sp*Time.deltaTime;
        var r = Mathf.PingPong(t, 1) - 0.5f;
        r *= a;

        var rot = tr.localEulerAngles;
        rot.z = r;
        tr.localEulerAngles = rot;
    }

    public override void OnEnd()
    {
        if (tr == null) return;

        var rot = tr.localEulerAngles;
        rot.z = 0.0f;
        tr.localEulerAngles = rot;
    }
}

public class RotateTo : Tween
{
    float tarY;
    float sp;


    public RotateTo(Transform t, float targetRot, float duration) : base(t)
    {
        tarY = targetRot;
        sp = 3.0f;
        return;
        //float a = Quaternion.Angle(tr.rotation, tarRot);
        //if (duration == 0.0f) 
            //sp = 1000.0f;
        //else 
            //sp = a / duration;
    }

    public override void Tick()
    {
        var rot = tr.eulerAngles; //Quaternion.RotateTowards(tr.rotation, Quaternion.Euler(tr.eulerAngles.x, tarY, tr.eulerAngles.z), sp*Time.deltaTime);
        rot.y = tarY;
        //tr.eulerAngles = rot;
    }

    public override void OnEnd() {}
}


public class TweenManager : MonoBehaviour
{
    Dictionary<Tween, float> tweens = new();
    

    void Update()
    {
        for (int i = tweens.Count-1; i >= 0; i--)
        {
            var tw = tweens.ElementAt(i).Key;
            var lt = tweens.ElementAt(i).Value;
            tw.Tick();
            lt -= Time.deltaTime;
            tweens[tw] = lt;
            if (lt <= 0.0f)
            {
                tw.OnEnd();
                Kill(tw);
            }
        }
    }

    void Kill(Tween tw)
    {
        tweens.Remove(tw);
        if (Log.tw)
        {
            Debug.Log("Kill tweening, Count: " + tweens.Count);
        }
    }

    void Play(Tween tw, float lt)
    {
        tweens[tw] = lt;
        if (Log.tw)
        {
            Debug.Log($"{tw.GetType().Name} {tw.tr.name}, Count: {tweens.Count}", tw.tr.gameObject);
        }
    }

    public void Shake(Transform tr, float lifetime, SShake conf)
    {
        var tw = new Shake(tr);
        tw.Init(conf);
        Play(tw, lifetime);
    }
    public void Shake(Transform tr) => Shake(tr, 1.2f, new SShake(40.0f, 2.0f));

    public void Oscillate(Transform tr, float lifetime = 1.0f)
    {
        Play(new Oscillate(tr), lifetime);
    }
    public void Rotate(Transform tr, float lifetime = .7f, float targetRot = 90.0f) 
        => Play(new RotateTo(tr, targetRot, lifetime), lifetime);
}

