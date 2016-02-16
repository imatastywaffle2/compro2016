using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public GameObject Vehicle;
    bool inMotion; //Is this object in motion
    float horizontalspeed; //How fast you can move side to side
    float acceleration;
    float rotateSpeed = 1.01f;
    float tiltAngle = 30;
    public Vector3 tempPosition;

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

        GetComponent<Rigidbody>().AddForce(transform.forward * acceleration * InputInfo.Forward());
        GetComponent<Rigidbody>().AddForce(transform.right * horizontalspeed * InputInfo.SideMovement());
	}
}