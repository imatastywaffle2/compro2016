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
            //if (this.PickupType == Pickups.Boost)
                //col.gameObject.GetComponent
        }
    }


}