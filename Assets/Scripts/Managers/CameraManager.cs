using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public const float isoAngle = 20;
    public const float camDist = 30;

    public Transform pivot;
    public Camera main;

    float defZoom;
    float tarZoom;
    float zoomVel = 1.0f;
    FollowTarget flw;


    private void Awake()
    {
        main = Camera.main;
        pivot = GameObject.Find("Camera Pivot").transform;
        flw = pivot.GetComponent<FollowTarget>();
        main.transform.localPosition = Vector3.back * camDist;
        tarZoom = defZoom = main.orthographicSize;
    }

    private void Update()
    {
        //t -= Time.deltaTime;
        //if (t < 0)
        //{
        //    t = 1.0f;

        //}

        if (main.orthographicSize != tarZoom)
        {
            main.orthographicSize = Mathf.SmoothDamp(main.orthographicSize, tarZoom, ref zoomVel, Time.deltaTime * 3.0f);
        }
    }

    public void Camshake()
    {
        Boot.twn.Shake(main.transform, .4f, new SShake(40.0f, .6f));
    }

    public void Focus(Transform tgt)
    {
        if (Log.cam) Debug.Log("Focus: " + tgt.name);
        flw.ChangeTarget(tgt);
    }

    public void Zoom(float z)
    {
        if (Log.cam) Debug.Log("Zoom: " + z);
        tarZoom = z;
    }

    public void Reset()
    {   
        Focus(Boot.player.transform);
        tarZoom = defZoom;
    }

    public Quaternion LookCamRotation() => Quaternion.LookRotation(Boot.cam.main.transform.forward, Vector3.up);
}
