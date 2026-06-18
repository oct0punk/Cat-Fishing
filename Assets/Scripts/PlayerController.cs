using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;



public class PlayerController : MonoBehaviour, IPointerClickHandler
{
    PlayerFSM fsm;

    public bool isFishing;
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
        fsm = new PlayerFSM(this);
    }

    private void FixedUpdate()
    {
        fsm.Tick();
    }


    public void CatchMode()     => fsm.CatchState();
    public void BattleMode()    => fsm.BattleState();
    public void FishingMode()   => fsm.FishingState();


    public void OnPointerClick(PointerEventData eventData)
    {
        Boot.game.OpenMenu();
    }
}
