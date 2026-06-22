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

    public void SetActive(bool val)
    {
        gameObject.SetActive(val);
        hook.SetActive(val);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Boot.con.isWaitingForNoTouch) return;
        if (Boot.Logs.inp) Debug.Log(name + " Game Object Clicked!");
        Boot.game.OpenMenu();
    }
}
