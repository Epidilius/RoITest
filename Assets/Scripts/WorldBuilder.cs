using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldBuilder : MonoBehaviour
{
    #region VARIABLES 
    [SerializeField] PoolBoss PoolBoss;
    List<PathNode> AllPaths;
    #endregion

    void Start()
    {
        //NavMeshPath = new NavMeshPath();
        AllPaths = new List<PathNode>();
        CreateWorld();
        SetupNavBuilder();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetWorld();
            CreateWorld();
        }
    }

    public void CreateWorld()
    {
        SpawnGroundTiles();

        SpawnProducers();
        SpawnConsumers();

        SpawnEndpointTiles();
        SpawnRoadTiles();

        PrepVehicles();
    }
    void ResetWorld()
    {
        PoolBoss.ResetPools();
        ClearNavMesh();
        AllPaths.Clear();
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
    void ClearNavMesh()
    {
        for (int i = 0; i < AllPaths.Count; i++)
        {
            AllPaths[i].GetComponent<NavMeshSourceTag>().enabled = false;
        }
    }

    //TILE FUNCTIONS
    void SpawnGroundTiles()
    {
        Vector3 tilePlacementPosition = Vector3.zero;

        for (int i = 0; i < Settings.GetWorldSize().x; i++)
        {
            for(int j = 0; j < Settings.GetWorldSize().y; j++)
            {
                tilePlacementPosition.x = Settings.GetTileSize() * i;
                tilePlacementPosition.z = Settings.GetTileSize() * j;

                var tile = PoolBoss.GetUnusedObject<Tile.GroundTile>();

                tile.GetComponent<Tile>().Init();
                tile.name = "Ground Tile #" + ((i * Settings.GetWorldSize().x) + j);
                SetPositionAndRotation(tile, tilePlacementPosition, Quaternion.identity);
            }
        }
    }
    void SpawnEndpointTiles()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            CreateEndpointAtBuilding(PoolBoss.GetUsedObject<Consumer>(i));
        }
        for(int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            CreateEndpointAtBuilding(PoolBoss.GetUsedObject<Producer>(i));
        }
    }
    void CreateEndpointAtBuilding(GameObject building)
    {
        var endpoint = PoolBoss.GetUnusedObject<Tile.EndpointTile>();

        building.GetComponent<Building>().SetEndpoint(endpoint);
        endpoint.GetComponent<Tile>().SetChildBuilding(building.GetComponent<Building>());

        var position = building.transform.position;
        position.x += 1.5f;    //TODO: Randomize the X and Z, to be either -1.5 or 1.5
        position.y = 0.1f;
        var rotation = building.transform.rotation;
        rotation.y = 1;   //TODO: Based on the XY the rotation should be

        endpoint.AddComponent(typeof(NavMeshSourceTag));

        SetPositionAndRotation(endpoint, position, rotation);
    }
    void SpawnRoadTiles()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i ++)
        {
            var consumer = PoolBoss.GetUsedObject<Consumer>(i);
            float shortestDistance = Mathf.Infinity;

            for (int j = 0; j < Settings.GetNumberOfProducers(); j++)
            {
                var producer = PoolBoss.GetUsedObject<Producer>(j);
                
                var path = GetPathForRoad(consumer.GetComponent<Consumer>().GetParentTile(), producer.GetComponent<Producer>().GetParentTile());

                if (path.Count < shortestDistance)
                {
                    consumer.GetComponent<Consumer>().SetNearestProducer(producer);
                    shortestDistance = path.Count;
                }

                AddRoadToAllPaths(path);
            }
        }
    }
    List<PathNode> GetPathForRoad(Tile start, Tile destination)
    {
        List<PathNode> path = null;
        do
        {
            path = GetComponent<PathFinder>().FindPath(start, destination);
        } while (path == null);

        return path;
    }
    void AddRoadToAllPaths(List<PathNode> path)
    {
        foreach (var road in path)
        {
            AllPaths.Add(road);
            road.GetTile().MakeTileRoad();
        }
    }

    //BUILDING FUNCTIONS
    void SpawnConsumers()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            CreateBuilding<Consumer>("Consumer #" + i);
        }
    }
    void SpawnProducers()
    {
        for (int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            CreateBuilding<Producer>("Producer #" + i);
        }
    }
    void CreateBuilding<T>(string name = "")
    {
        var building = PoolBoss.GetUnusedObject<T>();
        GameObject tileToUse = null;
        do
        {
            tileToUse = PoolBoss.GetUsedObject<Tile.GroundTile>(Random.Range(0, Settings.GetTotalTileAmount() - 1));
        } while (tileToUse.GetComponent<Tile>().GetChildBuilding() != null);

        Quaternion rotation = Random.rotation;
        rotation.x = 0;
        rotation.z = 0;
        Vector3 position = tileToUse.transform.position;
        position.y = 0.5f;
        
        building.name = name;
        SetPositionAndRotation(building, position, rotation);

        building.GetComponent<Building>().SetParentTile(tileToUse.GetComponent<Tile>());
        building.GetComponent<Building>().Init();
    }

    //VEHICLE
    void PrepVehicles()
    {
        for(int i = 0; i < Settings.GetNumberOfVehicles(); i++)
        {
            var vehicle = PoolBoss.GetObject<Vehicle>(i);
            vehicle.GetComponent<RoITestObject>().Init();
        }
    }
   
    void SetPositionAndRotation(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }
}