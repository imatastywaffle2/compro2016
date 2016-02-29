using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public GameObject Vehicle;
    bool inMotion; //Is this object in motion
    float horizontalspeed; //How fast you can move side to side
    float acceleration;
    float rotateSpeed = 1.2f;
    float turnSpeed = 1.5f;
    //float tiltAngle = 5; maybe use later
    public Vector3 tempPosition;
    private Vector3 mousePosition;

    public float stunDuration = 0;

    Vehicle Vehicles;
    InputInformation InputInfo;

	// Use this for initialization
	void Start ()
    {
        
        tempPosition = transform.position;
        Vehicles = GetComponent<Vehicle>();
        InputInfo = GetComponent<InputInformation>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        acceleration = Vehicles.fowardAccel;
        horizontalspeed = Vehicles.horizontalSpeed;
        if (stunDuration <= 0)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward * acceleration * InputInfo.Forward());
            GetComponent<Rigidbody>().AddForce(transform.right * horizontalspeed * InputInfo.SideMovement());
            transform.Rotate(Vector3.forward * rotateSpeed * InputInfo.RotateShip());
            transform.Rotate(Vector3.right * turnSpeed * InputInfo.AxisY());
            transform.Rotate(Vector3.up * turnSpeed * InputInfo.AxisX());
        }
        else
        {
            stunDuration -= Time.deltaTime;
        }
	}

    
}