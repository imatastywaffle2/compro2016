using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    Transform target;
    float cameraToCarID;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!target)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            if (temp.layer == 8)
            {
                target = temp.transform;
            }
        }       
    }   
    void FixedUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, .3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, .3f);
        }
    }
}
