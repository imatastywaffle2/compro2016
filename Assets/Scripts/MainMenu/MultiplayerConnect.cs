using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MultiplayerConnect : Photon.MonoBehaviour
{
    public Button playButton;

    public bool AutoConnect = true;

    public byte Version = 1;

    private bool ConnectInUpdate = true;

    public GameObject Menu;

    public VehicleSpawning vehicleSpawn;
     
    public virtual void Start()
    {
        PhotonNetwork.autoJoinLobby = false;
    }

    public virtual void Update()
    {
        if (ConnectInUpdate && AutoConnect && !PhotonNetwork.connected)
        {
            Debug.Log("Update() was called by unity. Scene is loaded. Let's connect to the Photon Master Server. Calling: PhotonNetwork.ConnectUsingSettings(); ");

            ConnectInUpdate = false;
            PhotonNetwork.ConnectUsingSettings(Version + "." + SceneManagerHelper.ActiveSceneBuildIndex);

           
        }

    }

    public virtual void OnConnectedToMaster()
    {
        playButton.interactable = true;
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public virtual void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one. Calling: PhotonNetwork.CreateRoom( null, new RoomOptions() {maxPlayers = 8}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions() { maxPlayers = 8 }, null);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() called by PUN. Now this client is in a room");
        Menu.SetActive(false);
        vehicleSpawn.SpawnPlayer();

    }

    public void ChangeToScene(string sceneToChangeTo)
    {
        SceneManager.LoadScene(sceneToChangeTo);
    }
}

       
