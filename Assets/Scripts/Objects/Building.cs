using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : RoITestObject
{
    int MaxProducts = 999;
    int CurrentProductAmount = 0;
    Tile ParentTile;
    GameObject Endpoint;

    public override void Init()
    {
        
    }
    
    protected void ResetProduct()
    {
        CurrentProductAmount = 0;
        UpdateProductGUI();
    }
    protected void AddOneProduct()
    {
        CurrentProductAmount++;
        UpdateProductGUI();
    }
    protected void RemoveOneProduct()
    {
        CurrentProductAmount--;
        UpdateProductGUI();
    }
    void UpdateProductGUI()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = CurrentProductAmount.ToString() + "\r\n";
    }

    public void SetMaxProductAmount(int max)
    {
        MaxProducts = max;
    }

    public Tile GetParentTile()
    {
        return ParentTile;
    }
    public void SetParentTile(Tile tile)
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
    public virtual void VehicleArrived(GameObject vehicle)
    {

    }
}
