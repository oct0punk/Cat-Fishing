using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public const float isoAngle     = 20;
    public const float camDist      = 30;
    public const float zoomSpeed    = 3.0f;
    public float rotDuration  = .2f;

    public Transform pivot;
    public Camera main;

    float defZoom;
    float tarZoom;
    float zoomVel;
    [SerializeField]float tarRot;
    float rotVel;
    FollowTarget flw;


    private void Awake()
    {
        main = Camera.main;
        pivot = GameObject.Find("Camera Pivot").transform;
        flw = pivot.GetComponent<FollowTarget>();
        main.transform.localPosition = Vector3.back * camDist;
        tarZoom = defZoom = main.orthographicSize;
        tarRot = pivot.eulerAngles.y;
    }

    private void FixedUpdate()
    {
        //t -= Time.deltaTime;
        //if (t < 0)
        //{
        //    t = 1.0f;

        //}

        if (main.orthographicSize != tarZoom)
        {
            main.orthographicSize = Mathf.SmoothDamp(main.orthographicSize, tarZoom, ref zoomVel, Time.deltaTime * zoomSpeed);
        }

        //if (Mathf.DeltaAngle(pivot.eulerAngles.y, tarRot) > 0.000001f)
        {
            var rot = pivot.eulerAngles;
            rot.y = Mathf.SmoothDampAngle(rot.y, tarRot, ref rotVel, rotDuration);
            pivot.eulerAngles = rot;
            IsoSprite.TickAll();
        }
    }

    public void Camshake()
    {
        Boot.twn.Shake(main.transform, .4f, new SShake(40.0f, .6f));
    }

    public void Focus(Transform tgt)
    {
        if (Log.cam) Debug.Log("Focusing: " + tgt.name);
        flw.ChangeTarget(tgt);
    }

    public void Zoom(float z)
    {
        if (Log.cam) Debug.Log("Zooming: " + z);
        tarZoom = z;
    }

    public void Rotate(float ang)
    {
        tarRot = ang;// Mathf.MoveTowardsAngle(tarRot, ang, 360.0f);
        if (Log.cam) Debug.Log("Rotating to: " + ang);
    }

    public void Reset()
    {   
        Focus(Boot.player.transform);
        tarZoom = defZoom;
        //tarRot = .0f;
    }

    public Quaternion LookCamRotation() => Quaternion.LookRotation(Boot.cam.main.transform.forward, Vector3.up);
}
