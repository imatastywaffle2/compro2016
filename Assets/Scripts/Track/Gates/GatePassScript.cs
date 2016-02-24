using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatePassScript : MonoBehaviour
{
    List<Vehicle> PlayersPassed = new List< Vehicle>();

	void Start ()
    {
	
	}

    void Update ()
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {

        }
    }

}
