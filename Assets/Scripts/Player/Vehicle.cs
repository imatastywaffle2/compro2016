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
    public GameObject shield;

    public Slider Velocimeter;

    public bool Stunned = false;


    // Use this for initialization
    void Start ()
    {
        Information = GetComponent<InputInformation>();
        rb = GetComponent<Rigidbody>();

       
    }

    void Awake()
    {
        
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
       
        if(Velocimeter == null && this.photonView.isMine)
        {
            if(GameObject.Find("Velocimeter"))
                Velocimeter = GameObject.Find("Velocimeter").GetComponent<Slider>();
        }
        CalculateSpeed();
        boostTime -= Time.deltaTime;
        vehicleStun -= Time.deltaTime;
        
        if (boostTime <= 0)
        {
            boostSpeed = 0;
        }
        if (Stunned == true && vehicleStun <= 0)
        {
            Stunned = false;
        }
    }

    void destroyPickup()
    {
       
    }

    [PunRPC]
    public void Stun()
    {
        if (!shieldActivated)
        {

            vehicleStun = 2;
            Stunned = true;
            Console.WriteLine("Should be stunned");
        }
        else if (shieldActivated)
        {
            shieldActivated = false;
            shield.SetActive(false);

        }  
    }

    [PunRPC]
    public void ActivateShield()
    {
        shield.SetActive(true);
        shieldActivated = true;
    }
   
    public void StunRemote()
    {
        photonView.RPC("Stun", PhotonTargets.All);
    }

    public void CalculateSpeed()
    {
        whatIsSpeed = rb.velocity.magnitude;
        if(this.photonView.isMine)
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
            stream.Serialize(ref shieldActivated);
        }
        else
        {
            stream.Serialize(ref Stunned);
            stream.Serialize(ref vehicleStun);
            stream.Serialize(ref shieldActivated);
        }
    }

}
