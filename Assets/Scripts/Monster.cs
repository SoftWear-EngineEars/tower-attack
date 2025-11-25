using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonsterData monsterData;

    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {

    }

    
    void Update()
    {
        
    }

    public void Initialize(MonsterData data)
    {
        monsterData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = monsterData.LoadSprite();
    }
}
