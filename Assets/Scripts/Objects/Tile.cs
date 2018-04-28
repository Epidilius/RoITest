using UnityEngine;

public class Tile : RoITestObject
{
    [SerializeField] Material BaseMaterial;
    [SerializeField] Material RoadMaterial;
    Building ChildBuilding;

    public override void Init()
    {
        GetComponent<NavMeshSourceTag>().enabled = false;
        SetMaterial(BaseMaterial);
        SetChildBuilding(null);
    }

    public void MakeTileRoad()
    {
        GetComponent<NavMeshSourceTag>().enabled = true;
        SetMaterial(RoadMaterial);
    }

    void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }

    public Building GetChildBuilding()
    {
        return ChildBuilding;
    }
    public void SetChildBuilding(Building child)
    {
        ChildBuilding = child;
    }

    public PathNode GetPathNode()
    {
        return GetComponent<PathNode>();
    }
    
    //These are for typing
    public partial class GroundTile
    {

    }
    public partial class EndpointTile
    {

    }
}
