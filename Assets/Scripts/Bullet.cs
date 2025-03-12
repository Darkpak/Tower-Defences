using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDamage = 1;
    
    private Transform _target;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }
    
    private void FixedUpdate()
    {
        if (!_target) return;
        Vector2 direction = (_target.position - transform.position).normalized;
        
        rb.linearVelocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }
}
