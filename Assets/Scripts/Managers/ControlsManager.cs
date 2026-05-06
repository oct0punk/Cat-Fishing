using UnityEngine;
using UnityEngine.Events;


public class ControlsManager : MonoBehaviour
{
    public Vector2 tPos { get; private set; }
    public UnityEvent onTouch;
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
            case TouchPhase.Moved:
                if (Log.ev) Debug.Log("Invoke onTouch");
                onTouch?.Invoke();
                break;
            case TouchPhase.Ended:
                if (Log.ev) Debug.Log("Invoke onTouchUp");
                onTouchUp?.Invoke();
                break;
        }
    }
}
