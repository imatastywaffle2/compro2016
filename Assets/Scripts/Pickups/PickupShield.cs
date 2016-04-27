using UnityEngine;
using System.Collections;

public class PickupShield : Pickup {

    // Use this for initialization
    public override void Use()
    {
        gameObject.GetComponent<Vehicle>().shieldActivated = true;
        Destroy(this);
    }
}
