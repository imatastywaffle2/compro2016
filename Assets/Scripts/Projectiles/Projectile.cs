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
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Vehicle>().Stun();
            Destroy(gameObject);           
        }
    }
    void OnTriggerEnter(Collider otherShip)
    {
        //make a code for detecting a ship that isnt yourself.
    }


}
