using UnityEngine;

public class TargetPositionBehaviour : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    
    private bool IsEnemyLayer(int layer) => enemyLayers == (enemyLayers | (1 << layer));
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsEnemyLayer(other.gameObject.layer))
        {
            var enemy = other.gameObject.GetComponentInParent<BaseEnemy>();
            if (enemy)
            {
                Bus.PlayerHealth -= (int)enemy.GetDamage();
                Bus.OnEnemyStepTarget.Invoke(enemy);
                
                Destroy(enemy.gameObject);
            }
        }
    }
}
