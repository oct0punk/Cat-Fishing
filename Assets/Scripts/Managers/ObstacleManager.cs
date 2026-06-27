using System.Collections;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    Coroutine co;
    [SerializeField] RockFactory fac;

    [Min(0f)] public float inter = 1.0f;


    public void StartSpawning()
    {
        Debug.Log("rock Start");
        co = StartCoroutine(SpawnRocks());
    }

    public void StopSpawning()
    {
        Debug.Log("rock Stop");
        if (co != null) StopCoroutine(co);
        else 
        {
            Debug.LogWarning("No spawn coroutine", gameObject);
            StopAllCoroutines();
        }
    }
    
    IEnumerator SpawnRocks()
    {
        yield return Helpers.Wait((Random.value +1) * inter);

        Debug.Log("rock Spawn");
        Vector3 pos = Boot.player.transform.position;
        var upBorderPos = Helpers.ScreenToWater(Boot.cam.main.ViewportToScreenPoint(new Vector2(.5f,1f)));
        var dir = upBorderPos - pos;
        dir.Normalize();
        var dist = dir.magnitude + 1f;
        pos = upBorderPos + dir * dist;
        pos.y = 0.0f;
        Rock r = fac.Create();
        r.transform.SetPositionAndRotation(pos, Helpers.RandomRotation());

        co = StartCoroutine(SpawnRocks());
    }
}
