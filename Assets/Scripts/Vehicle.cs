using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    [SerializeField] GameObject VehicleObject;
    ResourcePool VehiclePool;
    int MaxVehiclesPerConsumer = 1;
    int NumberOfConsumers = 5;  //TODO: A better way of doing this

    // Use this for initialization
    void Start()
    {
        VehiclePool = new ResourcePool(MaxVehiclesPerConsumer * NumberOfConsumers, VehicleObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
