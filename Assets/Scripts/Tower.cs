using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(HealthSystem))]
public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    private TowerData _towerData;
    private HealthSystem _healthSystem;

    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    public void Initialize(TowerData data)
    {
        _towerData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _towerData.LoadSprite();

        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.Initialize(_towerData.health);
        _healthSystem.OnDeath += Die;
    }

    private void Die()
    {
        EntityManager.Instance.DestroyTower(gameObject);
    }

    public int GetGoldValue()
    {
        return _towerData.goldValue;
    }

    Vector3? GetTargetPosition()
    {
        var monsters = GameObject.FindGameObjectsWithTag("Monster");
        var targets = monsters.Where(monster =>
            {
                var distance = Vector3.Distance(monster.transform.position, transform.position);
                return distance < _towerData.range && distance != 0;
            }).ToArray();


        if (targets.Length == 0)
            return null;

        return targets[Random.Range(0, targets.Length)].transform.position;
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            var targetPosition = GetTargetPosition();
            if (targetPosition.HasValue)
            {
                var rotation = Quaternion.LookRotation(Vector3.forward,
                    targetPosition.Value - transform.position);

                var newBullet = Instantiate(bullet, transform.position, rotation);
                newBullet.GetComponent<Bullet>().Initialize(_towerData.damage);

                newBullet.GetComponent<Rigidbody2D>().linearVelocity =
                    Vector3.Normalize(targetPosition.Value - transform.position) * bulletSpeed;
            }

            yield return new WaitForSeconds(1.0f / _towerData.fireSpeed);
        }
    }
}