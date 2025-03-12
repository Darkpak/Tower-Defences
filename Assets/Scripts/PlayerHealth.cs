using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth main;
    
    [Header("Attributes")]
    public int maxHealth = 10;
    private int _currentHealth;
    
    [Header("UI Reference")]
    public TextMeshProUGUI healthText;
    
    private void Awake()
    {
        main = this;
    }
    
    private void Start()
    {
        _currentHealth = maxHealth;
        UpdateHealthUI();
    }
    
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UpdateHealthUI();
        
        if (_currentHealth <= 0)
        {
            Debug.Log("Game Over");
            // Add game over logic here
        }
    }
    
    private void UpdateHealthUI()
    {
        healthText.text = "Health: " + _currentHealth;
    }
}