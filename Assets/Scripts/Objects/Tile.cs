using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] Material BaseMaterial;
    Building ChildBuilding;

    public void Init()
    {
        SetMaterial(BaseMaterial);
        SetChildBuilding(null);
    }

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
