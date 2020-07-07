using UnityEngine;
using UnityEngine.Events;

public class UniversalEvent : UnityEvent {}
public class UniversalEvent<T> : UnityEvent<T> {}
public class UniversalEvent<T, TK> : UnityEvent<T, TK> {}
public class UniversalEvent<T, TK, TKk> : UnityEvent<T, TK, TKk> {}

public static class Bus
{
    public static readonly UniversalEvent OnGameStart = new UniversalEvent();
    public static readonly UniversalEvent OnGameOver = new UniversalEvent();
    public static readonly UniversalEvent<BaseEnemy, float> OnEnemyAttacked = new UniversalEvent<BaseEnemy, float>();
    public static readonly UniversalEvent<BaseEnemy> OnEnemyStepTarget = new UniversalEvent<BaseEnemy>();
    public static readonly UniversalEvent<BaseEnemy> OnEnemyDead = new UniversalEvent<BaseEnemy>();

    public static readonly UniversalEvent OnGoldChanged = new UniversalEvent();
    
    public static Transform DestinationPos;
    public static int PlayerHealth;
    public static int PlayerGold;
    public static int EnemiesDead;
    public static bool IsGameOver => PlayerHealth == 0;
}
