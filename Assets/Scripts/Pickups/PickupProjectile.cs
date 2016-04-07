using UnityEngine;
using System.Collections;

public class PickupProjectile : Pickup {
    public GameObject projectile;

    public override void Use()
    {
        Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(this);
    }

}