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
        
        if (Input.GetKey(KeyCode.W) && horizontalspeed <= Vehicles.maxSpeed)
        {
            horizontalspeed += Vehicles.horizontalAccel;
        }
        else if (horizontalspeed > 0)
        {
                horizontalspeed -= decelleration;
        }
        if (Input.GetKey(KeyCode.S) && horizontalspeed <= Vehicles.maxSpeed)
        {
            horizontalspeed -= Vehicles.horizontalAccel;
        }
        else if (horizontalspeed < 0)
        {
            horizontalspeed += decelleration;
        }
        if (Input.GetKey(KeyCode.A) && fowardspeed <= Vehicles.maxSpeed)
        {
            fowardspeed += acceleration;
        }
            tempPosition.x += horizontalspeed;
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalspeed) * amplitude;
        tempPosition.z += fowardspeed;
        transform.position = tempPosition;

        GetComponent<Rigidbody>().AddForce(transform.forward * acceleration * InputInfo.Forward());
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * rotateSpeed);
        
	}
}