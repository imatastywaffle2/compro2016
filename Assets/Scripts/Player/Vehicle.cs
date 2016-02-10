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
    public string VehicleName = "Player";
    

    // Use this for initialization
    void Start ()
    {	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GetComponent<InputInformation>().VehicleType == 0)
        {
            fowardAccel = 3;
            maxSpeed = 20;
            recoveryTime = 4.5;
            horizontalSpeed = 5;
            verticalSpeed = 7;
            horizontalAccel = 1;
            verticalAccel = 1;
            minimumSpeed = 5;
        }

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
