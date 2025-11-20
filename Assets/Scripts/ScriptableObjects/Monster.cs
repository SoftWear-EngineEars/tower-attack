using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Scriptable Objects/Monster")]
public class Monster : Entity
{

    private Sprite _sprite; // make it set automatically based on _tier when sprites are in

    protected override void Awake()
    {
        if (_tier == Tier.X)
        {
            _tier = Tier.IV;
        }

        base.Awake();
    }

    protected override Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Monsters/Monster" + (_tier + 1));
    }
}