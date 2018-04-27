using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    //MATERIALS
    public Material OpenPathMat;
    public Material ClosedPathMat;
    public Material FinalPathMat;

    //STATE
    enum PathFinderState
    {
        StateIdle = 0,
        StateSearchingPath,
        StateFoundPath,
        StateError
    };
    PathFinderState CurrentState;
    
    //NODE COLLECTIONS
    List<PathNode> PathNodeOpen;
    List<PathNode> PathNodeClosed;
    List<PathNode> PathNodeFinal;
    List<Tile> AdjacentNodes;   //TODO: Can I get rid of this?

    private void Awake()
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

    public List<PathNode> FindPath(GameObject startingobject, GameObject destinationObject)
    {
        if(startingobject == null || destinationObject == null)
        {
            return null;
        }

        var startTile = startingobject.GetComponent<Tile>();
        var destinationTile = destinationObject.GetComponent<Tile>();

        return FindPath(startTile, destinationTile);
    }
    public List<PathNode> FindPath(Tile startingTile, Tile destinationTile)//TODO: Refactor?
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

            FindAjacentTiles(currentNode.GetTile());

            for(int i = 0; i < AdjacentNodes.Count; i++)
            {
                var tile = AdjacentNodes[i];
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
        return (int)((coordinates.x * Settings.GetWorldSize().y) + coordinates.z) / Settings.GetTileSize();
    }
    
    void FindAjacentTiles(Tile currentTile) //TODO: Return type?
    {
        AdjacentNodes.Clear();

        AdjacentNodes.Add(GetAdjacentTile(currentTile, 0, 1));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, 0, -1));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, 1, 0));
        AdjacentNodes.Add(GetAdjacentTile(currentTile, -1, 0));
    }
    Tile GetAdjacentTile(Tile currentTile, int deltaX, int deltaZ)
    {
        deltaX *= Settings.GetTileSize();
        deltaZ *= Settings.GetTileSize();

        Vector3 adjacentCoordinate = currentTile.transform.position;
        adjacentCoordinate.x += deltaX;
        adjacentCoordinate.z += deltaZ;

        var tileIndex = GetTileIndex(adjacentCoordinate);
        var tileObject = GameObject.Find("WorldBoss").GetComponent<PoolBoss>().GetUsedObject<Tile.GroundTile>(tileIndex);
        if(tileObject == null)
        {
            return null;
        }
        
        var tile = tileObject.GetComponent<Tile>();
        if (tile == null)
        {
            return null;
        }
        
        return tile;
    }

    bool DoesTileExistInClosedList(Tile tile)
    {
        //var matches = PathNodeClosed.Where(x => x.GetTile() == tile);
        //return matches.Count() > 0;
        int tileIndex = GetTileIndex(tile);

        for (int i = 0; i < PathNodeClosed.Count; i++)
        {
            if (GetTileIndex(PathNodeClosed[i].GetTile()) == tileIndex)
            {
                return true;
            }
        }

        return false;
    }
    bool DoesTileExistInOpenList(Tile tile) 
    {
        return GetOpenPathNodeForTile(tile) != null;    //TODO: Redo this to look like the Closed function?
    }    

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

    void SortOpenList()
    {
        PathNodeOpen = PathNodeOpen.OrderBy(x => x.GetFScore()).ToList();
    }
    void BuildFinalNodePath(PathNode node)
    {
        do
        {
            //node.GetTile().SetMaterial(FinalPathMat);
            PathNodeFinal.Insert(0, node);
            node = node.GetParent();
        } while (node != null);
    }
    
    void ClearPathNodes()
    {
        ClearDataFromNodes(PathNodeOpen);
        ClearDataFromNodes(PathNodeClosed);
        ClearDataFromNodes(PathNodeFinal);

        PathNodeOpen.Clear();
        PathNodeClosed.Clear();
        PathNodeFinal.Clear();
    }
    void ClearDataFromNodes(List<PathNode> nodes)
    {
        for(int i = 0; i < nodes.Count; i++)
        {
            ClearParentsFromNode(nodes[i]);
        }
    }
    void ClearParentsFromNode(PathNode node)
    {
        node.SetParent(null);
    }

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
    int GetManhattanDistanceCost(Tile startTile, Tile destinationTile)
    {
        Vector3 start = startTile.transform.position;
        Vector3 end   = destinationTile.transform.position;

        var distance = Mathf.Abs(end.x - start.x) + Mathf.Abs(end.z - start.z);
        return (int)distance;
    }
}
