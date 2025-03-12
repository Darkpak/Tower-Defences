using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;

    private List<Transform> _path;
    private int _pathIndex = 0;

    private float _baseSpeed;

    private void Start()
    {
        _baseSpeed = moveSpeed;
        if (_path != null && _path.Count > 0)
        {
            transform.position = _path[0].position; // Start at first waypoint
        }
    }

    private void Update()
    {
        if (_path == null || _path.Count == 0) return;

        if (Vector2.Distance(_path[_pathIndex].position, transform.position) <= 0.1f)
        {
            _pathIndex++;

            if (_pathIndex >= _path.Count)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                PlayerHealth.main.TakeDamage(1);
                Destroy(gameObject);
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_path == null || _pathIndex >= _path.Count) return;

        Vector2 direction = (_path[_pathIndex].position - transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    public void SetPath(List<Transform> newPath)
    {
        _path = newPath;
        _pathIndex = 0;
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = _baseSpeed;
    }
}