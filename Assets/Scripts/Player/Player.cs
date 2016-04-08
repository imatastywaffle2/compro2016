using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour
{
    public float playerID;
    public int currentGate = 0;
    GameObject localPlayers;
    GameObject remotePlayers;

    void Start()
    {
        
    }    

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        localPlayers = GameObject.Find("LocalPlayers");
        remotePlayers = GameObject.Find("RemotePlayers");
        if (info.sender.isLocal) {
            transform.SetParent(localPlayers.gameObject.transform);
            PhotonNetwork.player.TagObject = this.gameObject;
            gameObject.layer = 8;
        }
        else
        {
            transform.SetParent(localPlayers.gameObject.transform);
            gameObject.layer = 9;
        }
    }
}