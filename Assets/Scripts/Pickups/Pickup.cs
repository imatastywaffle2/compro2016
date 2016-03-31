﻿using UnityEngine;
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
                velocityIncrease = 150;
                gameObject.GetComponent<Vehicle>().Boost(velocityIncrease, timer);
                timer -= Time.deltaTime;
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
