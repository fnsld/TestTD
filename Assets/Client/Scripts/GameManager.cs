using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int startPlayerHealth = 10;
    [SerializeField] private int startPlayerGold = 10;
    
    private void Awake()
    {
        Bus.PlayerHealth = startPlayerHealth;
        Bus.PlayerGold = startPlayerGold;
        Bus.EnemiesDead = 0;
    }

    private void Start()
    {
        Time.timeScale = 1;
        Bus.OnGameStart.Invoke();
        Bus.OnEnemyStepTarget.AddListener(OnEnemyStepTarget);
    }

    private void OnEnemyStepTarget(BaseEnemy enemy)
    {
        if (Bus.PlayerHealth <= 0)
        {
            Time.timeScale = 0;
            Bus.OnGameOver.Invoke();
        }
    }
}
