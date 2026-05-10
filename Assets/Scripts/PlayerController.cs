using UnityEngine;
using UnityEngine.Events;



public class PlayerController : MonoBehaviour
{
    FSM<PlayerController>   fsm;
    PlayerFishingState           fishingState;
    PlayerBattleState            battleState;

    public bool isFishing;
    public float        lineL = 6.0f;
    public GameObject   hook;
    public UnityEvent   onStartFishing;


    private void Awake()
    {
        if (hook == null)
        {
            hook = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hook.transform.SetParent(null);
            hook.name = "Hook";
        }

        fishingState    = new PlayerFishingState(this);
        battleState     = new PlayerBattleState(this);
        fsm = fishingState;
        fishingState.OnEnter(this);
    }

    private void Update()
    {
        fsm.Tick(this);
    }

    void ChangeFSM(FSM<PlayerController> o)
    {
        if (Boot.Logs.fsm) Debug.Log("Change FSM state from " + fsm.GetType().Name + " to " + o.GetType().Name, gameObject);
        fsm.OnEnd(this);
        fsm = o;
        fsm.OnEnter(this);
    }

    public void EndFishing()
    {
        fishingState.EndFishing();
    }

    public void BattleMode()
    {
        ChangeFSM(battleState);
    }

    public void Reset()
    {
        ChangeFSM(fishingState);
    }
}
