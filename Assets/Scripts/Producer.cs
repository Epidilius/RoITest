using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : Building
{
    int ProductionSpeed;
    int FutureProductAmount;    //TODO: Use this for determining if a vehicle should be sent

    // Use this for initialization
    void Start()
    {

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
