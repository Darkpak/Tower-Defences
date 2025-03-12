using System.Collections;
using UnityEngine;
using UnityEditor;

public class SlowmoTower : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private LayerMask enemyMask;
    
    [Header("Attributes")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float aps = 2f; // attacks per second
    [SerializeField] private float freezeTime = 1f;
    
    private float _timeUntilFire;
    void Update()
    {
        _timeUntilFire += Time.deltaTime;
        
        if(_timeUntilFire >= 1 / aps)
        {
            FreezeEnemies();
            _timeUntilFire = 0f;
        }
    }

    private void FreezeEnemies()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,
            (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);
        em.ResetSpeed();
    }
    
    
    
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
    
}
