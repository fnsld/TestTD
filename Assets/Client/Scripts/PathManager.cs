using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private Transform destination;
    
    private void Awake()
    {
        Bus.DestinationPos = destination;
    }
}
