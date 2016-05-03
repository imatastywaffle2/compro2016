using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Vehicle : Photon.MonoBehaviour, IPunObservable
{
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

    public Slider Velocimeter;

    public bool Stunned = false;


    // Use this for initialization
    void Start ()
    {
        Information = GetComponent<InputInformation>();
        rb = GetComponent<Rigidbody>();

        Velocimeter = GameObject.Find("Velocimeter").GetComponent<Slider>();
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
        if (Stunned == true && vehicleStun <= 0)
        {
            Stunned = false;
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
            Stunned = true;
        }
        else if (shieldActivated)
            vehicleStun = 0;       
    }
    public void CalculateSpeed()
    {
        whatIsSpeed = rb.velocity.magnitude;

        Velocimeter.value = whatIsSpeed;
    }

    public void Boost(float boostSpeed, float boostTime)
    {
        this.boostTime = boostTime;
        this.boostSpeed = boostSpeed;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Serialize(ref Stunned);
            stream.Serialize(ref vehicleStun);
        }
        else
        {
            stream.Serialize(ref Stunned);
            stream.Serialize(ref vehicleStun);
        }
    }

}
