﻿using UnityEngine;
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

    public void previousGate(int nextGate)
    {
        NextGateParticles.Stop();
        if (nextGate < gateManager.Gates.Length)
            gateManager.Gates[nextGate].nextGate();
    }

    void OnTriggerEnter(Collider col)
    {
        
    }
}
