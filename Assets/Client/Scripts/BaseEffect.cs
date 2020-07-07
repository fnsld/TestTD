using UnityEngine;

public class BaseEffect : BaseEntity
{
    [SerializeField] private float destroyIn;

    public override void PrepareAfterPool()
    {
        base.PrepareAfterPool();
        if (destroyIn > 0)
            Invoke("MoveToPool", destroyIn);
    }
}
