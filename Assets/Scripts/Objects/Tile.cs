using UnityEngine;

public class Tile : MonoBehaviour
{
    Building ChildBuilding = null;  //TODO: Reset this on Pool reset
    
    public Building GetChildBuilding()
    {
        return ChildBuilding;
    }
    public void SetChildBuilding(Building child)
    {
        ChildBuilding = child;
    }

    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

    public PathNode GetPathNode()
    {
        return GetComponent<PathNode>();
    }

    public partial class GroundTile
    {

    }
    public partial class EndpointTile
    {

    }
}
