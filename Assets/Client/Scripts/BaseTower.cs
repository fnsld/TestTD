using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    [SerializeField] private Transform muzzle;
    
    [SerializeField] private Transform cannon;

    [SerializeField] private TextMeshPro textMesh;

    [SerializeField] private TextMeshPro textLevelUP;
    
    [SerializeField] private float upgradeCost = 5;

    [SerializeField] private float upgradeCostMod = 1.1f;

    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] protected float damage = 1;
    
    [SerializeField] protected float rotateSpeed = 8;
    
    [SerializeField] protected float fireCooldown = 0.3f;
    
    protected BaseEnemy currentEnemy;

    protected int currentLevel = 1;
    
    private List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();

    private Quaternion initialRotation;

    protected float lastFireTime;

    private Outline outline;

    private bool isSelected => textLevelUP.enabled;
    
    private void Awake()
    {
        outline = GetComponent<Outline>();
        initialRotation = cannon.rotation;
    }

    public void Select()
    {
        outline.OutlineWidth = 6f;
        textLevelUP.enabled = true;
        var available = (int)upgradeCost <= Bus.PlayerGold;
        textLevelUP.text = available ? "Upgrade available\nClick to upgrade!" : $"Upgrade cost:\n{(int)upgradeCost}";
        textLevelUP.color = available ? Color.green : Color.red;
    }
    
    public void Deselect()
    {
        outline.OutlineWidth = 0;
        textLevelUP.enabled = false;
    }

    public void Upgrade()
    {
        if (!isSelected) 
            return;
        
        if (Bus.PlayerGold < (int)upgradeCost) 
            return;
        
        currentLevel++;
        
        textMesh.text = currentLevel.ToString();
        
        Bus.PlayerGold -= (int)upgradeCost;
        
        Bus.OnGoldChanged.Invoke();
        
        fireCooldown *= 0.9f;

        damage *= 1.1f;

        upgradeCost *= upgradeCostMod;

        RandomizeScale(cannon);
        RandomizeScale(muzzle);
        
        transform.localScale = new Vector3(
            transform.localScale.x * 1.01f,
            transform.localScale.y * 1.01f,
            transform.localScale.z * 1.01f);
    }

    private void RandomizeScale(Transform t)
    {
        t.localScale = new Vector3(
            t.localScale.x * UnityEngine.Random.Range(0.6f, 1.4f),
            t.localScale.y * UnityEngine.Random.Range(0.7f, 1.3f),
            t.localScale.z* UnityEngine.Random.Range(0.6f, 1.4f));
    }

    private void FixedUpdate()
    {
        currentEnemy = GetNearbyEnemy();
        
        RotateCannonToCurrentEnemy();
        
        Fire(currentEnemy);
    }

    protected virtual void Fire(BaseEnemy enemy) { }
    
    private BaseEnemy GetNearbyEnemy()
    {
        float minDist = float.MaxValue;
        BaseEnemy current = null;
        
        for (int i = enemiesInRange.Count - 1; i >= 0; i--)
        {
            var enemy = enemiesInRange[i];
            
            if (enemy == null)
            {
                enemiesInRange.Remove(enemy);
                continue;
            }
            
            var dist = Vector3.Distance(enemy.transform.position, transform.position);
            if (dist < minDist)
            {
                current = enemy;
                minDist = dist;
            }
        }
       
        return current;
    }

    private bool IsEnemyLayer(int layer) => enemyLayers == (enemyLayers | (1 << layer));
    
    private void OnTriggerEnter(Collider other)
    {
        if (!IsEnemyLayer(other.gameObject.layer)) 
            return;
        
        var enemy = other.gameObject.GetComponentInParent<BaseEnemy>();
        if (enemy)
            enemiesInRange.Add(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsEnemyLayer(other.gameObject.layer)) 
            return;
        
        var enemy = other.gameObject.GetComponentInParent<BaseEnemy>();
        if (enemy)
            enemiesInRange.Remove(enemy);
    }

    private void RotateCannonToCurrentEnemy()
    {
        if (currentEnemy != null)
        {
            var correctEnemyPos = currentEnemy.transform.position;
            correctEnemyPos.y += 0.3f;
            var direction = (cannon.position - correctEnemyPos).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            cannon.rotation = Quaternion.Slerp(cannon.rotation, lookRotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            cannon.rotation = Quaternion.Slerp(cannon.rotation, initialRotation, Time.deltaTime * 2);
        }
    }
    
}
