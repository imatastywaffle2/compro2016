﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VehicleSpawning : MonoBehaviour {
    public Transform VehiclePrefab;
    public int VehicleType = 0;
    public int VehicleCount = 0;
    public List<Transform> players = new List<Transform>();
    public Transform LocalPlayers;


    void Start()
    {
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    { 
    }

    public void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("BaseShipPrefab", transform.position, Quaternion.Euler(0, 0, 0), 0);
        //
       // player.gameObject.layer = 8;
        VehicleCount++;
        //players.Add(player.transform);
       // player.GetComponent<Player>().playerID = Time.time + players.Count;
        
        //note to self change later to when player joins
    }
    
}