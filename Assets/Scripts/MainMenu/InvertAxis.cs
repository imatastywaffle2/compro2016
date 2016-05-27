using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InvertAxis : MonoBehaviour {
    // Use this for initialization
    public Toggle InvertToggle;



    void Start()
    {
        if(!PlayerPrefs.HasKey("InvertVert"))
            PlayerPrefs.SetInt("InvertVert", 1);
        else if(PlayerPrefs.GetInt("InvertVert") == 1)
            InvertToggle.isOn = true;
        else
            InvertToggle.isOn = false;
    }


    public void Invert(bool setting)
    {
        if(setting)
            PlayerPrefs.SetInt("InvertVert", 1);
        else
            PlayerPrefs.SetInt("InvertVert", 0);
    }
}
