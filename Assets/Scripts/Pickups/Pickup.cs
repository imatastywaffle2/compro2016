using UnityEngine;
using System.Collections;


public class Pickup : MonoBehaviour {
    Vehicle vehicle;
    InputInformation InputInfo;
    PickupUI pickupUI;

    public void Start()
    {
        vehicle = GetComponent<Vehicle>();
        InputInfo = GetComponent<InputInformation>();
        pickupUI = GameObject.FindObjectOfType<PickupUI>();
    }

    public void FixedUpdate()
    {
        if (InputInfo.UsePickup())
        {
            Use();
            pickupUI.disableIcon();
        }
    }

    public virtual void Use()
    {
        
    }
}
