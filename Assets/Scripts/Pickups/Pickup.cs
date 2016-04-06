using UnityEngine;
using System.Collections;


public class Pickup : MonoBehaviour {
    Vehicle vehicle;
    InputInformation InputInfo;

    public void Start()
    {
        vehicle = GetComponent<Vehicle>();
    }

    public void FixedUpdate()
    {
        if (InputInfo.UsePickup() == 1)
        {
            Use(vehicle);
        }
    }

    public virtual void Use(Vehicle vehicle)
    {
        vehicle.UsePickup();
    }


}
