using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Scriptable Objects/Tower")]
public class TowerData : EntityData
{
    public int range;

    public override Sprite LoadSprite()
    {
        return Resources.Load<Sprite>("Sprites/Towers/Tower" + ((int)tier + 1));
    }
}