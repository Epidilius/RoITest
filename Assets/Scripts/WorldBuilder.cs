using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    #region VARIABLES 
    [SerializeField] PoolBoss PoolBoss;
    int TileSize = 5;   //TODO: Un-hard code this?
    Vector3 TilePlacementPosition = Vector3.zero;   //TODO: Not a fan of this
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
                var tile = PoolBoss.GetUnusedGroundTile();
                SetPositionAndRotation(tile, TilePlacementPosition, Quaternion.identity);
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
        int amountOfTiles = (int)(Settings.GetWorldSize().x * Settings.GetWorldSize().y);

        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            var consumer  = PoolBoss.GetUnusedConsumerBuilding();
            GameObject tileToUse = null;
            do
            {
                tileToUse = PoolBoss.GetUsedGroundTile(Random.Range(0, amountOfTiles));
            } while (tileToUse.GetComponent<Tile>().GetChildBuilding() != null);
            
            consumer.GetComponent<Consumer>().SetParentTile(tileToUse);

            //TODO: Change these up a bit
            Quaternion rotation = tileToUse.transform.rotation;
            Vector3 position = tileToUse.transform.position;
            position.y = 0.5f;

            SetPositionAndRotation(consumer, position, rotation);
        }
    }
    void SpawnProducers()
    {
        //TODO: Condese this duplicated code
        int amountOfTiles = (int)(Settings.GetWorldSize().x * Settings.GetWorldSize().y);

        for (int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            var producer = PoolBoss.GetUnusedProducerBuilding();
            GameObject tileToUse = null;
            do
            {
                tileToUse = PoolBoss.GetUsedGroundTile(Random.Range(0, amountOfTiles));
            } while (tileToUse.GetComponent<Tile>().GetChildBuilding() != null);

            producer.GetComponent<Producer>().SetParentTile(tileToUse);

            //TODO: Change these up a bit
            Quaternion rotation = tileToUse.transform.rotation;
            Vector3 position = tileToUse.transform.position;
            position.y = 0.5f;

            SetPositionAndRotation(producer, position, rotation);
        }
    }

    void SetPositionAndRotation(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }
}