using UnityEngine;
using System.Collections;

public class Player : Photon.MonoBehaviour
{
    public float playerID;
    public int currentGate = 0;
    GameObject localPlayers = GameObject.Find("LocalPlayers");

    void Start()
    {
        
    }    

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.sender.isLocal) {
            transform.SetParent(localPlayers.gameObject.transform);
            gameObject.layer = 8;
        }
    }
}