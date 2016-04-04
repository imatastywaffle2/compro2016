using UnityEngine;
using System.Collections;

public class Vehicle : MonoBehaviour {
    private bool speedBoost;
    public Pickup ItemPickup;
    public float fowardAccel = 3;
    public float maxSpeed = 20;
    public double recoveryTime = 4.5;
    public float horizontalSpeed = 2;
    public float verticalSpeed = 7;
    public float horizontalAccel = 1;
    public float verticalAccel = 1;
    public float minimumSpeed = 5;
    public bool shieldActivated;
    public float vehicleStun;
    public float whatIsSpeed;
    public float boostTime;
    public float boostSpeed;
    public bool boostOn;
    InputInformation Information;
    public Rigidbody rb;


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
    }

    void UsePickup()
    {
        
    }
    void destroyPickup()
    {
       
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

    public void Boost(float boostSpeed, float boostTime)
    {
        this.boostTime = boostTime;
        this.boostSpeed = boostSpeed;
    }
}
