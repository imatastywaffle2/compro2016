using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lobby : MonoBehaviour {
    public Text playerList;
    public Text playerNumber;
    public VehicleSpawning vehicleSpawn;
    CursorLockMode wantedMode;

    // Use this for initialization
    void Start () {
	
	}

    void Awake()
    {
        wantedMode = CursorLockMode.Confined;
        Cursor.lockState = wantedMode;
        // Hide cursor when locking
        Cursor.visible = true;
    }
	
	// Update is called once per frame
	void Update () {

        playerList.text = "";
	    foreach(PhotonPlayer player in PhotonNetwork.playerList)
        {
            playerList.text += player.name + "\n";
        }

        playerNumber.text = "Ready Players " + vehicleSpawn.Players.Length + "/" + PhotonNetwork.playerList.Length;
	}
}
