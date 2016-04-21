using UnityEngine;
using System.Collections;
using System;

public class Player : Photon.MonoBehaviour, IPunObservable
{
    public float playerID;
    public int currentGate = 0;
    public int place = 0;
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
            transform.SetParent(remotePlayers.gameObject.transform);
            gameObject.layer = 9;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Serialize(ref currentGate);
        }
        else
        {
            stream.Serialize(ref currentGate);
        }
    }
}