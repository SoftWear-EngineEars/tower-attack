using System.Collections;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;
    private TowerData _towerData;

    private int _health;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _health = _towerData.health;
        StartCoroutine(Shoot());
    }

    public void Initialize(TowerData data)
    {
        _towerData = data;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _towerData.LoadSprite();
    }

    public int GetGoldValue()
    {
        return _towerData.goldValue;
    }

    private Vector3? GetTargetPosition()
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

    private IEnumerator Shoot()
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
    
    public void Damage(int health)
    {
        _health -= health;
        if (_health <= 0)
        {
            EntityManager.Instance.DestroyTower(gameObject);
        }
    }
}