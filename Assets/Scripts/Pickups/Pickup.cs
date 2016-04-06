using UnityEngine;
using System.Collections;


public class Pickup : MonoBehaviour {
    Vehicle vehicle;
    InputInformation InputInfo;

    public void Start()
    {
        vehicle = GetComponent<Vehicle>();
        InputInfo = GetComponent<InputInformation>();
    }

    public void FixedUpdate()
    {
        if (InputInfo.UsePickup())
        {
            Use();
        }
    }

    public virtual void Use()
    {       
    }
}
