using UnityEngine;
using System.Collections;

public class PickupBoost : Pickup {
    float boostSpeed = 300;
    float boostTime = 2.5f;

    public override void Use()
    {
        GetComponent<Vehicle>().Boost(boostSpeed, boostTime);
        Destroy(this);
    }
}
