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
	// Use this for initialization
	void Start ()
    {
        tempPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        acceleration = GetComponent<Vehicles>().fowardAccel;
        horizontalspeed = GetComponent<Vehicles>().horizontalSpeed;
        verticalspeed = GetComponent<Vehicles>().verticalSpeed;
        tempPosition.x += horizontalspeed;
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalspeed) * amplitude;
        tempPosition.z += fowardspeed;
        transform.position = tempPosition;	
	}
}