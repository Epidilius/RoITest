using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //TODO: Use PlayerPrefs instead? Go singleton?
    static Vector2 WorldSize = new Vector2(50, 50);
    static int NumberOfProducers = 2;
    static int NumberOfConsumers = 5;
    static int MaxVehiclesPerConsumer = 1;
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

    public static Vector2 GetWorldSize()
    {
        return WorldSize;
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
    public static int GetTileSize()
    {
        return TileSize;
    }
}
