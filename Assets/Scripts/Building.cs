using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    int MaxProducts = 999;
    int CurrentProductAmount = 0;
    GameObject ParentTile;

    protected void OnGUI()
    {
        //TODO: Display amount of products
    }

    protected void AddOneProduct()
    {
        CurrentProductAmount++;
    }
    protected void RemoveOneProduct()
    {
        CurrentProductAmount--;
    }

    public void SetMaxProductAmount(int max)
    {
        MaxProducts = max;
    }

    public GameObject GetParentTile()
    {
        return ParentTile;
    }
    public void SetParentTile(GameObject tile)
    {
        ParentTile = tile;
    }
}
