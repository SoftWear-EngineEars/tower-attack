using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;

    public void Initialize(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Monster"))
            return;
        
        other.GetComponent<HealthSystem>()?.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
