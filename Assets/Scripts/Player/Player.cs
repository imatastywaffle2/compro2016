using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Player : Photon.MonoBehaviour, IPunObservable
{
    public string playerName;
    public int playerID;
    public int currentGate = 0;
    public int currentLap = 1;
    public int maxLaps = 3;
    public int place = 0;
    GameObject localPlayers;
    GameObject remotePlayers;

    public float SplitTimer;
    public List<float> Splits = new List<float>();

    public bool Ready = false;



    void Start()
    {
        
    }    

    void Update()
    {
        SplitTimer += Time.deltaTime;
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

        playerID = info.sender.ID;
        playerName = info.sender.name;
    }

    public void SetLap()
    {
        photonView.RPC("LapChange", PhotonTargets.All);
    }

    [PunRPC]
    private void LapChange()
    {
        if (Splits.Count == 0 || SplitTimer - Splits[currentLap - 1] > 10)
        {
            currentGate = 0;
            currentLap++;
            Splits.Add(SplitTimer);
        }
    }

    public void EnablePlayer()
    {
        if (photonView.isMine)
            gameObject.GetComponent<InputInformation>().enabled = true;
        gameObject.GetComponent<Move>().enabled = true;
        Splits.Add(SplitTimer);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Serialize(ref currentGate);
                stream.Serialize(ref currentLap);
            stream.Serialize(ref Ready);
        }
        else
        {
            stream.Serialize(ref currentGate);
            stream.Serialize(ref currentLap);
            stream.Serialize(ref Ready);
        }
    }
}