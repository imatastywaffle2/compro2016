using UnityEngine;
using System.Collections;

public class CameraPicker : MonoBehaviour {
    public string ActiveCam;

	// Use this for initialization
	void Start () {
        setMainCamera(ActiveCam);
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private void setMainCamera(string cameraName)
    {
        foreach (Camera c in GameObject.FindObjectsOfType(typeof(Camera)))
        {
            if (c.name == cameraName) c.depth = -1;
            else c.depth = -2;
        }
    }
}
