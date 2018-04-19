using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumer : Building
{
    int CurrentVehicleAmount = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SendVehicle()
    {

    }

    Producer FindNearestProducer()
    {
        throw new Exception();
    }

    public void VehicleArrived()
    {
        AddOneProduct();
        CurrentVehicleAmount--;
    }
}
