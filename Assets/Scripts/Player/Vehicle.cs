using UnityEngine;
using System.Collections;

public class Vehicles : MonoBehaviour {
    private bool speedBoost;
    private int ItemPickup;
    public float fowardAccel;
    public float maxSpeed;
    public double recoveryTime;
    public float horizontalSpeed;
    public float verticalSpeed;
    public float horizontalAccel;
    public float verticalAccel;
    public float maxHorizontalSpeed;
    public float maxVerticalSpeed;

    // Use this for initialization
    void Start ()
    {	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(GetComponent<InputInformation>().VehicleType == 1)
        {
            fowardAccel = 3;
            maxSpeed = 20;
            recoveryTime = 4.5;
            horizontalSpeed = 5;
            verticalSpeed = 7;
        }

    }
    void OnCollisionEnter(Collision co)
    {
        if(co.gameObject.name == "PickUp")
        {
            //Fill with Code for getting item to use
        }
    }
}
