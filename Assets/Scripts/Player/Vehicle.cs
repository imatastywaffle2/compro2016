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
        if (Pickups && !Pickups.used && Information.UsePickup() == 1)
        {
            UsePickup();         
        }
        else if (Pickups && Pickups.used && Pickups.timer <= 0 || !shieldActivated || !Pickups.canShoot)
        {
            destroyPickup();   
        }
        else
        {
            bonusSpeed = 0;
            shieldActivated = false;          
        }
    }

    void UsePickup()
    {
        Pickups.used = true;
        bonusSpeed = Pickups.velocityIncrease;
        shieldActivated = Pickups.shield;        
    }
    void destroyPickup()
    {
        Destroy(ItemPickup.gameObject);
    }

    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.tag == "PickUp")
        {
            GameObject item = (GameObject)Instantiate(co.gameObject.GetComponent<PickupBox>().PickupType, transform.position, Quaternion.identity);
            item.transform.SetParent(transform);
            ItemPickup = item.GetComponent<Pickup>();
        }        
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
}
