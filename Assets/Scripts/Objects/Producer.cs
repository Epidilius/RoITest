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
        ResetProduct();
    }
    
    void Update()
    {
        TimeToNextProduct -= Time.deltaTime / 60f;
        if(TimeToNextProduct <= 0f)
        {
            CreateProduct();
        }
    }


    //PRODUCT RELATED
    void CreateProduct()
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
