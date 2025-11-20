using UnityEngine;
using UnityEngine.Serialization;

public abstract class EntityData : ScriptableObject
{
    public Tier tier;

    public int health;
    public int damage;
    
    public abstract Sprite LoadSprite();
}
