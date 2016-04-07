using UnityEngine;
using System.Collections;

public class PickupProjectile : Pickup {
    public Projectile projectile;

    public override void Use()
    {
        base.Use();
        Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);
        Destroy(this);
    }

}
