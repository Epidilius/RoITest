using UnityEngine;

public class PoolBoss : MonoBehaviour
{

    //TODO: Make this a Job

    #region VARIABLES 
    //TILES 
    [SerializeField] GameObject GroundTile;
    [SerializeField] GameObject EndpointTile;

    public ResourcePool GroundTilePool;
    public ResourcePool EndpointTilePool;

    //BUILDINGS 
    [SerializeField] GameObject ConsumerBuilding;
    [SerializeField] GameObject ProducerBuilding;

    public ResourcePool ConsumerPool;
    public ResourcePool ProducerPool;

    //VEHICLES 
    [SerializeField] GameObject Vehicle;
    public ResourcePool VehiclePool;
    #endregion

    void Awake()
    {
        SetupPools();
    }
    
    void SetupPools()
    {
        var totalTilesInWorld = (int)(Settings.GetWorldSize().x * Settings.GetWorldSize().y);
        GroundTilePool   = new ResourcePool(totalTilesInWorld, GroundTile);
        EndpointTilePool = new ResourcePool(Settings.GetNumberOfConsumers() + Settings.GetNumberOfProducers(), EndpointTile);

        ConsumerPool = new ResourcePool(Settings.GetNumberOfConsumers(), ConsumerBuilding);
        ProducerPool = new ResourcePool(Settings.GetNumberOfProducers(), ProducerBuilding);

        VehiclePool = new ResourcePool(Settings.GetNumberOfVehicles(), Vehicle);
    }
    public void ResetPools()
    {
        ResetPool<Vehicle>();

        ResetPool<Tile.GroundTile>();
        ResetPool<Tile.EndpointTile>();

        ResetPool<Consumer>();
        ResetPool<Producer>();
    }
    public void ResetPool<T>()
    {
        if (typeof(T) == typeof(Vehicle))                   VehiclePool.ResetPool();

        else if (typeof(T) == typeof(Tile.GroundTile))      GroundTilePool.ResetPool();
        else if (typeof(T) == typeof(Tile.EndpointTile))    EndpointTilePool.ResetPool();

        else if (typeof(T) == typeof(Consumer))             ConsumerPool.ResetPool();
        else if (typeof(T) == typeof(Producer))             ProducerPool.ResetPool();
    }
    
    public GameObject GetUnusedObject<T>()
    {
        if (typeof(T) == typeof(Vehicle))                   return VehiclePool.GetUnusedObject();

        else if (typeof(T) == typeof(Tile.GroundTile))      return GroundTilePool.GetUnusedObject();
        else if (typeof(T) == typeof(Tile.EndpointTile))    return EndpointTilePool.GetUnusedObject();

        else if (typeof(T) == typeof(Consumer))             return ConsumerPool.GetUnusedObject();
        else if (typeof(T) == typeof(Producer))             return ProducerPool.GetUnusedObject();

        return null;
    }
    public GameObject GetUsedObject<T>(int index)
    {
        if (typeof(T) == typeof(Vehicle))                   return VehiclePool.GetUsedObject(index);

        else if (typeof(T) == typeof(Tile.GroundTile))      return GroundTilePool.GetUsedObject(index);
        else if (typeof(T) == typeof(Tile.EndpointTile))    return EndpointTilePool.GetUsedObject(index);

        else if (typeof(T) == typeof(Consumer))             return ConsumerPool.GetUsedObject(index);
        else if (typeof(T) == typeof(Producer))             return ProducerPool.GetUsedObject(index);

        return null;
    }
    public GameObject GetObject<T>(int index)
    {
        if (typeof(T) == typeof(Vehicle))                   return VehiclePool.GetObject(index);

        else if (typeof(T) == typeof(Tile.GroundTile))      return GroundTilePool.GetObject(index);
        else if (typeof(T) == typeof(Tile.EndpointTile))    return EndpointTilePool.GetObject(index);

        else if (typeof(T) == typeof(Consumer))             return ConsumerPool.GetObject(index);
        else if (typeof(T) == typeof(Producer))             return ProducerPool.GetObject(index);

        return null;
    }
    
    public void ReturnItemToPool(GameObject item)
    {
        if (item.GetType() == Vehicle.GetType())                VehiclePool.RemoveItemFromUsedPool(item);

        else if (item.GetType() == GroundTile.GetType())        GroundTilePool.RemoveItemFromUsedPool(item);   
        else if (item.GetType() == EndpointTile.GetType())      EndpointTilePool.RemoveItemFromUsedPool(item);
        
        else if (item.GetType() == ConsumerBuilding.GetType())  ConsumerPool.RemoveItemFromUsedPool(item);        
        else if (item.GetType() == ProducerBuilding.GetType())  ProducerPool.RemoveItemFromUsedPool(item);        
    }
}