using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //TODO: Road : Tile, Endpoint : Tile, etc?
    //TOOD: Both for this and Building, I need to clear these on a pool reset
    GameObject ChildBuilding = null;
    
    public GameObject GetChildBuilding()
    {
        return ChildBuilding;
    }
    public void SetChildBuilding(GameObject child)
    {
        ChildBuilding = child;
    }
}
