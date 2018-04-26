using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Producer : Building
{
    [SerializeField] float ProductionTime = 1;
    float TimeToNextProduct;
    int FutureProductAmount;    //TODO: Use this for determining if a vehicle should be sent

    public override void Init()
    {
        TimeToNextProduct = ProductionTime;
    }

    // Update is called once per frame
    void Update()
    {
        TimeToNextProduct -= Time.deltaTime / 60f;
        if(TimeToNextProduct <= 0f)
        {
            CreateProdcut();
        }
    }

    public override void VehicleArrived(GameObject vehicle)
    {
        RemoveOneProduct();
        StartCoroutine(PauseToUnloadVehicle());
        vehicle.GetComponent<Vehicle>().StartDriving();
    }

    void CreateProdcut()
    {
        AddOneProduct();
        FutureProductAmount++;
        TimeToNextProduct = ProductionTime;
    }

    public void SetProductionSpeed(int speed)
    {
        ProductionTime = speed;
    }

    public void VehicleEnRoute()
    {
        FutureProductAmount--;
    }
    public int GetFutureAmount()
    {
        return FutureProductAmount;
    }
}
