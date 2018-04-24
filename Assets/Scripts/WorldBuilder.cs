using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldBuilder : MonoBehaviour
{
    //TODO: Fix freezes
    #region VARIABLES 
    [SerializeField] PoolBoss PoolBoss;
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
        SetupNavBuilder();

        SpawnGroundTiles();

        SpawnConsumers();
        SpawnProducers();
        InitBuildings();

        SpawnEndpointTiles();
        SpawnRoadTiles();
    }

    //NAV MESH FUNCTIONS
    void SetupNavBuilder()
    {
        var bounds = GameObject.Find("NavMeshBuildingBounds");
        var builder = bounds.GetComponent<LocalNavMeshBuilder>();

        var center = Vector3.zero;
        center.x += Settings.GetWorldSize().x * Settings.GetTileSize();
        center.z += Settings.GetWorldSize().y * Settings.GetTileSize();
        center /= 2;

        bounds.transform.position = center;

        builder.m_Size = center * 2;
        builder.m_Size.y = 20;
    }
    //TODO: For all these functions, should I use XYZPool.Length instead of what I do now?
    //TILE FUNCTIONS
    void SpawnGroundTiles()
    {
        for(int i = 0; i < Settings.GetWorldSize().x; i++)
        {
            TilePlacementPosition.x = Settings.GetTileSize() * i;
            for(int j = 0; j < Settings.GetWorldSize().y; j++)
            {
                TilePlacementPosition.z = Settings.GetTileSize() * j;
                var tile = PoolBoss.GetUnusedGroundTile();
                tile.name += " #" + ((i * Settings.GetWorldSize().x) + j);
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

            consumer.GetComponent<Consumer>().SetEndpoint(endpoint);

            var position = consumer.transform.position;
            position.x += 1.5f;    //TODO: Randomize the X and Z, to be either -1.5 or 1.5. Use a ternary
            position.y = 0.001f;
            var rotation = consumer.transform.rotation; //TODO: Better rotation
            rotation.y = 1;   //TODO: Based on the XY the rotation should be

            endpoint.AddComponent(typeof(NavMeshSourceTag));

            SetPositionAndRotation(endpoint, position, rotation);
        }
        //TODO: Unduplicate this code
        for(int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            var producer = PoolBoss.GetUsedProducerBuilding(i);
            var endpoint = PoolBoss.GetUnusedEndpointTile();

            producer.GetComponent<Producer>().SetEndpoint(endpoint);

            var position = producer.transform.position;
            position.x += 1.5f;    //TODO: Randomize the X and Z, to be either -1.5 or 1.5
            position.y = 0.1f;
            var rotation = producer.transform.rotation; //TODO: Better rotation
            rotation.y = 1;   //TODO: Based on the XY the rotation should be

            endpoint.AddComponent(typeof(NavMeshSourceTag));

            SetPositionAndRotation(endpoint, position, rotation);
        }
    }
    void SpawnRoadTiles()
    {
        //TODO: Path between Consumer and Producer endpoint tiles, then place roads down along that path. Maybe do A* or something for this part, and use Unity navmesh for the vehicles

        for(int i = 0; i < Settings.GetNumberOfConsumers(); i ++)
        {
            //TODO: Ternaries?
            var consumer = PoolBoss.GetUsedConsumerBuilding(i);
            if(consumer == null)
            {
                continue;
            }

            for(int j = 0; j < Settings.GetNumberOfProducers(); j++)
            {
                var producer = PoolBoss.GetUsedProducerBuilding(j);
                if(producer == null)
                {
                    continue;
                }

                var path = GetComponent<PathFinder>().FindPath(consumer.GetComponent<Consumer>().GetParentTile(), producer.GetComponent<Producer>().GetParentTile());
            }
        }
    }

    //BUILDING FUNCTIONS
    void SpawnConsumers()
    {
        int amountOfTiles = (int)(Settings.GetWorldSize().x * Settings.GetWorldSize().y);

        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            var consumer = PoolBoss.GetUnusedConsumerBuilding();
            GameObject tileToUse = null;
            do
            {
                tileToUse = PoolBoss.GetUsedGroundTile(Random.Range(0, amountOfTiles));
            } while (tileToUse.GetComponent<Tile>().GetChildBuilding() != null);
            
            consumer.GetComponent<Consumer>().SetParentTile(tileToUse);
            consumer.AddComponent(typeof(NavMeshObstacle));

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
            producer.AddComponent(typeof(NavMeshObstacle));

            //TODO: Change these up a bit
            Quaternion rotation = tileToUse.transform.rotation;
            Vector3 position = tileToUse.transform.position;
            position.y = 0.5f;

            SetPositionAndRotation(producer, position, rotation);
        }
    }
    void InitBuildings()
    {
        //TODO: Undupe this code.
        //TODO: Maybe, instead of this function, I should call SpawnProducers before SpawnConsumers, and then call the Inits in the spawners
        for (int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            PoolBoss.GetUsedConsumerBuilding(i).GetComponent<Consumer>().Init();
        }
        for (int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            PoolBoss.GetUsedProducerBuilding(i).GetComponent<Producer>().Init();
        }
    }

    void SetPositionAndRotation(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }
}