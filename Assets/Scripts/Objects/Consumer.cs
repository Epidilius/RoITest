using UnityEngine;

public class Consumer : Building
{
    //TODO: Fix the new problem where vehicles are only being spawned once. Test this by reducing the time it takes to make a product
    int CurrentVehicleAmount;
    Producer ClosestProducer;

    public override void Init()
    {
        CurrentVehicleAmount = Settings.GetVehiclesPerConsumer();
        ClosestProducer = FindNearestProducer();
    }
    Producer FindNearestProducer()
    {
        var producers = GameObject.FindGameObjectsWithTag("Producer");

        Producer closestProducer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (var producer in producers)
        {
            var distance = (producer.transform.position - transform.position).sqrMagnitude;
            if (distance < shortestDistance)
            {
                closestProducer = producer.GetComponent<Producer>();
                shortestDistance = distance;
            }
        }

        return closestProducer;
    }
    
    void Update()
    {
        if(IsVehicleReady())
        {
            SendVehicle();
        }
    }

    bool IsVehicleReady()   //TODO: Rename?
    {
        return (CurrentVehicleAmount > 0 && ClosestProducer.GetFutureAmount() < 1);
    }

    void SendVehicle()
    {
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
        StartCoroutine(PauseToUnloadVehicle());
        CurrentVehicleAmount++;
    }    
}
