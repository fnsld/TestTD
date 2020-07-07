using Client.Scripts;
using Pathfinding;
using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [SerializeField] private Transform[] bodyParts;
    
    private EnemyStat stat;

    private AIDestinationSetter aids;

    private void Awake()
    {
        aids = GetComponent<AIDestinationSetter>();
    }

    public float GetDamage() => stat.Damage;
    
    public void Initialize(EnemyStat stat)
    {
        this.stat = new EnemyStat()
        {
            Damage = stat.Damage,
            Health = stat.Health,
            Reward = stat.Reward
        };
    }

    public void RandomizeScale()
    {
        foreach (var bodyPart in bodyParts)
        {
            RandomizeScale(bodyPart);
        }
    }

    private void RandomizeScale(Transform transform)
    {
        transform.localScale = new Vector3(
            transform.localScale.x * UnityEngine.Random.Range(0.6f, 1.4f),
            transform.localScale.y * UnityEngine.Random.Range(0.7f, 1.3f),
            transform.localScale.z* UnityEngine.Random.Range(0.6f, 1.4f));
    }
    
    public void OnAttacked(float damage)
    {
        Bus.OnEnemyAttacked.Invoke(this, damage);
        stat.Health -= damage;
        if (stat.Health <= 0)
        {
            Bus.PlayerGold += stat.Reward;
            Bus.EnemiesDead++;
            
            Bus.OnGoldChanged.Invoke();
            
            Bus.OnEnemyDead.Invoke(this);
            
            Destroy(gameObject);
        }
    }

    public override void PrepareAfterPool()
    {
        aids.target = Bus.DestinationPos;
    }
}
