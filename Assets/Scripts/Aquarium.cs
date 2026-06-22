using UnityEngine;
using UnityEngine.UI;

public class Aquarium : MonoBehaviour
{
    [Min(0)] public int spawnFish;
    public Fish2DFactory fishFac;
    public Transform lbd;
    public Transform rbd;
    public Button backButton;

    private void Awake()
    {
        if (Boot.game)
        {
            if (Boot.Logs.aqu) Debug.Log("Boot game found");
            backButton.onClick.AddListener(Boot.game.ExitAquarium);
        }
        else
            Debug.LogWarning("Boot game missing");


        if (Boot.fish != null)
        {
            foreach (var f in Boot.fish.coll)
            {
                for (int i = 0; i < f.Value; i++)
                {
                    Spawn(f.Key);
                }
            }
        }
        for (int i = 0; i < spawnFish; i++)
        {
            Spawn(null);
        }
    }

    private void Spawn(FishData dat)
    {
        Fish2D f = fishFac.Create();
        f.SetDat(dat);
        f.transform.SetPositionAndRotation(
            new Vector3(
                Random.Range(lbd.position.x + 1.5f, rbd.position.x - 1.5f),
                (Random.value-.5f) * 2f*(Camera.main.orthographicSize-1f), 
                0f), 
            Quaternion.Euler(0f, 0f, Random.value * 360f));
    }
}
