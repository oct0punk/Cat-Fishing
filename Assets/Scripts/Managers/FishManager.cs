using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;



public class FishManager : MonoBehaviour
{
    [SerializeField] FishFactory fishFac;
    public FishData data;
    public Dictionary<FishData, int> coll = new();

    public void Spawn()
    {
        if (Boot.game.State != GameState.Fishing)   return;
        if (fishFac.all.Count >= Boot.Datas.maxFishCount) return;

        Fish f = fishFac.Create() as Fish;
        f.Init(data);
        if (!coll.ContainsKey(data))
            coll.Add(data, 0);
    }

    public void Catch(Fish fish)
    {
        coll[fish.dat]++;
        fish.enabled = false;
        if (Boot.Logs.fish) Debug.Log("Catch " + fish.dat.id + ": " + coll[fish.dat]);
    }

    public void Kill(Fish f)
    {
        Destroy(f.gameObject);
    }

    public void FleeAll(Fish exception = null)
    {
        foreach (Fish f in fishFac.all)
        {
            if (f == exception) continue;
            f.Flee();
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 1.0f, .20f);
    }
}
