using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "Scriptable Objects/FishData")]
public class FishData : ScriptableObject
{
    public string   id = "Fish";
    public int      hp = 10;
    public int      force = 10;
    public float    normalSpeed = 3.0f;
    public float    chargeSpeed = 12.0f;
    public float    pullingSpeed = 8.0f;
    public Sprite   spr;
}
