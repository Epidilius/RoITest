using UnityEngine;

public class Producer : Building
{
    [SerializeField] float ProductionTimeInMinutes;
    float TimeToNextProduct;
    int FutureProductAmount;

    public override void Init()
    {
        ProductionTimeInMinutes = Settings.GetMinutesToMakeProduct();
        ResetTimeToNextProduct();
    }
    
    void Update()
    {
        TimeToNextProduct -= Time.deltaTime / 6f;
        if(TimeToNextProduct <= 0f)
        {
            CreateProdcut();
        }
    }


    //PRODUCT RELATED
    void CreateProdcut()
    {
        AddOneProduct();
        FutureProductAmount++;
        ResetTimeToNextProduct();
    }    
    void ResetTimeToNextProduct()
    {
        TimeToNextProduct = ProductionTimeInMinutes;
    }

    //VEHICLE RELATED
    public override void VehicleArrived(GameObject vehicle)
    {
        RemoveOneProduct();
        vehicle.GetComponent<Vehicle>().StartDriving();
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
