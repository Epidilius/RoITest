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
    public virtual void VehicleArrived(GameObject vehicle)
    {

    }
}
