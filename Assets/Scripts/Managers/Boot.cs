using UnityEngine;

[ExecuteAlways]
public class Boot : MonoBehaviour
{
    public static bool isInit = false;

    public static GameManager   game;
    public static UIManager     ui;
    public static FishManager   fish;
    public static CameraManager cam;
    public static BattleManager bat;
    public static TweenManager  twn;
    public static ControlsManager con;

    public static PlayerController player;


    private void OnEnable()
    {
        if (isInit) return;
        if (Log.boot) Debug.Log("init boot");

        T init<T>() where T : Component
        {
            T res = GetComponentInChildren<T>();
            if (res == null)
            {
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(transform);
                res = go.AddComponent<T>();
                if (Log.boot){
                    Debug.Log("create: " + typeof(T).Name);
                }
            }
            else {
                if (Log.boot) {
                    Debug.Log("init: " + typeof(T).Name);
                }
            }
            return res;
        }

        game    = init<GameManager>();
        ui      = init<UIManager>();
        fish    = init<FishManager>();
        cam     = init<CameraManager>();
        bat     = init<BattleManager>();
        twn     = init<TweenManager>();
        con     = init<ControlsManager>();
        player  = FindFirstObjectByType<PlayerController>();

        bat.enabled = false;

        isInit = true;
    }

    private void OnDestroy()
    {
        isInit = false;
        if (Log.boot) 
            Debug.LogWarning("boot destruction");
    }


    public static MissingReferenceException MissingPlayerReference()
    {
        throw new MissingReferenceException("No player reference in Boot");
    }
}
