using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    Vector2 WorldSize = new Vector2(50, 50);

    //TILES
    [SerializeField] GameObject GroundTile;
    [SerializeField] GameObject RoadTile;
    [SerializeField] GameObject EndpointTile;

    ResourcePool GroundTilePool;
    ResourcePool RoadTilePool;
    ResourcePool EndpointTilePool;

    // Use this for initialization
    void Start()
    {
        var totalTilesInWorld = (int)(WorldSize.x * WorldSize.y);

        GroundTilePool = new ResourcePool(totalTilesInWorld, GroundTile);
        RoadTilePool = new ResourcePool(totalTilesInWorld, RoadTile);
        EndpointTilePool = new ResourcePool(totalTilesInWorld, EndpointTile);
    }

    // Update is called once per frame
    void Update()
    {

    }
}