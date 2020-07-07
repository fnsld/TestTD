using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooled {

    bool IsDestroyed {
        get;
        set;
    }

    GameObject Parent { get; set; }

    void MoveToPool();
    void PrepareAfterPool();
    void PrePooling();
}

/// <summary>
/// Used to keep objects alive instead of destroying and recreating them, so they don't create a much garbage.
/// </summary>
public class Pool : MonoBehaviour {

    // All pooled objects sorted by a type.
    private static Dictionary<GameObject, Queue<GameObject>> pooledObjects =
        new Dictionary<GameObject, Queue<GameObject>>();

    // All entities in the scene.
    private static List<GameObject> activeObjects = new List<GameObject>();

    private static int maxPoolAmount = 200;

    private static Pool Instance;

    /// <summary>
    /// Used to move an object to the pool.
    /// </summary>
    public static void Return(GameObject otherObject, GameObject parent = null) {
        if (otherObject == null) return;
        var iPooled = otherObject.GetComponent<IPooled>();
        if (iPooled != null) {
            if (parent == null) {
                parent = iPooled.Parent;
            } else {
                iPooled.Parent = parent;
            }
        }
        if (parent == null)
            parent = otherObject;
        otherObject.SetActive(false);
        if (!pooledObjects.ContainsKey(parent))
            pooledObjects[parent] = new Queue<GameObject>();
        else if (pooledObjects[parent].Count >= maxPoolAmount) {
            Destroy(otherObject);
            return;
        }
        iPooled?.PrePooling();
        activeObjects.Remove(otherObject);
        pooledObjects[parent].Enqueue(otherObject);
    }

    /// <summary>
    /// Used to instantiate an object and move it to the pool.
    /// </summary>
    public static void Warmup(GameObject otherObject, int count) {
        if (otherObject == null) return;
        if (Instance == null)
        {
            Instance = new GameObject().AddComponent<Pool>();
            Instance.name = "Pool";
        }
        Instance.StartCoroutine(WarmupCoroutine(otherObject, count));
    }

    private static IEnumerator WarmupCoroutine(GameObject otherObject, int count) {
        for (int i = 0; i < count; i++) {
            GameObject gameObject = Instantiate(otherObject, default,default, Instance.transform);
            Return(gameObject, otherObject);
            yield return null;
        }
    }

    /// <summary>
    /// Used to get an object from the pool or if the pool doesn't contain that kind of the object then instantiates it.
    /// </summary>
    public static T Get<T>(Vector3 position, GameObject gameObject) where T : class {
        Queue<GameObject> queue;
        if (pooledObjects.TryGetValue(gameObject, out queue)) {
            if (queue.Count > 0) {
                var queuedObject = queue.Dequeue();
                if (queuedObject != null && !queuedObject.activeSelf) {
                    queuedObject.transform.position = position;
                    queuedObject.SetActive(true);
                    queuedObject.GetComponent<IPooled>()?.PrepareAfterPool();
                    activeObjects.Add(queuedObject);
                    return queuedObject.GetComponent<T>();
                }
            }
        }
        GameObject newObject = Instantiate(gameObject, position, default, Instance.transform);
        newObject.SetActive(true);
        var iPooled = newObject.GetComponent<IPooled>();
        if (iPooled != null) {
            iPooled.Parent = gameObject;
            iPooled.PrepareAfterPool();
        }
        activeObjects.Add(newObject);
        return newObject.GetComponent<T>();
    }

    /// <summary>
    /// Used to destroy all pooled objects and clear the pool.
    /// </summary>
    public static void ClearPool() {
        foreach (var kvp in pooledObjects)
            foreach (var obj in kvp.Value)
                Destroy(obj);
        pooledObjects.Clear();
    }

    /// <summary>
    /// Used to move to the poll all objects from the scene
    /// </summary>
    public static void TakeAll()
    {
        for (int i = activeObjects.Count - 1; i >= 0; i--) {
            activeObjects[i].GetComponent<BaseEntity>()?.MoveToPool();
        }
        
    }

}