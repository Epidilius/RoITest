using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    //TODO: Move the Vehicle stuff to Consumer?
    #region VARIABLES 
    Vector2 WorldSize = new Vector2(50, 50);

    //TILES 
    public GameObject GroundTile;
    public GameObject RoadTile;
    public GameObject EndpointTile;

    ResourcePool GroundTilePool;
    ResourcePool RoadTilePool;
    ResourcePool EndpointTilePool;

    //BUILDINGS 
    public GameObject ConsumerBuilding;
    public GameObject ProducerBuilding;

    ResourcePool ConsumerPool;
    ResourcePool ProducerPool;

    int NumberOfProducers = 2;
    int NumberOfConsumers = 5;

    //VEHICLES 
    public GameObject Vehicle;
    ResourcePool VehiclePool;
    int MaxVehiclesPerConsumer = 1;
    #endregion

    // Use this for initialization
    void Start()
    {
        SetupPools();
        CreateWorld();
    }

    //POOL CREATION FUNCTIONS 
    void SetupPools()
    {
        var totalTilesInWorld = (int)(WorldSize.x * WorldSize.y);
        GroundTilePool = new ResourcePool(totalTilesInWorld, GroundTile);
        RoadTilePool = new ResourcePool(totalTilesInWorld, RoadTile);
        EndpointTilePool = new ResourcePool(totalTilesInWorld, EndpointTile);

        ConsumerPool = new ResourcePool(NumberOfConsumers, ConsumerBuilding);
        ProducerPool = new ResourcePool(NumberOfProducers, ProducerBuilding);

        VehiclePool = new ResourcePool(MaxVehiclesPerConsumer * NumberOfConsumers, Vehicle);
    }
    void ResetPools()
    {
        GroundTilePool.ResetPool();
        RoadTilePool.ResetPool();
        EndpointTilePool.ResetPool();

        ConsumerPool.ResetPool();
        ProducerPool.ResetPool();

        VehiclePool.ResetPool();
    }

    public void CreateWorld()
    {
        ResetPools();   //Bool for first time play to skip this?

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
        //TODO: Pick one of these
        for(int i = 0; i < WorldSize.x * WorldSize.y; i++)
        {
            
        }
        for(int i = 0; i < WorldSize.x; i++)
        {
            for(int j = 0; j < WorldSize.y; j++)
            {

            }
        }
    }
    void SpawnEndpointTiles()
    {
        for(int i = 0; i < NumberOfConsumers + NumberOfProducers; i++)
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