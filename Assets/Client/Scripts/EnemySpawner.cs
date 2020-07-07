using System.Collections;
using Client.Scripts;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float waveTime = 5;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float startEnemyHealth = 5;
    [SerializeField] private float startEnemyDamage = 5;
    [SerializeField] private int startEnemyGoldReward = 5;

    private EnemyStat currentEnemyStat;

    private int maxEnemiesInWave;
    private int currentWaveIndex = 1;
    
    private void Start()
    {
        currentEnemyStat = new EnemyStat()
        {
            Damage = startEnemyDamage, 
            Health = startEnemyHealth, 
            Reward = startEnemyGoldReward
        };

        maxEnemiesInWave = currentWaveIndex + Random.Range(1, 5);

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while (true)
        {
            if (!SpawnEnemy())
            {
                yield return new WaitForSeconds(waveTime);
                NewWave();
            }
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    private void NewWave()
    {
        currentWaveIndex++;
        maxEnemiesInWave = currentWaveIndex + Random.Range(1, 5);
        
        var random = Random.Range(0, 3);
        
        if (random == 0)
        {
            currentEnemyStat.Damage += Random.Range(1.0f, 2.0f);
        }
        
        if (random == 1)
        {
            currentEnemyStat.Health += Random.Range(1.0f, 2.0f);
        }
        
        if (random == 2)
        {
            currentEnemyStat.Reward += Random.Range(1, 3);
        }
    }
    
    private bool SpawnEnemy()
    {
        maxEnemiesInWave--;
        if (maxEnemiesInWave <= 0) 
            return false;
        
        var enemy = Pool.Get<BaseEnemy>(spawnTransform.position, enemyPrefab);
        enemy.transform.rotation = Quaternion.Euler(0, 90, 0);
        enemy.Initialize(currentEnemyStat);
        enemy.RandomizeScale();
        
        return true;
    }
}
