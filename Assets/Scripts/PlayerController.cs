using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public enum EPlayerState
{
    Standby,
    Fishing,
    Battle
}


public class PlayerController : MonoBehaviour
{
    Vector3                 aimPos;
    EPlayerState            stt;
    FSM<PlayerController>   fsm;
    PlayerFishing           fishingState;
    PlayerBattle            battleState;

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

        fishingState    = new PlayerFishing(this);
        battleState     = new PlayerBattle(this);
        fsm = fishingState;
        fishingState.OnEnter(this);
    }

    private void Update()
    {
        fsm.Tick(this);
    }

    void ChangeFSM(FSM<PlayerController> o)
    {
        fsm.OnEnd(this);
        fsm = o;
        fsm.OnEnter(this);
    }

    public void EndFishing()
    {
        fishingState.EndFishing();
    }
}
