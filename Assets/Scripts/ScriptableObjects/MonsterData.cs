using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class MonsterData : EntityData
{
    public override Sprite LoadSprite()
    {
        return Resources.Load<Sprite>("Sprites/Monsters/Monster" + ((int)tier + 1));
    }
}