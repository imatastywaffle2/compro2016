using UnityEngine;
using System.Collections;

public class VehicleSpawning : MonoBehaviour {
    private bool VehicleSpawned = false;
    private bool VehicleChosen = false;
    public Transform VehiclePrefab;
    public int VehicleType = 0;
    public int VehicleCount = 0;

    void Start()
    {
        if (VehicleChosen == false)
        {
            if (Input.GetKeyDown("]"))
            {
                if (VehicleType == 0)
                {
                    VehicleType++;
                }
                else if (VehicleType == 1)
                {
                    VehicleType--;
                }
            }
            if (Input.GetKeyDown("["))
            {
                if (VehicleType == 0)
                {
                    VehicleType--;
                }
                else if (VehicleType == 1)
                {
                    VehicleType++;
                }
            }
        }
        if (Input.GetKeyDown("o"))
        {
            VehicleChosen = true;
        }
        if (Input.GetKeyDown("/"))
        {
            VehicleChosen = false;
        }
        if (VehicleChosen = true && VehicleSpawned != true && VehicleCount == 0 && gameObject.name == "VehicleSpawn")
        {
            VehicleSpawned = true;
            Instantiate(VehiclePrefab);
        }
    }
}
