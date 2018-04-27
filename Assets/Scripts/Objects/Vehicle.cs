using UnityEngine;
using UnityEngine.AI;

public class Vehicle : RoITestObject
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
    VehicleState CurrentState;
    
    void Awake()
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
        Producer = producer;
    }
    public void SetTransform(Transform newTransform)
    {
        gameObject.GetComponent<NavMeshAgent>().Warp(newTransform.position);
    }
    
    public bool StartDriving()
    {
        if (!Agent.isActiveAndEnabled || !Agent.isOnNavMesh)
            return false;

        Agent.isStopped = false;
        var destinationSet = false;

        if (CurrentState == VehicleState.AtProducer)
        {
            destinationSet = Agent.SetDestination(Consumer.transform.position);
        }
        else
        {
            destinationSet = Agent.SetDestination(Producer.transform.position);
        }

        CurrentState = VehicleState.InTransit;

        return destinationSet;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Destination")
        {
            //TODO: Pause, then continue
            //TODO: Undupe this code
            if (other.gameObject == Consumer && CurrentState != VehicleState.AtConsumer && Vector3.Distance(Agent.destination, Consumer.transform.position) < 0.2)
            {
                CurrentState = VehicleState.AtConsumer;
                VehicleArrived(other.gameObject);
            }
            else if(other.gameObject == Producer && CurrentState != VehicleState.AtProducer && Vector3.Distance(Agent.destination, Producer.transform.position) < 0.2)
            {
                CurrentState = VehicleState.AtProducer;
                VehicleArrived(other.gameObject);
            }
        }
    }
    void VehicleArrived(GameObject buildingObject)
    {
        Agent.isStopped = true;
        buildingObject.transform.parent.GetComponent<Tile>().GetChildBuilding().VehicleArrived(gameObject);
    }

    public override void Init()
    {
        
    }
}
