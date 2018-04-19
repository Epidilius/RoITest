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
        SpawnTiles();
        SpawnBuildings();
    }

    //TILE FUNCTIONS
    void SpawnTiles()
    {
        SpawnGroundTiles();
        SpawnRoadTiles();
        SpawnEndpointTiles();
    }
    void SpawnGroundTiles()
    {

    }
    void SpawnRoadTiles()
    {

    }
    void SpawnEndpointTiles()
    {

    }

    //BUILDING FUNCTIONS
    void SpawnBuildings()
    {
        SpawnConsumers();
        SpawnProducers();
    }
    void SpawnConsumers()
    {

    }
    void SpawnProducers()
    {

    }
}