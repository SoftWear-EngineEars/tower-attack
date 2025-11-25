using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Scriptable Objects/Tower")]
public class TowerData : EntityData
{
    public int range;
    public int goldValue;

    public Sprite LoadSprite()
    {
        return LoadSprite("Tower");
    }
}