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
    public float bonusSpeed;
    Pickup Pickups;
    InputInformation Information;


    // Use this for initialization
    void Start ()
    {
        Pickups = GetComponent<Pickup>();
        Information = GetComponent<InputInformation>();
        bonusSpeed = 0;
    }
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (Pickups.timer > 0 && Information.UsePickup() == 1 && ItemPickup.name == "PickupVelocityIncrease")
        {
            UsePickup();
        }
        else if (Pickups.timer <= 0)
        {
            bonusSpeed = 0;
        }
    }

    void UsePickup()
    {
        Pickups.timer -= Time.deltaTime;
        bonusSpeed = 150;
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
}
