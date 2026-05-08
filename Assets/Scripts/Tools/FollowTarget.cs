using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Vector3             tgtPos;

    [SerializeField] 
           Transform    target;
    public Vector3      offset;
    public float        speed   = 3.0f;
    public bool         relative;
    public bool         x       = true;
    public bool         y       = true;
    public bool         z       = true;

    float vx;
    float vy;
    float vz;

    void FixedUpdate()
    {
        tgtPos = target.position + offset;
        if (relative)
            tgtPos = target.position + target.rotation * offset;

        float nx = .0f;
        float ny = .0f;
        float nz = .0f;
        if (x) nx = Mathf.SmoothDamp(transform.position.x, tgtPos.x, ref vx, speed);
        if (y) ny = Mathf.SmoothDamp(transform.position.y, tgtPos.y, ref vy, speed);
        if (z) nz = Mathf.SmoothDamp(transform.position.z, tgtPos.z, ref vz, speed);

        transform.position = new Vector3 (nx, ny, nz);
    }

    private void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.lightYellow;
        Gizmos.DrawLine(target.position, tgtPos);
    }

    private void OnValidate()
    {
        Sync();
    }

    public void Sync()
    {
        tgtPos = target.position + offset;
        if (relative)
            tgtPos = target.position + target.rotation * offset;

        transform.position = tgtPos;
    }

    public void ChangeTarget(Transform tgt)
    {
        target = tgt;
        if (target is RectTransform)
        {
            throw new System.NotImplementedException();
            // TODO - manage ui case
        }
    }
}
