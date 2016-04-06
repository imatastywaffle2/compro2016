using UnityEngine;
using System.Collections;

public class PickupBoost : Pickup {
    float boostSpeed = 100;
    float boostTime = 2.5f;

    public override void Use(Vehicle vehicle)
    {
        vehicle.Boost(boostSpeed, boostTime);
    }
}
