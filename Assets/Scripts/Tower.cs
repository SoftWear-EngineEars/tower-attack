using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData towerData;

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = towerData.LoadSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
