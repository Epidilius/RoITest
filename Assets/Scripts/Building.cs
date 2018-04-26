﻿using System.Collections;
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
        gameObject.GetComponentInChildren<TextMesh>().text = CurrentProductAmount.ToString();
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

    protected IEnumerator PauseToUnloadVehicle()
    {
        yield return new WaitForSeconds(5);
    }
}
