using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fish/FishData")]
public class FishData : ScriptableObject
{
    public string fishName;
    public string rarity;
    public float price;
    public string size;
    public Sprite fishSprite;
}
