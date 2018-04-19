using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : Building
{
    [SerializeField] GameObject ConsumerBuilding;
    ResourcePool ConsumerPool;
    int NumberOfConsumers = 5;
    int CurrentVehicleAmount = 0;

    // Use this for initialization
    void Start()
    {
        ConsumerPool = new ResourcePool(NumberOfConsumers, ConsumerBuilding);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetNumberOfConsumers()
    {
        return NumberOfConsumers;
    }

    void SendVehicle()
    {

    }

    Producer FindNearestProducer()
    {
        throw new Exception();
    }

    public void VehicleReturned()
    {
        AddOneProduct();
        CurrentVehicleAmount--;
    }
}
