using UnityEngine;

public static class Settings
{
    static Vector2 WorldSize = new Vector2(50, 50);
    static int NumberOfProducers = 2;
    static int NumberOfConsumers = 5;
    static int MaxVehiclesPerConsumer = 1;
    static int MinutesToMakeProduct = 1;
    static int VehicleCooldownDurationModifier = 2;
    static int TileSize = 5;
   
    public static void SetWorldSize(Vector2 size)
    {
        WorldSize = size;
    }
    public static void SetNumberOfProducers(int amount)
    {
        NumberOfProducers = amount;
    }
    public static void SetNumberOfConsumers(int amount)
    {
        NumberOfConsumers = amount;
    }
    public static void SetVehiclesPerConsumer(int amount)
    {
        MaxVehiclesPerConsumer = amount;
    }
    public static void SetMinutesToMakeProduct(int time)
    {
        MinutesToMakeProduct = time;
    }

    public static Vector2 GetWorldSize()
    {
        return WorldSize;
    }
    public static int GetTotalTileAmount()
    {
        return (int)(WorldSize.x * WorldSize.y);
    }
    public static int GetNumberOfProducers()
    {
        return NumberOfProducers;
    }
    public static int GetNumberOfConsumers()
    {
        return NumberOfConsumers;
    }
    public static int GetVehiclesPerConsumer()
    {
        return MaxVehiclesPerConsumer;
    }
    public static int GetMinutesToMakeProduct()
    {
        return MinutesToMakeProduct;
    }
    public static int GetVehicleCooldownDuration()
    {
        return MinutesToMakeProduct * VehicleCooldownDurationModifier;
    }
    public static int GetTileSize()
    {
        return TileSize;
    }
}
