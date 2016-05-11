using UnityEngine;
using System.Collections;

public class PickupProjectile : Pickup {
    public GameObject projectile;

    public override void Use()
    {
        GameObject projectile = PhotonNetwork.Instantiate("Projectile", transform.position, transform.rotation);
        Destroy(this);
    }

}