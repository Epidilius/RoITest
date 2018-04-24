using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Vehicle : MonoBehaviour
{
    enum VehicleState
    {
        Inactive = 0,
        InTransit,
        AtDestination,
        AtHome
    }
    GameObject Home;
    GameObject Destination;
    NavMeshAgent Agent;
    Time ArrivalTime;
    VehicleState CurrentState;
    
    // Use this for initialization
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
        CurrentState = VehicleState.Inactive;
    }

    // Update is called once per frame
    void Update()
    {
        if(CurrentState == VehicleState.AtDestination)
        {
            StartCoroutine(PauseBetweenJobs());
            CurrentState = VehicleState.InTransit;
            SetDestination(Home);
            StartDriving();
        }
    }

    IEnumerator PauseBetweenJobs()
    {
        yield return new WaitForSeconds(5);
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
        Agent.isStopped = false;
        CurrentState = VehicleState.InTransit;
        return Agent.SetDestination(Destination.transform.position);  //TODO: Wrap in a try/catch? No, just find a way to ONLY call this after pathing is done
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            //TODO: Pause, then continue
            //TODO: Differentiate between Consumer and Producer destinations. Maybe have two endpoint types?

            if(other.gameObject == Destination && Destination == Home)
            {
                Debug.Log("Vehicle reached home");
                CurrentState = VehicleState.AtHome;
                Agent.isStopped = true;

                var tile = other.gameObject.transform.parent.GetComponent<Tile>();
                var consumer = tile.GetChildBuilding().GetComponent<Consumer>();
                consumer.VehicleArrived(gameObject);
            }
            else if(other.gameObject == Destination)
            {
                Debug.Log("Vehicle reached destination");
                CurrentState = VehicleState.AtDestination;
                Agent.isStopped = true;
            }
        }
    }
}
