using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBoss : MonoBehaviour
{

    //TODO: Make this a singleton, or maybe static? Or do I just leave it as a MonoBehaviour? 
    //TODO: Make funcs to fetch each type of object (consumers, tiles, vehicles, producers, roads, etc) 
    //TODO: Have SEPERATE functions for each 
    //TODO: Endpoint list will be of a size equal to building amount, the rest will be equal to the settings, road will be equal to the amount of total tiles 
    //TODO: Have the UI stuff set all the variables 

    #region VARIABLES 
    //TILES 
    [SerializeField] GameObject GroundTile;
    [SerializeField] GameObject RoadTile;
    [SerializeField] GameObject EndpointTile;

    ResourcePool GroundTilePool;
    ResourcePool RoadTilePool;
    ResourcePool EndpointTilePool;

    //BUILDINGS 
    [SerializeField] GameObject ConsumerBuilding;
    [SerializeField] GameObject ProducerBuilding;

    ResourcePool ConsumerPool;
    ResourcePool ProducerPool;

    //VEHICLES 
    [SerializeField] GameObject Vehicle;
    ResourcePool VehiclePool;
    #endregion

    void Start()
    {
        SetupPools();
    }

    //POOL CREATION FUNCTIONS 
    void SetupPools()
    {
        var totalTilesInWorld = (int)(Settings.GetWorldSize().x * Settings.GetWorldSize().y);   //TODO: Do I still need the cast?
        GroundTilePool = new ResourcePool(totalTilesInWorld, GroundTile);
        RoadTilePool = new ResourcePool(totalTilesInWorld, RoadTile);
        EndpointTilePool = new ResourcePool(totalTilesInWorld, EndpointTile);

        ConsumerPool = new ResourcePool(Settings.GetNumberOfConsumers(), ConsumerBuilding);
        ProducerPool = new ResourcePool(Settings.GetNumberOfProducers(), ProducerBuilding);

        VehiclePool = new ResourcePool(Settings.GetNumberOfConsumers() + Settings.GetNumberOfProducers(), Vehicle);
    }
    public void ResetPools()
    {
        GroundTilePool.ResetPool();
        RoadTilePool.ResetPool();
        EndpointTilePool.ResetPool();

        ConsumerPool.ResetPool();
        ProducerPool.ResetPool();

        VehiclePool.ResetPool();
    }

    //GET OBJECT FROM POOL
    public GameObject GetGroundTile()
    {
        return GroundTilePool.GetFirstUnusedObject();
    }
    public GameObject GetRoadTile()
    {
        return RoadTilePool.GetFirstUnusedObject();
    }
    public GameObject GetEndpointTile()
    {
        return EndpointTilePool.GetFirstUnusedObject();
    }
    public GameObject GetConsumerBuilding()
    {
        return ConsumerPool.GetFirstUnusedObject();
    }
    public GameObject GetProducerBuilding()
    {
        return ProducerPool.GetFirstUnusedObject();
    }
    public GameObject GetVehicle()
    {
        return VehiclePool.GetFirstUnusedObject();
    }

    //RETURN OBJECT TO POOL
    public void ReturnItemToPool(GameObject item)
    {
        if (item.GetType() == Vehicle.GetType())                VehiclePool.RemoveItemFromUsedPool(item);

        else if (item.GetType() == GroundTile.GetType())        GroundTilePool.RemoveItemFromUsedPool(item);
        else if (item.GetType() == RoadTile.GetType())          RoadTilePool.RemoveItemFromUsedPool(item);        
        else if (item.GetType() == EndpointTile.GetType())      EndpointTilePool.RemoveItemFromUsedPool(item);
        
        else if (item.GetType() == ConsumerBuilding.GetType())  ConsumerPool.RemoveItemFromUsedPool(item);        
        else if (item.GetType() == ProducerBuilding.GetType())  ProducerPool.RemoveItemFromUsedPool(item);        
    }

}