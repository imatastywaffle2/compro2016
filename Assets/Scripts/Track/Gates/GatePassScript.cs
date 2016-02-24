using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatePassScript : MonoBehaviour
{
    List<Player> PlayersPassed = new List< Player>();

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
            if (!PlayersPassed.Contains(col.gameObject.GetComponent<Player>()))
                PlayersPassed.Add(col.gameObject.GetComponent<Player>());
        }
    }

}
