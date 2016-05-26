using UnityEngine;
using System.Collections;

public class EscapeMenu : MonoBehaviour {
    public GameObject leaveButton;
    CursorLockMode wantedMode;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            
            leaveButton.SetActive(!leaveButton.activeInHierarchy);
            if (leaveButton.activeInHierarchy)
            {
                wantedMode = CursorLockMode.Confined;
                Cursor.lockState = wantedMode;
                // Hide cursor when locking
                Cursor.visible = true;
            }
            else
            {
                wantedMode = CursorLockMode.Locked;
                Cursor.lockState = wantedMode;
                // Hide cursor when locking
                Cursor.visible = false;
            }
        }
	}
}
