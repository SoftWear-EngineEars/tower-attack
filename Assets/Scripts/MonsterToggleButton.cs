using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MonsterToggleButton : MonoBehaviour
{
    public Tier monsterTier;
    private Toggle _toggle;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
    }

    public bool IsSelected()
    {
        return _toggle.isOn;
    }
}