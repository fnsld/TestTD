using UnityEngine;

public class TowerCannon : BaseTower
{
    [SerializeField] private Transform bulletSpawnPos;

    [SerializeField] private ParticleSystem shootEffect;

    protected override void Fire(BaseEnemy enemy)
    {
        if (Time.time - lastFireTime < fireCooldown) 
            return;
        
        if (currentEnemy != null)
        {
            shootEffect.transform.position = bulletSpawnPos.position;
            shootEffect.Play();
            
            enemy.OnAttacked(damage);
            
            lastFireTime = Time.time;
        }
    }
}