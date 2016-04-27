using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class VehicleSpawning : Photon.MonoBehaviour, IPunObservable
{
    public int VehicleType = 0;
    public bool localReady = false;
    public GameObject LocalPlayers;
    public GameObject RemotePlayers;

    public List<Player> Players = new List<Player>();
    
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
        if(!MatchStarted)
        {
            for (int i = 0; i < Players.Count; i++)
            {
                if (!Players[i].Ready)
                    break;
                else if (i + 1 == Players.Count)
                    MatchStarted = true;
            }
        }
        else
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
        GameObject player = PhotonNetwork.Instantiate("BaseShipPrefab", transform.position + (transform.right * (Players.Count * -50)), transform.rotation, 0);
        Players.Add(player.GetComponent<Player>());
    }

    public void ReadyPlayer()
    {
        SpawnPlayer();
        localReady = true;

        ((GameObject)PhotonNetwork.player.TagObject).GetComponent<Player>().Ready = true;
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
            stream.SendNext(Players);
        }
        else
        {
            Players = (List<Player>)stream.ReceiveNext();
        }
    }
}
