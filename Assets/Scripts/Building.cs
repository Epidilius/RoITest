using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    int MaxProducts = 999;
    int CurrentProductAmount = 0;
    GameObject ParentTile;  //TODO: Tile?
    GameObject Endpoint;

    public virtual void Init()
    {
        //TODO: Stuff like find nearest Producer. Maybe move to just be in the Consumer
    }

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
    public GameObject GetEndpoint()
    {
        return Endpoint.transform.Find("Destination").gameObject;
    }
    public void SetEndpoint(GameObject endpoint)
    {
        Endpoint = endpoint;
    }
}
