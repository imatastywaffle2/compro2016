using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class StartGate : Photon.MonoBehaviour, IPunObservable
{
    List<Player> places = new List<Player>();
    public GateManager gateManager;

    // Use this for initialization
    void Start ()
    {
	   
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (col.GetComponent<Player>().currentGate >= gateManager.Gates.Length)
            {
                places.Add(col.GetComponent<Player>());
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {

        }
        else
        {

        }
    }
}
