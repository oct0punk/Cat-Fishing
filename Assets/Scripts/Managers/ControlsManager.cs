using UnityEngine;
using UnityEngine.Events;


public class ControlsManager : MonoBehaviour
{
    bool touchFlag = false;

    public Vector2 tPos { get; private set; }
    public UnityEvent onTouch;
    public UnityEvent onTouchBegin;
    public UnityEvent onTouchUp;


    private void Update()
    {
        // Focus on index 0
        if (Input.touches.Length <= 0) return;
        Touch t = Input.touches[0];

        tPos = t.position;
        switch (t.phase)
        {
            case TouchPhase.Began:
                if (Boot.Logs.ev) Debug.Log("Invoke onTouchBegin");
                onTouchBegin?.Invoke();
                if (Boot.Logs.ev) Debug.Log("Invoke onTouch");
                //onTouch?.Invoke();
                break;
            case TouchPhase.Moved:
                if (Boot.Logs.ev) Debug.Log("Invoke onTouch");
                //onTouch?.Invoke();
                break;
            case TouchPhase.Ended:
                if (touchFlag)
                {
                    touchFlag = false;
                    if (Boot.Logs.inp) Debug.Log("re-enable touch after pointer up");
                    break;
                }
                if (Boot.Logs.ev) Debug.Log("Invoke onTouchUp");
                onTouchUp?.Invoke();
                break;
        }
        onTouch?.Invoke();
    }

    public bool IsLeftSidedTouch()
    {
        return tPos.x < Screen.width * 0.5f;
    }
    public Vector2 GetTouchScreenPos()
    {
        return new Vector2(
            tPos.x / Screen.width, 
            tPos.y / Screen.height);
    }
    public void TouchFlag()
    {
        if (Input.touchCount > 0)
        {
            if (Boot.Logs.inp) Debug.Log("lock touch while pointer is down");
            touchFlag = true;
        }
    }
}
