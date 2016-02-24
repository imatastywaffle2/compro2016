using UnityEngine;
using System.Collections.Generic;

public class VehicleSpawning : MonoBehaviour {
    private bool VehicleSpawned = false;
    private bool VehicleChosen = false;
    public Transform VehiclePrefab;
    public int VehicleType = 0;
    public int VehicleCount = 0;
    

    public Transform LocalPlayers;


    void Start()
    {
        Vector3 temp = new Vector3(0, 0, 5);
        
      
        VehicleSpawned = true;
        Transform player = (Transform)Instantiate(VehiclePrefab, temp, Quaternion.Euler(0,0,0));
        player.SetParent(LocalPlayers);
        
    }
}
