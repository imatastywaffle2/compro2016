using UnityEngine;
using System.Collections;

public class VehicleSpawning : MonoBehaviour {
    private bool VehicleSpawned = false;
    private bool VehicleChosen = false;
    public Transform VehiclePrefab;
    public int VehicleType = 0;
    public int VehicleCount = 0;

    public Transform LocalPlayers;


    void Start()
    {
      
        VehicleSpawned = true;
        Transform player = (Transform)Instantiate(VehiclePrefab);
        player.SetParent(LocalPlayers);
        
    }
}
