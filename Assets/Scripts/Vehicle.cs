using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    GameObject Home;
    GameObject Destination;
    NavMeshAgent Agent;
    
    // Use this for initialization
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHome(GameObject home)
    {
        Home = home;
    }
    public void SetDestination(GameObject destination)
    {
        Destination = destination;  //TODO: Do this? Or just set the AI Agent's target?
    }
    public void SetTransform(Transform newTransform)
    {
        gameObject.GetComponent<NavMeshAgent>().Warp(newTransform.position);    //TODO: Do I need to do gameObject.transform?
    }

    public bool StartDriving()
    {
        return Agent.SetDestination(Destination.transform.position);  //TODO: Wrap in a try/catch? No, just find a way to ONLY call this after pathing is done
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            //TODO: Pause, then continue
            //TODO: Differentiate between Consumer and Producer destinations. Maybe have two endpoint types?
            Debug.Log("Vehicle reached a trigger");

            if(other.gameObject == Destination)
            {
                Debug.Log("Vehicle reached destination");
                Agent.isStopped = true;
            }
        }
    }
}
