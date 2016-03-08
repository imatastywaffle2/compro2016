using UnityEngine;
using System.Collections;

public class InputInformation : MonoBehaviour
{
    CursorLockMode wantedMode;
    bool invertVert = false;
    bool invertHorizontal = false;

    void Start()
    {
        wantedMode = CursorLockMode.Locked;
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = (CursorLockMode.Locked != wantedMode);
        
        invertVert = PlayerPrefs.GetInt("InvertVert") == 1 ? true : false;
        invertHorizontal = PlayerPrefs.GetInt("InvertHorizontal") == 1 ? true : false;
    }

    // Use this for initialization
    void Update()
    {
        Forward();
        SideMovement();
        RotateShip();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;

    }

    public float Forward()
    {
        if (Input.GetKey(KeyCode.W))
        {
            return 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            return -0.5f;
        }
        else
            return 0;
    }
    public float SideMovement()
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
    public float RotateShip()
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
    public float AxisX()
    {
        if (invertHorizontal)
            return Input.GetAxis("Mouse X");
        else
            return Input.GetAxis("Mouse X"); //Note to self: later maybe allow invert setting
    }
    public float AxisY()
    {
        if(invertVert)
            return -Input.GetAxis("Mouse Y");
        else
            return Input.GetAxis("Mouse Y"); //Get invert setting
    }

}