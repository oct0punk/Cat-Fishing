using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Vector3             tgtPos;

    [SerializeField] 
           Transform    target;
    public Vector3      offset;
    public float        speed = 3.0f;
    public bool         relative;
    public bool         y = true;


    void Update()
    {
        tgtPos = target.position + offset;
        if (relative)
            tgtPos = target.position + target.rotation * offset;

        if (!y) tgtPos.y = 0.0f;

        transform.position = Vector3.Lerp(transform.position, tgtPos, Time.deltaTime * speed);
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
