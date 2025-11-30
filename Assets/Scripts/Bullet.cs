using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int _damage;

    public void Initialize(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Monster"))
            return;
        
        other.GetComponent<Monster>().Damage(_damage);
    }
}
