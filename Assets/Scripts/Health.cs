using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int hitPoints = 2;
    [SerializeField] private int currencyWorth = 50;
    [SerializeField] private Slider healthSlider;
    
    private bool _isDestroyed = false;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = hitPoints;
        healthSlider.maxValue = hitPoints;
    }

    public void TakeDamage(int dmg)
    {
        hitPoints -= dmg;

        if (hitPoints <= 0)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            _isDestroyed = true;
            Destroy(gameObject);
        }
    }

    private void OnGUI()
    {
        healthSlider.value = hitPoints;
    }
}
