using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public GameObject Vehicle;
    bool inMotion; //Is this object in motion
    float horizontalspeed; //How fast you can move side to side
    float verticalspeed; //How fast you can ascend or descend
    float amplitude = 1;
    float fowardspeed = 0;
    float acceleration;
    float rotateSpeed = 3;
    float tiltAngle = 30;
    float decelleration = 1;
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
        verticalspeed = Vehicles.verticalSpeed;


        GetComponent<Rigidbody>().AddForce(transform.right * horizontalspeed * InputInfo.SideMovement());
        GetComponent<Rigidbody>().AddForce(transform.forward * acceleration * InputInfo.Forward());
	}
}