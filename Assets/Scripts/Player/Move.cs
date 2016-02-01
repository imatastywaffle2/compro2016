using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
    bool disabled;
    bool inMotion;
    float horizontalspeed;
    float verticalspeed;
    float amplitude;
    float fowardspeed; 

    public Vector3 tempPosition;
	// Use this for initialization
	void Start ()
    {
        tempPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        tempPosition.x += horizontalspeed;
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalspeed) * amplitude;
        tempPosition.z += fowardspeed;
        transform.position = tempPosition;	
	}
}