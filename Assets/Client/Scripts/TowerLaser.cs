using UnityEngine;

public class TowerLaser : BaseTower
{
    [SerializeField] private Transform lazerSpawnPos;

    private LineRenderer line;
    
    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
    }
    
    protected override void Fire(BaseEnemy enemy)
    {
        if (currentEnemy != null)
        {
            line.enabled = true;
            line.SetPosition(0, lazerSpawnPos.position);
            var correctPos = currentEnemy.transform.position;
            correctPos.y += 0.3f;
            line.SetPosition(1, correctPos);
            if (Time.time - lastFireTime > fireCooldown)
            {
                currentEnemy.OnAttacked(damage);
                lastFireTime = Time.time;
            }
        }
        else
        {
            line.enabled = false;
        }
    }
}