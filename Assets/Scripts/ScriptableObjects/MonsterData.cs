using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class MonsterData : EntityData
{
    public Sprite LoadSprite()
    {
        return LoadSprite("Monster");
    }
}