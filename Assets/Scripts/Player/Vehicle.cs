using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
    private bool speedBoost;
    public Pickup ItemPickup;
    public float fowardAccel;
    public float maxSpeed;
    public double recoveryTime;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float horizontalAccel;
    public float verticalAccel;
    public float minimumSpeed;
    public bool shieldActivated;
    public float vehicleStun;
    public float whatIsSpeed;
    public float boostTime;
    public float boostSpeed;
    public bool boostOn;
    InputInformation Information;
    public Rigidbody rb;
    public GameObject projectile;


    // Use this for initialization
    void Start ()
    {
        Information = GetComponent<InputInformation>();
        rb = GetComponent<Rigidbody>();
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
       
        CalculateSpeed();
        boostTime -= Time.deltaTime;
        if (boostTime <= 0)
        {
            boostSpeed = 0;
        }
        if (vehicleStun <= 0)
        {
            gameObject.GetComponent<InputInformation>().enabled = true;
            gameObject.GetComponent<Move>().enabled = true;
        }
    }

    public void UsePickup()
    {
        if (ItemPickup)
        {
            
        }
    }
    void destroyPickup()
    {
       
    }
    public void Stun()
    {
        if (!shieldActivated)
        {
            vehicleStun = 2;
            gameObject.GetComponent<InputInformation>().enabled = false;
            gameObject.GetComponent<Move>().enabled = false;
        }
        else if (shieldActivated)
            vehicleStun = 0;       
    }
    public void CalculateSpeed()
    {
        whatIsSpeed = rb.velocity.magnitude;
    }

    public void Boost(float boostSpeed, float boostTime)
    {
        this.boostTime = boostTime;
        this.boostSpeed = boostSpeed;
    }
}
