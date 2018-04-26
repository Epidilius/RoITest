using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : Building
{
    int CurrentVehicleAmount;
    Producer ClosestProducer;
    GameObject Vehicle;
    
    public override void Init()
    {
        CurrentVehicleAmount = Settings.GetVehiclesPerConsumer();
        ClosestProducer = FindNearestProducer();
    }
    Producer FindNearestProducer()
    {
        var producers = GameObject.FindGameObjectsWithTag("Producer");

        Producer closestProducer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var producer in producers)
        {
            var distance = (producer.transform.position - transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                closestProducer = producer.GetComponent<Producer>();
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
        if(ClosestProducer.GetFutureAmount() < 1)
        {
            return;
        }

        var vehicle = GameObject.Find("WorldBoss").GetComponent<PoolBoss>().GetUnusedVehicle();  //TODO: Better way of doing this
        PrepVehicle(vehicle);
        SetVehicleHome(vehicle);
        SetVehicleDestination(vehicle); //TODO: Change these to be Vehicle and Tile types

        if (!vehicle.GetComponent<Vehicle>().StartDriving())
        {
            //TODO: Should I do this? I don't think so 
            GameObject.Find("WorldBoss").GetComponent<PoolBoss>().ReturnItemToPool(vehicle);
            return;
        }

        ClosestProducer.VehicleEnRoute();
        CurrentVehicleAmount--;
    }
    void PrepVehicle(GameObject vehicle)
    {
        var destination = GetEndpoint();
        vehicle.GetComponent<Vehicle>().SetTransform(destination.transform);
    }
    void SetVehicleHome(GameObject vehicle)
    {
        vehicle.GetComponent<Vehicle>().SetConsumer(GetEndpoint());
    }
    void SetVehicleDestination(GameObject vehicle)
    {
        vehicle.GetComponent<Vehicle>().SetProducer(ClosestProducer.GetComponent<Producer>().GetEndpoint());
    }

    public override void VehicleArrived(GameObject vehicle)
    {
        GameObject.Find("WorldBoss").GetComponent<PoolBoss>().ReturnItemToPool(vehicle);
        AddOneProduct();
        StartCoroutine(PauseToUnloadVehicle());
        CurrentVehicleAmount++;
    }    
}
