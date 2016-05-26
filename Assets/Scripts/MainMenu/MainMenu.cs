using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
    public InputField Name;

	// Use this for initialization
	void Start () {
	    if(PlayerPrefs.GetString("Username") != null && PlayerPrefs.GetString("Username") != "")
        {
            Name.text = PlayerPrefs.GetString("Username");
        }
	}
}
