using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;


public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;
    
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject bossPrefab;

    [Header("Attributes")] 
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondsCap = 15f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();
    
    private float _timeSinceLastSpawn;
    private int _enemiesLeftToSpawn;
    private float _eps; // enemies per second
    private bool _isSpawning;
    
    [HideInInspector] public int currentWave = 1;
    [HideInInspector] public int enemiesAlive;

    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }

    private void Start()
    {
        StartCoroutine(StartWave());
    }
    
    private void Update()
    {
        if (!_isSpawning) return;
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= (1f / _eps) && _enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            _enemiesLeftToSpawn--;
            enemiesAlive++;
            _timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive == 0 && _enemiesLeftToSpawn == 0)
        {
            EndWave();
        }
    }

    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        _isSpawning = true;
        if (currentWave % 5 == 0)
        {
            SpawnBoss();
            _enemiesLeftToSpawn = 0;
        }
        else
        {
            _enemiesLeftToSpawn = EnemiesPerWave();
            _eps = EnemiesPerSecond();
        }
    }

    private void EndWave()
    {
        _isSpawning = false;
        _timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void SpawnBoss()
    {
        Transform spawnPoint = LevelManager.main.GetRandomStartPoint();
        List<Transform> enemyPath = LevelManager.main.GetPathForStartPoint(spawnPoint);
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
        EnemyMovement enemyMovement = boss.GetComponent<EnemyMovement>();
        
        if (enemyMovement != null)
        {
            enemyMovement.SetPath(enemyPath);
        }
    }

    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave,
            difficultyScalingFactor), 0f, enemiesPerSecondsCap);
    }
    
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        GameObject prefabToSpawn = enemyPrefabs[index];

        Transform spawnPoint = LevelManager.main.GetRandomStartPoint();
        List<Transform> enemyPath = LevelManager.main.GetPathForStartPoint(spawnPoint);

        if (enemyPath == null || enemyPath.Count == 0) 
        {
            Debug.LogError("No path found for spawn point!");
            return;
        }

        GameObject enemy = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
        enemy.GetComponent<EnemyMovement>().SetPath(enemyPath); // ✅ Assign path
    }
}
