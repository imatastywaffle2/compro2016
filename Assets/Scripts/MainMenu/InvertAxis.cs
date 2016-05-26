using UnityEngine;
using System.Collections;

public class InvertAxis : MonoBehaviour {
	// Use this for initialization
    public void Invert(bool setting)
    {
        if(setting)
            PlayerPrefs.SetInt("InvertVert", 0);
        else
            PlayerPrefs.SetInt("InvertVert", 1);
    }
}
