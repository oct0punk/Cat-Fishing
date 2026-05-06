using UnityEngine;
using UnityEngine.Events;

public class MonoBehaviourWithEvents : MonoBehaviour
{
    public UnityEvent onDestroy;

    private void OnDestroy()
    {
        if (onDestroy.GetPersistentEventCount() > 0)
            if (Log.ev) Debug.Log("Call onDestroy", gameObject);
        onDestroy.Invoke();
    }
}
