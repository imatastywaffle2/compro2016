using UnityEngine;
using System.Collections;

public class PickupBoost : Pickup {
    float boostSpeed = 10;
    float boostTime = 2;

    public override void Use(Vehicle vehicle)
    {
        vehicle.Boost(boostSpeed, boostTime);
    }
}
