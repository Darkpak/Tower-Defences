using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Tower[] towers;

    private int _selectedTower = 0;
    private void Awake()
    {
        main = this;
    }

    public Tower GetSelectedTower()
    {
        return towers[_selectedTower];
    }
    
    public void SetSelectedTower(int selectedTower)
    {
        _selectedTower = selectedTower;
    }
}
