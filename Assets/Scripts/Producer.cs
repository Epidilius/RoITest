using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : Building
{
    [SerializeField] float ProductionTime = 1;
    float TimeToNextProduct;
    int FutureProductAmount;    //TODO: Use this for determining if a vehicle should be sent

    // Use this for initialization
    void Start()
    {
        TimeToNextProduct = ProductionTime;
    }

    // Update is called once per frame
    void Update()
    {
        TimeToNextProduct -= Time.deltaTime / 100f; //TODO: Do the /100f?
        if(TimeToNextProduct <= 0f)
        {
            CreateProdcut();
        }
    }

    public void VehicleArrived()
    {
        RemoveOneProduct();
    }

    void CreateProdcut()
    {
        AddOneProduct();
        TimeToNextProduct = ProductionTime;
    }

    public void SetProductionSpeed(int speed)
    {
        ProductionTime = speed;
    }
}
