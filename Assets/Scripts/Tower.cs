using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData towerData;

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(TowerData data)
    {
        towerData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = towerData.LoadSprite();
    }

    public int GetGoldValue()
    {
        return towerData.goldValue;
    }
}
