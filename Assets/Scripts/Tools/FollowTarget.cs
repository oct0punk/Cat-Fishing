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

        float nx = transform.position.x;
        float ny = transform.position.y;
        float nz = transform.position.z;
        if (x) nx = Mathf.SmoothDamp(nx, tgtPos.x, ref vx, smooth);
        if (y) ny = Mathf.SmoothDamp(ny, tgtPos.y, ref vy, smooth);
        if (z) nz = Mathf.SmoothDamp(nz, tgtPos.z, ref vz, smooth);

        transform.position = new Vector3 (nx, ny, nz);
    }

    private void OnValidate()
    {
        if (targets == null)    return;
        if (targets.Length <= 0) return;
        Sync();
    }


    Vector3 GetMedPoint()
    {
        if (targets == null)     return transform.position;
        if (targets.Length == 0) return transform.position;
        if (targets.Length == 1)  return targets[0].position;
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
        if (!x) tgtPos.x = transform.position.x;
        if (!y) tgtPos.y = transform.position.y;
        if (!z) tgtPos.z = transform.position.z;

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
