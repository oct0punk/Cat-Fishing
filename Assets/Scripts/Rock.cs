using UnityEngine;

public class Rock : MonoBehaviourWithEvents, IProduct
{
    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (transform.localScale.x >= 1f) return;
        transform.localScale += Vector3.one * Time.deltaTime * 10f;
    }
}
