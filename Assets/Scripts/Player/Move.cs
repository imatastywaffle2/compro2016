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
    private Vector3 mousePosition;
    float invertOrNot;
    float velocityZerotoOne;
    public ParticleSystem[] engines;   
    public float stunDuration;
    Vehicle Vehicles;
    InputInformation InputInfo;


    // Use this for initialization
    void Start ()
    {
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
            GetComponent<Rigidbody>().AddForce(transform.forward * (acceleration + Vehicles.bonusSpeed) * InputInfo.Forward());
            GetComponent<Rigidbody>().AddForce(transform.right * horizontalspeed * InputInfo.SideMovement());
            transform.Rotate(Vector3.forward * rotateSpeed * InputInfo.RotateShip());
            transform.Rotate(Vector3.right * turnSpeed * InputInfo.AxisY());
            transform.Rotate(Vector3.up * turnSpeed * InputInfo.AxisX());          
            foreach(ParticleSystem engine in engines)
            {
                if (InputInfo.Forward() > 0)
                    engine.Play();
                else
                    engine.Stop();
            }
        }
        else
        {
            stunDuration -= Time.deltaTime;
        }
        

    }



}