using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourWithEvents : MonoBehaviour
{
    [HideInInspector] public UnityEvent onDestroy;

    private void OnDestroy()
    {
        if (onDestroy.GetPersistentEventCount() > 0)
            if (Boot.Logs.ev) Debug.Log("Call onDestroy", gameObject);
        onDestroy.Invoke();
    }
}
