using UnityEngine;
using System.Collections;

public enum Pickups
{
    Shield,
    Boost,
    Projectile
}


public class PickupBox : MonoBehaviour
{
    public Pickups PickupType;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Vehicle player = col.gameObject.GetComponent<Vehicle>();
            if (player.ItemPickup == null)
            {
            
                if (this.PickupType == Pickups.Boost)
                {
                    player.ItemPickup = new PickupBoost();
                }
                else if (this.PickupType == Pickups.Shield)
                {
                    player.ItemPickup = new PickupShield();
                }
                else if (this.PickupType == Pickups.Projectile)
                {
                    player.ItemPickup = new PickupProjectile();
                }
                Destroy(gameObject);
            }

        }
    }


}