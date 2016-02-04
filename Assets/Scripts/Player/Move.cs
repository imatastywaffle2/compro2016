using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    public GameObject Vehicle;
    bool disabled; //Unable to move
    bool inMotion; //Is this object in motion
    float horizontalspeed; //How fast you can move side to side
    float verticalspeed; //How fast you can ascend or descend
    float amplitude;
    float fowardspeed;
    float acceleration;
    public Vector3 tempPosition;
    public Vector3 direction = new Vector3(0, 0, 0);

    Vehicles Vehicles;
    InputInformation InputInfo;

	// Use this for initialization
	void Start ()
    {
        tempPosition = transform.position;
        Vehicles = GetComponent<Vehicles>();
        InputInfo = GetComponent<InputInformation>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        acceleration = Vehicles.fowardAccel;
        horizontalspeed = Vehicles.horizontalSpeed;
        verticalspeed = Vehicles.verticalSpeed;
        if (disabled != true & Input.GetKey("a"))
        {
            horizontalspeed += Vehicles.horizontalAccel;
        }
        if (disabled != true & Input.GetKey("d"))
        {
            horizontalspeed -= Vehicles.horizontalAccel;
        }
        tempPosition.x += horizontalspeed;
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalspeed) * amplitude;
        tempPosition.z += fowardspeed;
        transform.position = tempPosition;

        GetComponent<Rigidbody>().AddForce(transform.forward * acceleration * InputInfo.Forward());
	}
}