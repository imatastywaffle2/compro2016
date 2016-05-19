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
    public GameObject PlayersContainer;

    public Player[] Players;

    public float ReadyTimer = 5;
    public bool MatchStarted = false;
    public bool PlayersEnabled = false;

    public Text StartTimer;


    void Start()
    {
        EventManager.StartListening("FinishMatch", Reset);
    }
    void Update()
    {

    }

    void Reset()
    {
        MatchStarted = false;
        PlayersEnabled = false;
        ReadyTimer = 5;
        PhotonNetwork.room.open = true;
        PhotonNetwork.room.visible = true;
    }


    void FixedUpdate()
    {
        Players = PlayersContainer.GetComponentsInChildren<Player>();

        if (Players != null)
            if (!MatchStarted && Players.Length >= PhotonNetwork.playerList.Length)
            {
                MatchStarted = true;
                PhotonNetwork.room.open = false;
                PhotonNetwork.room.visible = false;
            }
            else if (MatchStarted)
            {
                if (ReadyTimer > 0)
                {
                    StartTimer.enabled = true;
                    StartTimer.text = ((int)ReadyTimer).ToString();
                    ReadyTimer -= Time.deltaTime;
                }
                else if (!PlayersEnabled)
                {
                    PlayersEnabled = true;
                    StartTimer.enabled = false;
                    foreach (Player player in Players)
                    {
                        player.EnablePlayer();
                    }
                }
            }
    }

    public void SpawnPlayer()
    {
        if (Players != null)
            PhotonNetwork.Instantiate("BaseShipPrefab", transform.position + (transform.right * ((Players.Length % 5) * 50)) + (-transform.up * ((Players.Length / 5) * 25)), transform.rotation, 0);
        else
            PhotonNetwork.Instantiate("BaseShipPrefab", transform.position, transform.rotation, 0);

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
            stream.Serialize(ref ReadyTimer);
            stream.Serialize(ref MatchStarted);
        }
        else
        {
            stream.Serialize(ref ReadyTimer);
            stream.Serialize(ref MatchStarted);
        }
    }
}
