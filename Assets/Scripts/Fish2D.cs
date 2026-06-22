using UnityEngine;

public class Fish2D : MonoBehaviourWithEvents, IProduct
{
    FishData dat;


    public void SetDat(FishData dat)
    {
        if (dat == null) return;
        if (Boot.Logs.fish) Debug.Log("init " + dat.name, gameObject);
        this.dat = dat;
        GetComponentInChildren<SpriteRenderer>().sprite = dat.spr;
    }
}
