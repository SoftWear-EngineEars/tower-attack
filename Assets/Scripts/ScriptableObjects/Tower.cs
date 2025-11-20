using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "Scriptable Objects/Tower")]
public class Tower : Entity
{
    [SerializeField] private int _range;

    protected override Sprite GetSprite()
    {
        return null;
    }
}