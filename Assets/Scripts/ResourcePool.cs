using System.Collections;
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
            item.transform.position = UnusedPosition;
            SetStatusOfComponents(ref item, false);
            UnusedObjectPool.Add(item);
            AllObjectPool.Add(item);
        }
    }

    public void ResetPool()
    {
        List<GameObject> clone = UsedObjectPool;

        foreach (var item in clone)
        {
            RemoveItemFromUsedPool(item);
        }
    }

    public GameObject GetUnusedObject()
    {
        var item = UnusedObjectPool[0];
        SetStatusOfComponents(ref item, true);

        UnusedObjectPool.RemoveAt(0);
        UsedObjectPool.Add(item);

        return item;
    }
    public GameObject GetUsedObject(int index)
    {
        if (index > UsedObjectPool.Count || index < 0) return null; //TODO: Turn into function?

        return UsedObjectPool[index];
    }
    public GameObject GetObject(int index)
    {
        if (index > AllObjectPool.Count || index < 0) return null;

        return AllObjectPool[index];
    }
    public void RemoveItemFromUsedPool(GameObject item)
    {
        item.transform.position = UnusedPosition;
        UsedObjectPool.Remove(item);

        SetStatusOfComponents(ref item, false);
        UnusedObjectPool.Add(item);
    }

    void SetStatusOfComponents(ref GameObject item, bool enabled)
    {
        item.GetComponent<Renderer>().enabled = enabled;
        item.GetComponent<Collider>().enabled = enabled;
    }
}
