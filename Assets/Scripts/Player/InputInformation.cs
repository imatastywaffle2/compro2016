using UnityEngine;
using System.Collections;

public class InputInformation : MonoBehaviour
{


    // Use this for initialization
    void Update()
    {
        Forward();
        SideMovement();
        RotateShip();
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
        else
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