using UnityEngine;
using System.Collections;

public class InputInformation : MonoBehaviour
{
    private bool VehicleSpawned = false;
    private bool VehicleChosen = false;
    public int VehicleType = 0;
    private int VehicleCount = 0;
    public Transform VehiclePrefab;
    

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
        if (VehicleChosen = true && VehicleSpawned != true && VehicleCount == 0)
        {
            VehicleCount++;
            if (VehicleCount == 1)
            {
                VehicleSpawned = true;
                Instantiate(VehiclePrefab);
            }           
        }
    }

    public float Forward()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            return -1;
        }
        return 0;
    }
    public float SideMovement()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return -1;
        }       
        else if (Input.GetKey(KeyCode.D))
        {
            return 1;
        }
        return 0;
    }
    public float RotateShip()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            return -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            return 1;
        }
        return 0;
    }
}