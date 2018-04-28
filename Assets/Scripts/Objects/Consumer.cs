using UnityEngine;

public class Consumer : Building
{
    [SerializeField] float VehicleCooldownDuration;
    float VehicleCooldown;
    int CurrentVehicleAmount;
    public Producer ClosestProducer;    //TODO: Remove the public once done debugging

    public override void Init()
    {
        VehicleCooldownDuration = Settings.GetVehicleCooldownDuration();
        CurrentVehicleAmount = Settings.GetVehiclesPerConsumer();

        ResetVehicleCooldown();
        ResetProduct();
    }
    public void SetNearestProducer(GameObject producer)
    {
        ClosestProducer = producer.GetComponent<Producer>();
    }
    
    void Update()
    {
        if (IsVehicleReady())
        {
            SendVehicle();
        }
        else
        {
            VehicleCooldown -= Time.deltaTime / 60f;
            if (VehicleCooldown <= 0f)
            {
                
            }
        }
    }

    bool IsVehicleReady()   //TODO: Rename?
    {
        return (CurrentVehicleAmount > 0 && ClosestProducer.GetFutureAmount() > 0 && VehicleCooldown <= 0f);
    }
    void ResetVehicleCooldown()
    {
        VehicleCooldown = VehicleCooldownDuration;
    }

    void SendVehicle()
    {
        Debug.Log(name + " sending vehicle to " + ClosestProducer.name);
        var vehicle = GameObject.Find("WorldBoss").GetComponent<PoolBoss>().GetUnusedObject<Vehicle>();  //TODO: Better way of doing this
        PrepVehicle(vehicle.GetComponent<Vehicle>());

        if (!vehicle.GetComponent<Vehicle>().StartDriving())
        {
            GameObject.Find("WorldBoss").GetComponent<PoolBoss>().ReturnItemToPool(vehicle);
            return;
        }

        ClosestProducer.VehicleEnRoute();
        CurrentVehicleAmount--;
    }
    void PrepVehicle(Vehicle vehicle)
    {
        var destination = GetEndpoint();
        vehicle.SetTransform(destination.transform);

        SetVehicleHome(vehicle);
        SetVehicleDestination(vehicle);
    }
    void SetVehicleHome(Vehicle vehicle)
    {
        vehicle.SetConsumer(GetEndpoint());
    }
    void SetVehicleDestination(Vehicle vehicle)
    {
        vehicle.SetProducer(ClosestProducer.GetComponent<Producer>().GetEndpoint());
    }

    public override void VehicleArrived(GameObject vehicle)
    {
        GameObject.Find("WorldBoss").GetComponent<PoolBoss>().ReturnItemToPool(vehicle);    //TODO: I dont like the GetComponents in this class
        AddOneProduct();
        CurrentVehicleAmount++;
        ResetVehicleCooldown();
    }    
}
