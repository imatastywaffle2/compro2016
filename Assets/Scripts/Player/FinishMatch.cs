using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class FinishMatch : Photon.MonoBehaviour 
{
    public float CheckTime = 4;
    public float ElapsTimer= 0;
    public Player[] players;
    public List<int> finishedPlayers = new List<int>();

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
                       return -x.currentLap.CompareTo(y.currentLap);
                   else
                       return -x.currentGate.CompareTo(y.currentGate);

               });
            PlaceList.text = "";
            int numberFinished = 0;
            for (int i = 0; i < players.Length; i++)
            {
                int place = i + 1;

                for (int f = 0; f < finishedPlayers.Count; f++)
                {
                    int playerFinished = finishedPlayers.IndexOf(players[i].playerID);
                    if (playerFinished >= 0)
                        place = playerFinished + 1;
                    else
                        place = i + finishedPlayers.Count;
                }

                string mod;

                switch (place)
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

                    

                PlaceList.text += players[i].playerName + " - " + place + mod + "\n";

                if (players[i].place >= 1)
                {
                    numberFinished++;
                    if (finishedPlayers.Count < players[i].place)
                    {
                        finishedPlayers.Add(players[i].playerID);
                    }
                }

                if (numberFinished >= players.Length)
                {
                    photonView.RPC("FinishGame", PhotonTargets.All);
                }
                  
                
            }
       


        }


	}

    [PunRPC]
    public void FinishGame()
    {
        finishedPlayers.Clear();
        EventManager.TriggerEvent("FinishMatch");
        for (int i = 0; i < players.Length; i++)
        {
            Destroy(players[i].gameObject);
            

        }
        Lobby.SetActive(true);
        PlayerGui.SetActive(false);

    }
}
