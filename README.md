Rise of Industry Code Test

----  ----  ----  ----

Controls

WASD - Move around

Mouse - Look

Left Shift - Speed up

R - Rebuild world

----  ----  ----  ----

Game Behaviour

Producers create products at a rate of once per minute, by default. If a consumer has a vehicle ready, it will dispatch it to the producer. Vehicle readiness is determined by three factors:

1) Will the producer have a product when it arrives?

2) Does the consumer have a vehicle that is not dispatched?

3) Has the vehicle cooldown exhausted?

If the answer to those three questions is "Yes", a vehicle can be dispatched. Once a vehicle arrives at the producer, a product is removed from the producer's count, and the vehicle returns to the consumer that dispatched it. Upon return, the vehicle is removed from play, and the consumer's product count ticks up.

The vehcile cooldown is determined by this equation:

MinutesToMakeProduct * (VehicleCooldownDurationModifier * (GetNumberOfConsumers() / GetNumberOfProducers())

This was done to let the vehicles stagger a bit, and to stop a single consumer from monopolizing a producer.

These times, as well as some other values, can be modified in the Settings.cs file.


----  ----  ----  ----

Misc Technical Stuff

There are two different types of navigation in the project. A* is used to find paths between consumers and producers. Unity's NavMesh system is used to navigate the cars on the road tiles.
