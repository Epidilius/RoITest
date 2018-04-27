using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour{

    int PoolSize;
    Vector3 UnusedPosition;
    List<GameObject> UnusedObjectPool;
    List<GameObject> UsedObjectPool;
    List<GameObject> AllObjectPool;
    GameObject PoolType;

    public ResourcePool(int size, GameObject objectPooled)
    {
        PoolSize = size;
        PoolType = objectPooled;
        UnusedPosition = new Vector3(-1000, -1000, -1000);  //TODO: Necessary?
        UnusedObjectPool = new List<GameObject>();
        UsedObjectPool = new List<GameObject>();
        AllObjectPool = new List<GameObject>();

        InitPool();
    }

    void InitPool()
    {
        for (int i = 0; i < PoolSize; i++)
        {
            var item = Instantiate(PoolType);
            SetStatusOfComponents(item, false);
            UnusedObjectPool.Add(item);
            AllObjectPool.Add(item);
        }
    }

    public void ResetPool()
    {
        //TODO: Something better than this
        if(PoolType.GetComponent<Vehicle>() != null)
        {
            var clone = UsedObjectPool.ToArray();

            foreach (var item in clone)
            {
                RemoveItemFromUsedPool(item);
            }
        }
        UnusedObjectPool = AllObjectPool.GetRange(0, AllObjectPool.Count);
        UsedObjectPool.Clear();
    }

    public GameObject GetUnusedObject()
    {
        if (UnusedObjectPool.Count < 1) return null;

        var item = UnusedObjectPool[0];
        SetStatusOfComponents(item, true);

        UnusedObjectPool.RemoveAt(0);
        UsedObjectPool.Add(item);

        return item;
    }
    public GameObject GetUsedObject(int index)
    {
        return IsIndexValid(index, UsedObjectPool) ? UsedObjectPool[index] : null;
    }
    public GameObject GetObject(int index)
    {
        return IsIndexValid(index, AllObjectPool) ? AllObjectPool[index] : null;
    }
    public void RemoveItemFromUsedPool(GameObject item)
    {
        UsedObjectPool.Remove(item);
        SetStatusOfComponents(item, false);
        UnusedObjectPool.Add(item);
    }

    bool IsIndexValid(int index, List<GameObject> pool) //TODO: Remove the pool argument?
    {
        return (index < pool.Count && index >= 0);
    }
    void SetStatusOfComponents(GameObject item, bool enabled)   //TODO: Do I need the ref?
    {
        item.gameObject.SetActive(enabled);
        if (enabled == false) item.transform.position = UnusedPosition;
    }
}
