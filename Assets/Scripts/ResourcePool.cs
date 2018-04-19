using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePool : MonoBehaviour{

    int PoolSize;
    Vector3 UnusedPosition;
    List<GameObject> UnusedObjectPool;
    List<GameObject> UsedObjectPool;    //TODO: Do I need this? 
    GameObject PoolType;

    public ResourcePool(int size, GameObject objectPooled)
    {
        PoolSize = size;
        PoolType = objectPooled;
        UnusedPosition = new Vector3(-1000, -1000, -1000);  //TODO: Necessary?
        UnusedObjectPool = new List<GameObject>();
        UsedObjectPool = new List<GameObject>();

        InitPool();
    }

    void InitPool()
    {
        Debug.Log("Starting creation of " + PoolSize + " " + PoolType.name + ". Time is: " + System.DateTime.Now);
        for (int i = 0; i < PoolSize; i++)
        {
            var item = Instantiate(PoolType);
            item.transform.position = UnusedPosition;
            SetStatusOfComponents(ref item, false);
            UnusedObjectPool.Add(item);
        }
        Debug.Log("Finished creation of " + PoolType.name + ". Time is: " + System.DateTime.Now);
    }

    public void ResetPool()
    {
        List<GameObject> clone = UsedObjectPool;

        foreach (var item in clone)
        {
            RemoveItemFromUsedPool(item);
        }
    }

    public GameObject GetFirstUnusedObject()
    {
        var item = UnusedObjectPool[0];
        SetStatusOfComponents(ref item, true);

        UnusedObjectPool.RemoveAt(0);
        UsedObjectPool.Add(item);

        return item;
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
