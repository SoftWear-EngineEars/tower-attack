using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class MonsterData : EntityData
{

    public int cost;
    public float speed;
    public Sprite LoadSprite()
    {
        return LoadSprite("Monster");
    }
}