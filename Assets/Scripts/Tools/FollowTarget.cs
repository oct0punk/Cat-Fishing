using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Vector3             tgtPos;

    [SerializeField] 
           Transform[]    targets;
    public Vector3      offset;
    public float        smooth = .5f;
    //public bool         relative;
    public bool         x       = true;
    public bool         y       = true;
    public bool         z       = true;

    float vx;
    float vy;
    float vz;

    void FixedUpdate()
    {
        tgtPos = GetMedPoint() + offset;

        float nx = .0f;
        float ny = .0f;
        float nz = .0f;
        if (x) nx = Mathf.SmoothDamp(transform.position.x, tgtPos.x, ref vx, smooth);
        if (y) ny = Mathf.SmoothDamp(transform.position.y, tgtPos.y, ref vy, smooth);
        if (z) nz = Mathf.SmoothDamp(transform.position.z, tgtPos.z, ref vz, smooth);

        transform.position = new Vector3 (nx, ny, nz);
    }

    private void OnValidate()
    {
        Sync();
    }


    Vector3 GetMedPoint()
    {
        if (targets.Length==0) return Vector3.zero;
        if (targets.Length == 1) return targets[0].position;
        Vector3 pos = Vector3.zero;
        foreach (Transform t in targets)
        {
            pos += t.position;
        }
        pos /= targets.Length;
        return pos;
    }

    public void Sync()
    {
        tgtPos = GetMedPoint() + offset;

        transform.position = tgtPos;
    }

    public void SetTargets(Transform[] tgts)
    {
        targets = tgts;
    }

    public void SetTarget(Transform tgt)
    {
        SetTargets(new Transform[1] { tgt });
    }
}
