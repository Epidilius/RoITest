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
        AtProducer,
        AtConsumer
    }
    GameObject Consumer;
    GameObject Producer;
    NavMeshAgent Agent;
    Time ArrivalTime;
    VehicleState CurrentState;
    
    // Use this for initialization
    void Start()
    {
        Agent = gameObject.GetComponent<NavMeshAgent>();
        CurrentState = VehicleState.Inactive;
    }

    public void SetConsumer(GameObject consumer)
    {
        Consumer = consumer;
    }
    public void SetProducer(GameObject producer)
    {
        Producer = producer;  //TODO: Do this? Or just set the AI Agent's target?
    }
    public void SetTransform(Transform newTransform)
    {
        gameObject.GetComponent<NavMeshAgent>().Warp(newTransform.position);    //TODO: Do I need to do gameObject.transform?
    }

    public bool StartDriving()
    {
        Agent.isStopped = false;
        var destinationSet = false;

        if (CurrentState == VehicleState.AtProducer)
        {
            destinationSet = Agent.SetDestination(Consumer.transform.position);
        }
        else
        {
            destinationSet =  Agent.SetDestination(Producer.transform.position);
        }

        CurrentState = VehicleState.InTransit;

        return destinationSet;
        //TODO: Wrap in a try/catch? No, just find a way to ONLY call this after pathing is done
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            //TODO: Pause, then continue
            //TODO: Undupe this code
            if (other.gameObject == Consumer && CurrentState != VehicleState.AtConsumer && Vector3.Distance(Agent.destination, Consumer.transform.position) < 0.2)
            {
                Debug.Log("Vehicle reached home");
                CurrentState = VehicleState.AtConsumer;
                Agent.isStopped = true;

                var tile = other.gameObject.transform.parent.GetComponent<Tile>();
                var consumer = tile.GetChildBuilding().GetComponent<Building>();
                consumer.VehicleArrived(gameObject);
            }
            else if(other.gameObject == Producer && CurrentState != VehicleState.AtProducer && Vector3.Distance(Agent.destination, Producer.transform.position) < 0.2)
            {
                Debug.Log("Vehicle reached destination");
                CurrentState = VehicleState.AtProducer;
                Agent.isStopped = true;

                var tile = other.gameObject.transform.parent.GetComponent<Tile>();
                var producer = tile.GetChildBuilding().GetComponent<Building>();    //Will using Building instead of Consumer/Producer work?
                producer.VehicleArrived(gameObject);
            }
        }
    }
}
