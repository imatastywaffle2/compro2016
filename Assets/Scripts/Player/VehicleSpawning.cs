using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class VehicleSpawning : Photon.MonoBehaviour, IPunObservable
{
    public int VehicleType = 0;
    public int ReadyCount = 0;
    public bool localReady = false;
    public Transform LocalPlayers;


    void Start()
    {
    }
    void Update()
    {
       
    }
    void FixedUpdate()
    { 

    }

    public void SpawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate("BaseShipPrefab", transform.position, Quaternion.Euler(0, 0, 0), 0);
        
    }

    public void ReadyPlayer()
    {
        SpawnPlayer();
        localReady = true;

        if (ReadyCount >= PhotonNetwork.playerList.Length)
        {

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
