using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float bps = 1f; // Firing Rate
    [SerializeField] private int baseUpgradeCost = 100;

    private float _bpsBase;
    private float _targetingRangeBase;

    private Transform _target;
    private float _timeUntilFire;

    private int level = 1;
    
    private void Start()
    {
        _bpsBase = bps;
        _targetingRangeBase = targetingRange;
        
        upgradeButton.onClick.AddListener(Upgrade);
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            FindTarget();
            return;
        }
        else
        {
            _timeUntilFire += Time.deltaTime;
            
            if(_timeUntilFire >= 1 / bps)
            {
                Shoot();
                _timeUntilFire = 0f;
            }
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            _target = null;
        }
        
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(_target);
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            _target = hits[0].transform;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(_target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(_target.position.y - transform.position.y, 
            _target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        turretRotationPoint.rotation = targetRotation;
        //turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation,
        //    targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void OpenUpgradeUI()
    {
        upgradeUI.SetActive(true);
    }

    public void CloseUpgradeUI()
    {
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

    public void Upgrade()
    {
        if (level >= 3)
        {
            Debug.Log("Max level reached.");
        }
        else
        {
            level++;
        }
        
        if (CalculateCost() > LevelManager.main.currency) return;
        
        LevelManager.main.SpendCurrency(CalculateCost());

        bps = CalculateBPS();
        targetingRange = CalculateRange();
        
        CloseUpgradeUI();
        Debug.Log($"New DPS: {bps}");
        Debug.Log($"New Range: {targetingRange}");
        Debug.Log($"New Price: {CalculateCost()}");
    }

    private int CalculateCost()
    {
        return Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
    }

    private float CalculateBPS()
    {
        return _bpsBase * Mathf.Pow(level, 0.6f);
    }

    private float CalculateRange()
    {
        return _targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
