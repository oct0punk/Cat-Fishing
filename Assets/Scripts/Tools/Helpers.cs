using System.Collections.Generic;
using UnityEngine;


public static class Helpers
{    
    public static Vector3 LinePlaneIntersection(Vector3 l_st, Vector3 l_dir, Vector3 p_co, Vector3 p_no)
    {        
        var dot = Vector3.Dot(p_no, l_dir);

        if (Mathf.Abs(dot) > 0.01)
        {
            var w = l_st - p_co;
            var fac = -Vector3.Dot(p_no, w) / dot;
            l_dir *= fac;
            return l_st + l_dir;
        }
        return l_st;
    }

    public static Vector3 ProjPointOnSeg(Vector3 p, Vector3 segStart, Vector3 segDir)
    {
        Vector3 relPos = p - segStart;
        segDir.Normalize();
        if (relPos.magnitude < 1e-5f) return p;
        return segStart + segDir*Vector3.Dot(relPos, segDir);
    }

    public static Vector2 WorldToScreen(Vector3 world) { return RectTransformUtility.WorldToScreenPoint(Boot.cam.main, world); }
    public static Vector3 ScreenToWater(Vector2 scrpos)
    { 
        var ray = RectTransformUtility.ScreenPointToRay(Boot.cam.main, scrpos);
        return LinePlaneIntersection(ray.origin, ray.direction, Vector3.zero, Vector3.up);
    }

    
    static Dictionary<float, YieldInstruction> waits = new() { [0.0f] = new WaitForEndOfFrame() };
    public static YieldInstruction Wait(float dur)
    {
        if (!waits.ContainsKey(dur))
        {
            waits[dur] = new WaitForSeconds(dur);
        }
        if (Boot.Logs.boot) Debug.Log("wait for " + dur);
        return waits[dur];
    }
    
    public static Color ColAlpha(Vector3 rgb, float alpha)  => new(rgb.x, rgb.y, rgb.z, alpha);
    public static Color ColAlpha(Color rgb, float alpha)    => new(rgb.r, rgb.g, rgb.b, alpha);

    public static Quaternion RandomRotation()
    {
        return Quaternion.Euler(0f, Random.value * 360f, 0f);
    }
}

