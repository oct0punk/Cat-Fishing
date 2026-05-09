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
        return;
        bhv();
    }

    public void Camshake()
    {
        Boot.twn.Shake(main.transform, .4f, new SShake(40.0f, .6f));
    }

    public void Focus(Transform tgt)
    {
        if (Log.cam) Debug.Log("Focusing: " + tgt.name);
        flw.SetTarget(tgt);
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
    public void Follow(Fish f)
    {
        var dir = f.transform.position - Boot.player.transform.position;
        var ang = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        flw.SetTargets(new Transform[2]{f.transform, Boot.player.transform});
        Zoom(defZoom - 1.0f);
        Rotate(ang);
        bhv = FollowFishBhv;
    }

    public void Reset()
    {   
        Focus(Boot.player.transform);
        tarZoom = defZoom;
        //tarRot = .0f;
        bhv = StandardBhv;
    }

    void StandardBhv()
    {
        if (main.orthographicSize != tarZoom)
        {
            main.orthographicSize = Mathf.SmoothDamp(main.orthographicSize, tarZoom, ref zoomVel, Time.deltaTime * zoomSpeed);
        }

        if (Mathf.DeltaAngle(pivot.eulerAngles.y, tarRot) > 0.000001f)
        {
            var rot = pivot.eulerAngles;
            rot.y = Mathf.SmoothDampAngle(rot.y, tarRot, ref rotVel, rotDuration);
            pivot.eulerAngles = rot;
            IsoSprite.TickAll();
        }
    }

    void FollowFishBhv()
    {
        Fish f = Boot.bat.conf.fish;
        PlayerController p = Boot.player;
        var med = (f.transform.position + p.transform.position) * .5f;
        var pvp = pivot.position;
        pvp.x = Mathf.SmoothDamp(pvp.x, med.x, ref vx, Boot.Datas.CamFollowSmoothing);
        pvp.y = Mathf.SmoothDamp(pvp.y, med.y, ref vy, Boot.Datas.CamFollowSmoothing);
        pvp.z = Mathf.SmoothDamp(pvp.z, med.z, ref vz, Boot.Datas.CamFollowSmoothing);
        pivot.position = pvp;
    }

    public Quaternion LookCamRotation() => Quaternion.LookRotation(Boot.cam.main.transform.forward, Vector3.up);
}
