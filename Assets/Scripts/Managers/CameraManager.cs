using System;
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
    float tarRot;
    float rotVel;
    float vx;
    float vy;
    float vz;
    FollowTarget flw;
    Action bhv;



    private void Awake()
    {
        main = Camera.main;
        pivot = GameObject.Find("Camera Pivot").transform;
        flw = pivot.GetComponent<FollowTarget>();
        main.transform.localPosition = Vector3.back * camDist;
        tarZoom = defZoom = main.orthographicSize;
        Reset();
    }

    private void LateUpdate()
    {

        bhv();
    }

    public void Camshake()
    {
        Boot.twn.Shake(main.transform, .4f, new SShake(40.0f, .6f));
    }

    public void Focus(Transform tgt)
    {
        if (Boot.Logs.cam) Debug.Log("Focusing: " + tgt.name);
        flw.SetTarget(tgt);
    }

    public void Zoom(float z)
    {
        if (Boot.Logs.cam) Debug.Log("Zooming: " + z);
        tarZoom = z;
    }

    public void Rotate(float ang)
    {
        tarRot = ang;// Mathf.MoveTowardsAngle(tarRot, ang, 360.0f);
        if (Boot.Logs.cam) Debug.Log("Rotating to: " + ang);
    }
    public void Follow(Fish f)
    {
        //flw.SetTargets(new Transform[2]{f.transform, Boot.player.transform});
        flw.SetTarget(f.transform);
        flw.offset.y = -4f;
        Zoom(defZoom - 1.0f);
        var dir = f.transform.position - Boot.player.transform.position;
        var ang = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        Boot.cam.Rotate(ang);
    }

    public void Reset()
    {   
        Focus(Boot.player.transform);
        tarZoom = defZoom;
        bhv = StandardBhv;
        flw.offset = Vector3.zero;
    }

    void StandardBhv()
    {
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

    public Quaternion LookCamRotation() => Quaternion.LookRotation(Boot.cam.main.transform.forward, Vector3.up);
}
