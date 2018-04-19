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

    void Start()
    {
        SetupPools();
    }

    //POOL CREATION FUNCTIONS
    void SetupPools()
    {
        var totalTilesInWorld = (int)(WorldSize.x * WorldSize.y);
        GroundTilePool   = new ResourcePool(totalTilesInWorld, GroundTile);
        RoadTilePool     = new ResourcePool(totalTilesInWorld, RoadTile);
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
    
    //TODO: Move to new file?
    class ResourcePool
    {
        int PoolSize;
        Vector3 UnusedPosition;
        List<GameObject> UnusedObjectPool;
        List<GameObject> UsedObjectPool;    //TODO: Do I need this? 
        GameObject PoolType;

        public ResourcePool(int size, GameObject objectPooled)
        {
            PoolSize = size;
            PoolType = objectPooled;
            UnusedPosition = new Vector3(-1000, -1000, -1000);  //TODO: Necessary?
            UnusedObjectPool = new List<GameObject>();
            UsedObjectPool = new List<GameObject>();

            InitPool();
        }

        void InitPool()
        {
            Debug.Log("Starting creation of " + PoolSize + " " + PoolType.name + ". Time is: " + System.DateTime.Now);
            for (int i = 0; i < PoolSize; i++)
            {
                var item = Instantiate(PoolType);
                item.transform.position = UnusedPosition;
                SetStatusOfComponents(ref item, false);
                UnusedObjectPool.Add(item);
            }
            Debug.Log("Finished creation of " + PoolType.name + ". Time is: " + System.DateTime.Now);
        }

        public void ResetPool()
        {
            List<GameObject> clone = UsedObjectPool;

            foreach(var item in clone)
            {
                RemoveItemFromUsedPool(item);
            }
        }

        public GameObject GetFirstUnusedObject()
        {
            var item = UnusedObjectPool[0];
            SetStatusOfComponents(ref item, true);

            UnusedObjectPool.RemoveAt(0);
            UsedObjectPool.Add(item);

            return item;
        }
        public void RemoveItemFromUsedPool(GameObject item)
        {
            item.transform.position = UnusedPosition;
            UsedObjectPool.Remove(item);

            SetStatusOfComponents(ref item, false);
            UnusedObjectPool.Add(item);
        }

        void SetStatusOfComponents(ref GameObject item, bool enabled)
        {
            item.GetComponent<Renderer>().enabled = enabled;
            item.GetComponent<Collider>().enabled = enabled;
        }
    }
}