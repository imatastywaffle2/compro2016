using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
    public bool used = false;
    public float velocityIncrease;
    public bool shield = false;
    public Quaternion scale;
    public GameObject projectile;
    public float timer;
    public bool canShoot = true;

    void FixedUpdate()
    {
        if (used)
        {
            if (gameObject.name == "PickupVelocityIncrease")
            {
                timer -= Time.deltaTime;
                velocityIncrease = 150;
            }
            if (gameObject.name == "PickupShield")
            {
                shield = true;
            }
            if (gameObject.name == "PickupProjectile")
            {              
                ShootProjectile();
                canShoot = false;
            }
        }

    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Projectile")
        {
            shield = false;
        }
    }
    void ShootProjectile()
    {
        Instantiate(projectile);
    }

}
