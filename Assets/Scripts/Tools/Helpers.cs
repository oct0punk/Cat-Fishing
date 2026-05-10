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
    public static Vector2 WorldToScreen(Vector3 world)
    {
        return RectTransformUtility.WorldToScreenPoint(Boot.cam.main, world);
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
}

