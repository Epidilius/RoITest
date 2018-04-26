using UnityEngine;
using UnityEngine.AI;

public class WorldBuilder : MonoBehaviour
{
    //TODO: Fix freezes
    #region VARIABLES 
    [SerializeField] PoolBoss PoolBoss;
    #endregion

    void Start()
    {
        CreateWorld();
    }
    
    public void CreateWorld()
    {
        PoolBoss.ResetPools();   //Bool for first time play to skip this?
        SetupNavBuilder();

        SpawnGroundTiles();

        SpawnProducers();
        SpawnConsumers();

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

                tile.name += " #" + ((i * Settings.GetWorldSize().x) + j);
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
        var rotation = building.transform.rotation; //TODO: Better rotation
        rotation.y = 1;   //TODO: Based on the XY the rotation should be

        endpoint.AddComponent(typeof(NavMeshSourceTag));

        SetPositionAndRotation(endpoint, position, rotation);
    }
    void SpawnRoadTiles()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i ++)
        {
            var consumer = PoolBoss.GetUsedObject<Consumer>(i);

            for(int j = 0; j < Settings.GetNumberOfProducers(); j++)
            {
                var producer = PoolBoss.GetUsedObject<Producer>(j);
                var path = GetComponent<PathFinder>().FindPath(consumer.GetComponent<Consumer>().GetParentTile(), producer.GetComponent<Producer>().GetParentTile());
                //TODO Use the path var to spawn roads
            }
        }
    }

    //BUILDING FUNCTIONS
    void SpawnConsumers()
    {
        for(int i = 0; i < Settings.GetNumberOfConsumers(); i++)
        {
            CreateBuilding<Consumer>();
        }
    }
    void SpawnProducers()
    {
        for (int i = 0; i < Settings.GetNumberOfProducers(); i++)
        {
            CreateBuilding<Producer>();
        }
    }
    void CreateBuilding<T>()
    {
        var building = PoolBoss.GetUnusedObject<T>();
        GameObject tileToUse = null;
        do
        {
            tileToUse = PoolBoss.GetUsedObject<Tile.GroundTile>(Random.Range(0, Settings.GetTotalTileAmount() - 1));
        } while (tileToUse.GetComponent<Tile>().GetChildBuilding() != null);

        building.GetComponent<Building>().SetParentTile(tileToUse);
        building.GetComponent<Building>().Init();
        building.AddComponent(typeof(NavMeshObstacle));

        //TODO: Change these up a bit
        Quaternion rotation = tileToUse.transform.rotation;
        Vector3 position = tileToUse.transform.position;
        position.y = 0.5f;

        SetPositionAndRotation(building, position, rotation);
    }

    void SetPositionAndRotation(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
    }
}