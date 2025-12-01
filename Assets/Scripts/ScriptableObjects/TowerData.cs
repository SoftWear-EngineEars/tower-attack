using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Scriptable Objects/Tower")]
public class TowerData : EntityData
{
    public int range;
    public int goldValue;
    [Tooltip("Shots per second")] public float fireSpeed;
    public bool isFinalTower;

    public Sprite LoadSprite()
    {
        return LoadSprite("Tower");
    }
}