using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

    public bool trackTarget;
    public float stunDuration = 2;

    // Speed
    public float speed = 700;

    // Target
    public Transform target;
    Pickup Pickups;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (trackTarget)
        {
            if (target)
            {
                transform.LookAt(target.position);
                // Fly towards the target        
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
            else {
                // Otherwise destroy self
                Destroy(gameObject);
            }
        }
        else
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * speed);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer != 8)
        {
            other.gameObject.GetComponent<Vehicle>().Stun();
            Destroy(gameObject);           
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider otherShip)
    {
        //make a code for detecting a ship that isnt yourself.
        if (otherShip.gameObject.layer != 8 && otherShip.gameObject.layer != 0)
        {
            target = otherShip.transform;
        }
        else
        {
            target = null;
        }
    }
}
