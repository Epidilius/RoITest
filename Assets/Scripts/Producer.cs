using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : Building
{
    [SerializeField] GameObject ProducerBuilding;
    ResourcePool ProducerPool;
    int NumberOfProducers = 2;
    int ProductionSpeed;
    int FutureProductAmount;    //TODO: Use this for determining if a vehicle should be sent

    // Use this for initialization
    void Start()
    {
        ProducerPool = new ResourcePool(NumberOfProducers, ProducerBuilding);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetProductionSpeed(int speed)
    {
        ProductionSpeed = speed;
    }
}
