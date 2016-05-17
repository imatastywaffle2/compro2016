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

    

    void Start()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (col.gameObject.layer == 8)
            {
                Vehicle player = col.gameObject.GetComponent<Vehicle>();
                if (player.ItemPickup == null)
                {

                    if (this.PickupType == Pickups.Boost)
                    {
                        player.ItemPickup = player.gameObject.AddComponent<PickupBoost>();
                    }
                    else if (this.PickupType == Pickups.Shield)
                    {
                        player.ItemPickup = player.gameObject.AddComponent<PickupShield>();
                    }
                    else if (this.PickupType == Pickups.Projectile)
                    {
                        player.ItemPickup = player.gameObject.AddComponent<PickupProjectile>();
                    }
                    GameObject.FindObjectOfType<PickupUI>().enableIcon(this.PickupType);

                }
            }

        }
    }
}