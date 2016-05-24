using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FinishMatch : Photon.MonoBehaviour 
{
    public float CheckTime = 4;
    public float ElapsTimer= 0;
    public Player[] players;

    public GameObject Lobby;
    public GameObject PlayerGui;
    public Text PlaceList;


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        ElapsTimer += Time.deltaTime;

        if( ElapsTimer > CheckTime)
        {
            players = gameObject.GetComponentsInChildren<Player>();
            Array.Sort(players,
               delegate (Player x, Player y) {
                   if (x.currentLap != y.currentLap)
                       return x.currentLap.CompareTo(y.currentLap);
                   else
                       return x.currentGate.CompareTo(y.currentGate);

               });
            PlaceList.text = "";
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].place < 1)
                {
                    string mod;
                    switch (i)
                    {
                        case 1:
                            mod = "st";
                            break;
                        case 2:
                            mod = "nd";
                            break;
                        case 3:
                            mod = "rd";
                            break;
                        default:
                            mod = "th";
                            break;
                    }
                    PlaceList.text += players[i].playerName + " - " + (i+1) + mod + "\n";
                }
                else if (i == players.Length -1)
                {
                    photonView.RPC("FinishGame", PhotonTargets.All);
                }
            }
       


        }


	}

    [PunRPC]
    public void FinishGame()
    {
        EventManager.TriggerEvent("FinishMatch");
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
            

        }
        Lobby.SetActive(true);
        PlayerGui.SetActive(false);

    }
}
