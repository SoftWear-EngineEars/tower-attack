using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Scriptable Objects/Tower")]
public class Tower : ScriptableObject
{
    private Tier _tier;
    
    private int _damage;
    private int _health;
    private int _range;

    private Sprite _sprite; // make it set automatically based on _tier when sprites are in
}