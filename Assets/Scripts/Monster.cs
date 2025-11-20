using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = monsterData.LoadSprite();
    }

    
    void Update()
    {
        
    }
}
