using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : Building
{
    int CurrentVehicleAmount;
    GameObject ClosestProducer;
    
    public override void Init()
    {
        CurrentVehicleAmount = Settings.GetVehiclesPerConsumer();
        ClosestProducer = FindNearestProducer();
    }
    GameObject FindNearestProducer()
    {
        var producers = GameObject.FindGameObjectsWithTag("Producer");

        GameObject closestProducer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var producer in producers)
        {
            var distance = (producer.transform.position - transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                closestProducer = producer;
                shortestDistance = distance;
            }
        }

        return closestProducer;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsVehicleReady())
        {
            SendVehicle();
        }
    }

    bool IsVehicleReady()
    {
        return CurrentVehicleAmount > 0;
    }

    void SendVehicle()
    {
        var vehicle = GameObject.Find("WorldBoss").GetComponent<PoolBoss>().GetUnusedVehicle();  //TODO: Better way of doing this
        PrepVehicle(vehicle);
        SetVehicleDestinations(vehicle);
        vehicle.GetComponent<Vehicle>().StartDriving(); //TODO: Change these to be Vehicle and Tile types

        CurrentVehicleAmount--;
    }
    void PrepVehicle(GameObject vehicle)
    {
        var destination = GetEndpoint();
        vehicle.GetComponent<Vehicle>().SetTransform(destination.transform);
    }
    void SetVehicleDestinations(GameObject vehicle)
    {
        vehicle.GetComponent<Vehicle>().SetHome(GetEndpoint());
        vehicle.GetComponent<Vehicle>().SetDestination(ClosestProducer.GetComponent<Producer>().GetEndpoint());
    }

    public void VehicleArrived()
    {
        AddOneProduct();
        CurrentVehicleAmount++;
    }
}
