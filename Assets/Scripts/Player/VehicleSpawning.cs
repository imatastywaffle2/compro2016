using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class VehicleSpawning : Photon.MonoBehaviour, IPunObservable
{
    public int VehicleType = 0;
    public int ReadyCount = 0;
    public bool localReady = false;
    public GameObject LocalPlayers;
    public GameObject RemotePlayers;

    public float ReadyTimer = 5;
    public bool MatchStarted = false;
    public bool PlayersEnabled = false;

    public Text StartTimer;


    void Start()
    {
    }
    void Update()
    {
       
    }
    void FixedUpdate()
    {
        if (MatchStarted)
        {
            if(ReadyTimer > 0)
            {
                StartTimer.enabled = true;
                StartTimer.text = ((int)ReadyTimer).ToString();
                ReadyTimer -= Time.deltaTime;
            }
            else if(!PlayersEnabled)
            {
                PlayersEnabled = true;
                StartTimer.enabled = false;
                EnablePlayers();
            }
        }
    }

    public void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("BaseShipPrefab", transform.position + new Vector3(ReadyCount * -150, 0, 0), transform.rotation, 0);
        
    }

    public void ReadyPlayer()
    {
        SpawnPlayer();
        localReady = true;

        ReadyCount++;

        if (ReadyCount >= PhotonNetwork.playerList.Length)
        {
            MatchStarted = true;
        }


    }
    
    public void EnablePlayers()
    {
        InputInformation[] inputs = LocalPlayers.GetComponentsInChildren<InputInformation>(true);
        Move[] moves = LocalPlayers.GetComponentsInChildren<Move>(true);

        Move[] remoteMoves = RemotePlayers.GetComponentsInChildren<Move>(true);

        for (int i = 0; i < remoteMoves.Length; i++)
        {
            remoteMoves[i].enabled = true;
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i].enabled = true;
            moves[i].enabled = true;
        }

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.Serialize(ref ReadyCount);
        }
        else
        {
            stream.Serialize(ref ReadyCount);
        }
    }
}
