using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PathData
{
    public List<Transform> waypoints;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform[] startPoints;
    public List<PathData> paths;

    public int currency;
    
    private void Awake()
    {
        main = this;
    }
    
    private void Start()
    {
        currency = 100;
    }
    
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }
    
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase that.");
            return false;
        }
    }
    
    public Transform GetRandomStartPoint()
    {
        return startPoints[Random.Range(0, startPoints.Length)];
    }

    public List<Transform> GetPathForStartPoint(Transform startPoint)
    {
        int index = System.Array.IndexOf(startPoints, startPoint);
        if (index >= 0 && index < paths.Count)
        {
            return paths[index].waypoints;
        }
        return null;
    }
}
