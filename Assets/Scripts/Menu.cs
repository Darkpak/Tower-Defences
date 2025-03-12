using System;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;
    [SerializeField] private TextMeshProUGUI waveUI;
    [SerializeField] private TextMeshProUGUI enemiesLeftUI;
    [SerializeField] private Animator anim;

    private bool _isMenuOpen = true;

    public void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;
        anim.SetBool("MenuOpen", _isMenuOpen);
    }

    private void OnGUI()
    {
        currencyUI.text = LevelManager.main.currency.ToString();
        waveUI.text = $"Wave: {EnemySpawner.main.currentWave}";
        enemiesLeftUI.text = $"Enemies Left: {EnemySpawner.main.enemiesAlive}";
    }

    public void SetSelected()
    {
        
    }
}
