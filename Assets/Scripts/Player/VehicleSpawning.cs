using UnityEngine;
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
        SpawnPlayer();
    }
    void Update()
    {
        
    }
    void FixedUpdate()
    {     
        
            
    }

    public void SpawnPlayer()
    {
        Transform player = (Transform)Instantiate(VehiclePrefab, transform.position, Quaternion.Euler(0, 0, 0));
        VehicleCount++;
        players.Add(player);
        player.GetComponent<Player>().playerID = Time.time + players.Count;
        player.SetParent(LocalPlayers);
        //note to self change later to when player joins
    }
    
}
