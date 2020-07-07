using UnityEngine;

public class InputManager : MonoBehaviour
{
    private BaseTower currentTower;
    private bool isPressed;
    
    private void Update()
    {
        if (Bus.IsGameOver) 
            return;
        
        if (currentTower != null)
        {
            currentTower.Select();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            var p = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, Camera.main.transform.position.z * -1));
   
            var colls = Physics.OverlapSphere(p, 1f, LayerMask.GetMask("TowerComponents"));
            foreach (var c in  colls)
            {
                var xz = new Vector3(p.x, 0, p.z);
                var xzC = new Vector3(c.transform.position.x, 0, c.transform.position.z);
                var d = Vector3.Distance(xz, xzC);

                if (d > 2f) 
                    return;
            
                var tower = c.GetComponentInParent<BaseTower>();
                if (tower)
                {
                    if (tower == currentTower)
                    {
                        currentTower.Upgrade();
                    }
                    else
                    {
                        if (currentTower != null)
                            currentTower.Deselect();
                    }
                    currentTower = tower;
                    return;
                }
            }
        }
    }
}
