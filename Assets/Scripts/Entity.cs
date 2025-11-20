using UnityEngine;

public abstract class Entity : ScriptableObject
{
    [SerializeField] protected Tier _tier;

    [SerializeField] protected int _health;
    [SerializeField] protected int _damage;
    
    private Sprite _sprite; 

    protected virtual void Awake()
    {
        // TODO: sprite thing
    }
}
