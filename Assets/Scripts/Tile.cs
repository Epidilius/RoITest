using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //TODO: Road : Tile, Endpoint : Tile, etc?
    //TOOD: Both for this and Building, I need to clear these on a pool reset
    //TODO: Should tiles know their index?
    GameObject ChildBuilding = null;
    GameObject ParentNode = null;
    
    public GameObject GetChildBuilding()
    {
        return ChildBuilding;
    }
    public void SetChildBuilding(GameObject child)
    {
        ChildBuilding = child;
    }

    public bool IsNavigable()
    {
        return ChildBuilding == null;
    }
    public bool IsRoad()
    {
        return true;    //TODO: Decide on if I have a member variable or inherit into a road class, probably the latter. 
    }

    public GameObject GetParent()
    {
        return ParentNode;
    }
    public void SetParentNode(GameObject parentNode)
    {
        ParentNode = parentNode;
    }

    //DEBUG
    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

    public PathNode GetPathNode()
    {
        return GetComponent<PathNode>();
    }
    public int GetGScore()
    {
        if(ParentNode != null)
        {
            return ParentNode.GetComponent<Tile>().GetGScore() + 1;
        }

        return 1;
    }

    public bool CompareTiles(GameObject tileA, GameObject tileB)
    {
        return tileA.GetComponent<Tile>().GetGScore() < tileB.GetComponent<Tile>().GetGScore();
    }
}
