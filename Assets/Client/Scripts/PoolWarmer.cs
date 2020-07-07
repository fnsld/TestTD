using UnityEngine;

public class PoolWarmer : MonoBehaviour
{
    [SerializeField] private GameObject[] Prefabs;
    [SerializeField] private int Count = 10;
    
    public void Awake()
    {
        foreach (var pref in Prefabs)
        {
            Pool.Warmup(pref, Count);
        }

    }

}
