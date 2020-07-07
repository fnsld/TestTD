using UnityEngine;

public class BaseEntity : MonoBehaviour, IPooled
{
    public virtual bool IsDestroyed {
        get;
        set;
    } = true;
    
    public GameObject Parent { get; set; }
    
    public virtual void MoveToPool()
    {
        Pool.Return(this.gameObject);
    }

    public virtual void PrepareAfterPool()
    {
        
    }

    public virtual void PrePooling()
    {
        
    }
}