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
        return Input.GetAxis("Vertical");
    }
    public float SideMovement()
    {
        return Input.GetAxis("Horizontal");
    }
    public float Rotate()
    {
        return Input.GetAxis("Roll");
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