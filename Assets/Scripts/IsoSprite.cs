using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[ExecuteAlways]
public class IsoSprite : MonoBehaviour
{
    public static List<IsoSprite> all = new();

    private void Awake()
    {        
        all.Add(this);
        if (Boot.Logs.iso)
            Debug.Log("Add 1 item to all isoSpr. Count: " + all.Count);
    }

    private void OnDestroy()
    {
        all.Remove(this);
        if (Boot.Logs.iso)
            Debug.Log("Remove 1 item from all isoSpr. Count: " + all.Count);
    }

    public void Tick()
    {
        if (Boot.Logs.iso)
            Debug.Log("Tick iso", gameObject);

        Quaternion lookRot = Boot.cam.LookCamRotation();
        transform.rotation = lookRot;// * Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward);
    }

#if UNITY_EDITOR
    [MenuItem("Developer/Rotate all iso sprites")]
#endif
    public static void TickAllComponents()
    {
        all.Clear();
        foreach (var iso in Resources.FindObjectsOfTypeAll(typeof(IsoSprite)))
        {
            all.Add(iso.GetComponent<IsoSprite>());
            if (Boot.Logs.iso) Debug.Log("add: " + iso.name + ", Count: " + all.Count);
        }
        TickAll();
    }

    public static void TickAll()
    {
        if (Boot.Logs.iso)
            Debug.Log("Tick all, Count: " + all.Count);
        foreach (var spr in all)
        {
            spr.Tick();
        }
    }
}
