using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] PoolBoss PoolBoss;

    //DEBUG
    public Material OpenPathMat;
    public Material ClosedPathMat;
    public Material FinalPathMat;
    public Material RoadMaterial;

    enum PathFinderState
    {
        StateIdle = 0,
        StateSearchingPath,
        StateFoundPath,
        StateError
    };
    PathFinderState CurrentState;
    
    List<PathNode> PathNodeOpen;
    List<PathNode> PathNodeClosed;
    List<PathNode> PathNodeFinal;
    List<Tile> AdjacentNodes;

    private void Start()
    {
        CurrentState = PathFinderState.StateIdle;

        PathNodeOpen = new List<PathNode>();
        PathNodeClosed = new List<PathNode>();
        PathNodeFinal = new List<PathNode>();
        AdjacentNodes = new List<Tile>();   //TODO: Do I need this?
    }

    public void Reset()
    {
        CurrentState = PathFinderState.StateIdle;
        ClearPathNodes();
    }

    //TODO: Refactor this function
    public List<PathNode> FindPath(GameObject startingobject, GameObject destinationObject)
    {
        var startTile = startingobject.GetComponent<Tile>();
        var destinationTile = destinationObject.GetComponent<Tile>();

        if(startTile == null || destinationTile == null)
        {
            //TODO: start == dest?
             return null;
        }

        return FindPath(startTile, destinationTile);
    }
    public List<PathNode> FindPath(Tile startingTile, Tile destinationTile)
    {
        Reset();

        var startingIndex    = GetTileIndex(startingTile);
        var destinationIndex = GetTileIndex(destinationTile);

        if(startingIndex == destinationIndex)
        {
            return null;
        }

        CurrentState = PathFinderState.StateSearchingPath;

        var hScore = GetManhattanDistanceCost(startingTile, destinationTile);

        var startingNode = startingTile.GetPathNode();
        startingNode.SetScore(hScore);
        AddPathNodeToOpenList(startingNode);

        while(IsSearchingForPath())
        {
            if(PathNodeOpen.Count == 0)
            {
                //TODO: Debug on errors?
                CurrentState = PathFinderState.StateError;
                return null;
            }

            var currentNode = PathNodeOpen[0];
            MoveNodeToClosedList(currentNode);

            if(GetTileIndex(currentNode.GetTile()) == destinationIndex)
            {
                BuildFinalNodePath(currentNode);
                CurrentState = PathFinderState.StateFoundPath;
                return PathNodeFinal;
            }

            var adjacents = GetAdjacentTiles(currentNode.GetTile());

            for(int i = 0; i < adjacents.Count; i++)
            {
                var tile = adjacents[i];
                if (tile == null)
                {
                    continue;
                }

                if (DoesTileExistInClosedList(tile))
                {
                    continue;
                }

                if (!DoesTileExistInOpenList(tile))
                {
                    hScore = GetManhattanDistanceCost(tile, destinationTile);

                    var node = tile.GetPathNode();
                    node.SetParent(currentNode);
                    node.SetScore(hScore);
                    AddPathNodeToOpenList(node);
                }
                else
                {
                    var node = GetOpenPathNodeForTile(tile);

                    if(currentNode.GetGScore() + 1 < node.GetGScore())
                    {
                        node.SetParent(currentNode);
                        SortOpenList();
                    }
                }
            }
        }

        return null;
    }

    //DATA FETCHERS
    public bool IsSearchingForPath()
    {
        return CurrentState == PathFinderState.StateSearchingPath;
    }
    public int GetPathSize()
    {
        return PathNodeFinal.Count;
    }
    PathNode GetNodeAtIndex(int index)
    {
        if(index < GetPathSize())
        {
            return PathNodeFinal[index];
        }
        return null;
    }

    int GetTileIndex(Tile tile)
    {
        return GetTileIndex(tile.transform.position);
    }
    int GetTileIndex(Vector3 coordinates)
    {
        return (int)((coordinates.x * Settings.GetWorldSize().y) + coordinates.z) / Settings.GetTileSize();   //TODO: Test this
    }

    //TODO: Should this be something Tile handles?
    List<Tile> GetAdjacentTiles(Tile currentTile)
    {
        AdjacentNodes.Clear();

        AdjacentNodes.Add(GetAdjacentTile(currentTile, 0, 1));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, 0, -1));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, 1, 0));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, -1, 0));

        return AdjacentNodes;   //TODO: Actually return this?
    }
    Tile GetAdjacentTile(Tile currentTile, int deltaX, int deltaZ)
    {
        deltaX *= Settings.GetTileSize();
        deltaZ *= Settings.GetTileSize();

        Vector3 adjacentCoordinate = currentTile.transform.position;
        adjacentCoordinate.x += deltaX;
        adjacentCoordinate.z += deltaZ;

        var tileIndex = GetTileIndex(adjacentCoordinate);

        var tileObject = PoolBoss.GetGroundTile(tileIndex);

        if (tileObject == null) return null;

        return tileObject.GetComponent<Tile>();
    }

    //TODO: Faster way than looping through ther lists thousands of times?
    bool DoesTileExistInClosedList(Tile tile)
    {
        var tileIndex = GetTileIndex(tile);
        
        for(int i = 0; i < PathNodeClosed.Count; i++)
        {
            if(GetTileIndex(PathNodeClosed[i].GetTile()) == tileIndex)
            {
                return true;
            }
        }
        return false;
    }
    bool DoesTileExistInOpenList(Tile tile) 
    {
        return GetOpenPathNodeForTile(tile) != null;
    }

    //Returns a PathNode that matches the Tile from the Open List
    PathNode GetOpenPathNodeForTile(Tile tile)
    {
        int tileIndex = GetTileIndex(tile);

        for(int i = 0; i < PathNodeOpen.Count; i++)
        {
            if(GetTileIndex(PathNodeOpen[i].GetTile()) == tileIndex)
            {
                return PathNodeOpen[i];
            }
        }

        return null;
    }

    //Sort the open list, the path node with the lowest F score will be first
    void SortOpenList()
    {
        PathNodeOpen = PathNodeOpen.OrderBy(x => x.GetFScore()).ToList();
    }

    //Add a path node to the open list and sorts it based on the F score
    void AddPathNodeToOpenList(PathNode node)
    {
        //node.GetTile().SetMaterial(OpenPathMat);
        PathNodeOpen.Add(node);
        SortOpenList();
    }
    void MoveNodeToClosedList(PathNode node)
    {
        //node.GetTile().SetMaterial(ClosedPathMat);
        PathNodeClosed.Add(node);
        PathNodeOpen.Remove(node);
    }

    //Build the final path, called once a path is found
    void BuildFinalNodePath(PathNode node)
    {
        do
        {
            if(node.GetParent() != null)    //TODO: Do I need this?
            {
                PathNodeFinal.Insert(0, node);
            }

            //node.GetTile().SetMaterial(FinalPathMat);
            //TODO: Place in new function
            if (node.gameObject.GetComponent<NavMeshSourceTag>() == null)
            {
                node.gameObject.AddComponent(typeof(NavMeshSourceTag));
            }

            node.GetTile().SetMaterial(RoadMaterial);
            node = node.GetParent();
        } while (node != null);
    }

    //Clears the Open, closed and final path Lists, cleans up all memory associated with them
    void ClearPathNodes()
    {
        PathNodeOpen.Clear();
        PathNodeClosed.Clear();
        PathNodeFinal.Clear();
    }
    /*
     for (int i = 0; i < PathNodeFinal.Count; i++)
        {
            var node = PathNodeFinal[i];

            //TODO: Move to function
            if (node.gameObject.GetComponent<NavMeshSourceTag>() != null)
            {
                Destroy(node.gameObject.GetComponent<NavMeshSourceTag>());
            }
        }
     */

    //Calculate the manhattan distance (h score)
    int GetManhattanDistanceCost(Tile startTile, Tile destinationTile)
    {
        Vector3 start = startTile.transform.position;
        Vector3 end   = destinationTile.transform.position;

        var distance = Mathf.Abs((int)(end.x - start.x)) + Mathf.Abs((int)(end.z - start.z));   //TODO: Maybe have one casting call for this instead of two?
        return distance;

    }
}
