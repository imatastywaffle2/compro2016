using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gate : MonoBehaviour
{
    public int GateNumber;
    GateManager gateManager;
    public ParticleSystem NextGateParticles;
    public List<Player> PlayersPassed = new List<Player>();

    void Start ()
    {
        gateManager = gameObject.GetComponentInParent<GateManager>();
    }
	

	void Update ()
    {
	    
	}

    public void nextGate()
    {
        NextGateParticles.Play();
    }

    public void previousGate()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        
    }
}
