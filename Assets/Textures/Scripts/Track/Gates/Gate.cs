using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gate : MonoBehaviour {
    public int GateNumber;
    GateManager gateManager;
    public List<Player> PlayersPassed = new List<Player>();

    void Start ()
    {
        gateManager = gameObject.GetComponentInParent<GateManager>();
    }
	

	void Update ()
    {
	
	}
}
