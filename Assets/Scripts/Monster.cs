using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Monster : MonoBehaviour
{
    [Tooltip("How often the monster checks for towers to move to (seconds)")]
    [SerializeField] private float towerCheckRate = 0.75f;

    [Tooltip("How often the monster damages a tower (seconds)")]
    [SerializeField] private float damageDelay = 1.0f;

    [Tooltip("The distance (center to center) at which the monster is able to damage a target")]
    [SerializeField] private float reach = 1.5f;

    private MonsterData _monsterData;
    private HealthSystem _healthSystem;
    private SpriteRenderer _spriteRenderer;
    private GameObject _targetTower;

    void Start()
    {
        StartCoroutine(FindTarget());
        StartCoroutine(AttackTower());
    }

    void Update()
    {
        if (_targetTower == null) return;

        var newPosition = Vector2.MoveTowards(transform.position, _targetTower.transform.position, _monsterData.speed * Time.deltaTime);
        if (Vector2.Distance(newPosition, _targetTower.transform.position) != 0)
            transform.position = newPosition;
    }

    public void Initialize(MonsterData data)
    {
        _monsterData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _monsterData.LoadSprite();

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize(_monsterData.health);
        _healthSystem.OnDeath += Die;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator FindTarget()
    {
        while (true)
        {
            var towers = GameObject.FindGameObjectsWithTag("Tower");
            if (towers.Length > 0)
            {
                _targetTower = towers[0];

                for (var i = 1; i < towers.Length; i++)
                {
                    if (Vector3.Distance(towers[i].transform.position, transform.position) <
                        Vector3.Distance(_targetTower.transform.position, transform.position))
                    {
                        _targetTower = towers[i];
                    }
                }
            }
            yield return new WaitForSeconds(towerCheckRate);
        }
    }

    private bool IsTouchingTower()
    {
        return _targetTower != null && Vector3.Distance(_targetTower.transform.position, transform.position) < reach;
    }

    private IEnumerator AttackTower()
    {
        while (true)
        {
            if (IsTouchingTower())
            {
                _targetTower.GetComponent<HealthSystem>()?.TakeDamage(_monsterData.damage);
                if (_targetTower.GetComponent<Tower>().IsCenterTower()) 
                    Destroy(gameObject);
            }
            yield return new WaitForSeconds(damageDelay);
        }
    }
}
