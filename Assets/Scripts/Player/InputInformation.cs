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

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = wantedMode = CursorLockMode.None;

    }

    public float Forward()
    {
        if (Input.GetButton("Vertical") == true)
        {
            return 1;
        }
        else if (Input.GetButton("Vertical") == false)
        {  
            return -0.5f;
        }
        else
            return 0;
    }
    public float SideMovement()
    {
        if (Input.GetButton("Horizontal") == true)
        {
            return 1;
        }
        else if (Input.GetButton("Horizontal") == false)
        {
            return -1f;
        }
        else
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
    public bool UsePickup()
    {
        return Input.GetButton("UseItem");
    }

}