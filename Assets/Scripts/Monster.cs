using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [Tooltip("How often the monster checks for towers to move to (seconds)")] 
    [SerializeField] private float towerCheckRate = 0.75f; // tentative; we might want it to check slower if more lag or faster if it gets annoying

    [Tooltip("How often the monster damages a tower (seconds)")] 
    [SerializeField] private float damageDelay = 1.0f;

    [Tooltip("The distance (center to center) at which the monster is able to damage a target")]
    [SerializeField] private float reach = 1.5f;

    private MonsterData _monsterData;
    
    private SpriteRenderer _spriteRenderer;
    private PolygonCollider2D _collider;

    private int _health;
    private GameObject _targetTower;
    
    void Start()
    {
        _collider = GetComponent<PolygonCollider2D>();
        _health = _monsterData.health;
        StartCoroutine(FindTarget());
        StartCoroutine(AttackTower());
    }

    
    void Update()
    {
        var newPosition = Vector2.MoveTowards(transform.position, _targetTower.transform.position, _monsterData.speed * Time.deltaTime);
        if (Vector2.Distance(newPosition, _targetTower.transform.position) != 0) // we don't move to the same position as the tower, otherwise the tower can't shoot us. That would be unfair.
            transform.position = newPosition;
    }

    public void Initialize(MonsterData data)
    {
        _monsterData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _monsterData.LoadSprite();
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            var towers = GameObject.FindGameObjectsWithTag("Tower");

            _targetTower = towers[0];
        
            // find the closest tower; make _targetPosition that position
            for (var i = 1; i < towers.Length; i++)
            {
                if (Vector3.Distance(towers[i].transform.position, transform.position) <
                    Vector3.Distance(_targetTower.transform.position, transform.position))
                {
                    _targetTower = towers[i];
                }
            }

            yield return new WaitForSeconds(towerCheckRate); 
        }
    }

    private bool IsTouchingTower()
    {
        return Vector3.Distance(_targetTower.transform.position, transform.position) < reach;
    }
    private IEnumerator AttackTower()
    {
        while (true)
        {
            Debug.Log(IsTouchingTower());
            if (IsTouchingTower())
            {
                _targetTower.GetComponent<Tower>().Damage(_monsterData.damage);
            }
            yield return new WaitForSeconds(damageDelay);
        }
    }

    public void Damage(int health)
    {
        _health -= health;
        if (_health <= 0)
            Destroy(gameObject);
    }
}
