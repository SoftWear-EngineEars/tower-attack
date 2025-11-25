using UnityEngine;
using UnityEngine.Serialization;

public abstract class EntityData : ScriptableObject
{
    public Tier tier;

    public int health;
    public int damage;
    
    protected Sprite LoadSprite(string entityType)
    {
        return Resources.Load<Sprite>($"Sprites/{entityType}s/{entityType}{EntityManager.TierToInt(tier)}");
    }
}
