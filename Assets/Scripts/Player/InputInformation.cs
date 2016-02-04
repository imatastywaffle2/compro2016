using UnityEngine;
using System.Collections;

public class InputInformation : MonoBehaviour
{
    private bool VehicleSpawned = false;
    private bool VehicleChosen = false;
    public int VehicleType = 0;
    

    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
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
        if (VehicleChosen = true && VehicleSpawned != true)
        {

        }
    }
}