using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    #region VARIABLES 
    [SerializeField] PoolBoss PoolBoss;
    int TileSize = 5;   //TODO: Un-hard code this?
    Vector3 TilePlacementPosition = Vector3.zero;
    #endregion

    // Use this for initialization
    void Start()
    {
        CreateWorld();
    }

    public void CreateWorld()
    {
        PoolBoss.ResetPools();   //Bool for first time play to skip this?

        SpawnGroundTiles();

        SpawnConsumers();
        SpawnProducers();

        SpawnEndpointTiles();
        SpawnRoadTiles();
    }

    //TODO: For all these functions, should I use XYZPool.Length instead of what I do now?
    //TILE FUNCTIONS
    void SpawnGroundTiles()
    {
        for(int i = 0; i < Settings.GetWorldSize().x; i++)
        {
            TilePlacementPosition.x = TileSize * i;
            for(int j = 0; j < Settings.GetWorldSize().y; j++)
            {
                TilePlacementPosition.z = TileSize * j;
                var tile = PoolBoss.GetGroundTile();
                tile.transform.position = TilePlacementPosition;    //TODO: Make a func to place a passed object at a passed position?
            }
        }
    }
    void SpawnEndpointTiles()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers() + Settings.GetNumberOfProducers(); i++)
        {
            //TODO: Get the location of buildings, pick a side at random, place an endpoint so the trigger is as close to the building as possible in terms of rotations
        }
    }
    void SpawnRoadTiles()
    {
        //TODO: Path between Consumer and Producer endpoint tiles, then place roads down along that path. Maybe do A* or something for this part, and use Unity navmesh for the vehicles
    }

    //BUILDING FUNCTIONS
    void SpawnConsumers()
    {
        //TODO: Pick random tile, make sure there is nothing on the picked tile, place building in random rotation and position on the picked tile
    }
    void SpawnProducers()
    {
        //TODO: Same as above
    }
}