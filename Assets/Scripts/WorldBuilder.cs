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
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            var consumer = PoolBoss.GetUsedConsumerBuilding(i);
            var endpoint = PoolBoss.GetUnusedEndpointTile();

            var position = consumer.transform.position;
            position.x += 1.5f;    //TODO: Randomize the X and Z, to be either -1.5 or 1.5
            position.y = 0.1f;
            var rotation = Quaternion.identity; //TODO: Rotate based on position

            SetPositionAndRotation(endpoint, position, rotation);
        }
        //TODO: Unduplicate this code
        for(int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            var producer = PoolBoss.GetUsedProducerBuilding(i);
            var endpoint = PoolBoss.GetUnusedEndpointTile();

            var position = producer.transform.position;
            position.x += 1.5f;    //TODO: Randomize the X and Z, to be either -1.5 or 1.5
            position.y = 0.1f;
            var rotation = Quaternion.identity; //TODO: Rotate based on position

            SetPositionAndRotation(endpoint, position, rotation);
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