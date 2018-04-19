using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    int MaxProducts = 999;
    int CurrentProductAmount = 0;
    
    protected void Spawn(Vector3 spawnPoint, Quaternion rotation)
    {
        //TODO: Fetch from pool
        //TODO: Move to location
    }

    protected void OnGUI()
    {
        //TODO: Display amount of products
    }

    protected void AddOneProduct()
    {

    }
    protected void RemoveOneProduct()
    {

    }

    public void SetMaxProductAmount(int max)
    {
        MaxProducts = max;
    }
}
