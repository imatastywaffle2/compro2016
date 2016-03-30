using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
    private bool speedBoost;
    public GameObject ItemPickup;
    public float fowardAccel = 3;
    public float maxSpeed = 20;
    public double recoveryTime = 4.5;
    public float horizontalSpeed = 2;
    public float verticalSpeed = 7;
    public float horizontalAccel = 1;
    public float verticalAccel = 1;
    public float minimumSpeed = 5;
    public bool shieldActivated;
    public float bonusSpeed;
    public float vehicleStun;
    public float whatIsSpeed;
    Pickup Pickups;
    InputInformation Information;
    public Rigidbody rb;


    // Use this for initialization
    void Start ()
    {
        Pickups = GetComponent<Pickup>();
        Information = GetComponent<InputInformation>();
        rb = GetComponent<Rigidbody>();
        bonusSpeed = 0;
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (ItemPickup && !ItemPickup.GetComponent<Pickup>().used && Information.UsePickup() == 1)
        {
            UsePickup();         
        }
        else if (ItemPickup && ItemPickup.GetComponent<Pickup>().used && Pickups.timer <= 0 || !shieldActivated || !Pickups.canShoot)
        {
            destroyPickup();   
        }
        else
        {
            bonusSpeed = 0;
            shieldActivated = false;          
        }
        CalculateSpeed();
    }

    void UsePickup()
    {
        Pickups.used = true;
        bonusSpeed = ItemPickup.GetComponent<Pickup>().velocityIncrease;
        shieldActivated = ItemPickup.GetComponent<Pickup>().shield;        
    }
    void destroyPickup()
    {
        Destroy(ItemPickup.gameObject);
    }
    public void Stun()
    {
        if (!shieldActivated)
        {
            vehicleStun = 2;
        }
        else
            vehicleStun = 0;       
    }
    public void CalculateSpeed()
    {
        whatIsSpeed = rb.velocity.magnitude;
    }

    public void boost(float boostSpeed, float boostTime)
    {


    }
}
