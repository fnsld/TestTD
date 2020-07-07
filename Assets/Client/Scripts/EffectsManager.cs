using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject attackEffectPrefab;
    
    [SerializeField] private GameObject deadEffectPrefab;

    public void Start()
    {
        Bus.OnEnemyDead.AddListener(OnEnemyDead);
        Bus.OnEnemyAttacked.AddListener(OnEnemyAttacked);
    }
    
    private void OnEnemyDead(BaseEnemy enemy)
    {
        Pool.Get<BaseEffect>(enemy.transform.position, deadEffectPrefab);
    }
    
    private void OnEnemyAttacked(BaseEnemy enemy, float damage)
    {
        Pool.Get<BaseEffect>(enemy.transform.position, attackEffectPrefab);
    }
}
