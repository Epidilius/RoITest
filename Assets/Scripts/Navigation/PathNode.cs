using System;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    PathNode Parent = null;
    int HScore;

    //SETTERS
    public void SetScore(int score)
    {
        HScore = score;
    }
    public void SetParent(PathNode parent)
    {
        Parent = parent;
    }

    //OBJECTS
    public Tile GetTile()
    {
        return GetComponent<Tile>();
    }
    public PathNode GetParent()
    {
        return Parent;
    }

    //SCORES
    public int GetGScore()
    {
        if(Parent != null)
        {
            try
            {
                return Parent.GetGScore() + 1;  //TODO :THIS CAUSED A STACK OVERFLOW
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
                return 1;
            }
        }
        return 1;
    }
    public int GetHScore()
    {
        return HScore;
    }
    public int GetFScore()
    {
        return GetGScore() + GetHScore();
    }
}
