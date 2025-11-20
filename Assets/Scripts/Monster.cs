using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class Monster : ScriptableObject
{
    private Tier _tier;

    private int _health;
    private int _damage;

    private Sprite _sprite; // make it set automatically based on _tier when sprites are in
}