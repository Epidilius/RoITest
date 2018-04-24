using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Singleton, or maybe static?
public class AStar {

    enum PathFinderState
    {
        StateIdle = 0,
        StateSearchingPath,
        StateFoundPath,
        StateError
    };
    PathFinderState CurrentState = PathFinderState.StateIdle;

    //TODO: List<Tile>?
    List<GameObject> PathNode_Opem;
    List<GameObject> PathNode_Closed;
    List<GameObject> PathNode_Final;

    public AStar()
    {
        PathNode_Opem = new List<GameObject>();
        PathNode_Closed = new List<GameObject>();
        PathNode_Final = new List<GameObject>();
    }

    public void FindPath(GameObject beginningTile, GameObject endTile)
    {
        //TODO: Return type. Maybe an array of inidices?
    }

    public void Reset()
    {
        CurrentState = PathFinderState.StateIdle;
    }

    public bool IsSearching()
    {
        return CurrentState == PathFinderState.StateSearchingPath;
    }
    public int GetPathSize()
    {
        return PathNode_Final.Count;
    }
    public GameObject GetPathNodeAtIndex(int index)
    {
        if(index < GetPathSize())
        {
            return PathNode_Final[index];
        }

        return null;
    }
}
