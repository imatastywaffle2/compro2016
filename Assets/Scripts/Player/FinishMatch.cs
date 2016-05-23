using UnityEngine;
using System.Collections;

public class FinishMatch : Photon.MonoBehaviour 
{
    public float CheckTime = 4;
    public float ElapsTimer= 0;
    public Player[] players;

    public GameObject Lobby;
    public GameObject PlayerGui;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        ElapsTimer += Time.deltaTime;

        if( ElapsTimer > CheckTime)
        {
            players = gameObject.GetComponentsInChildren<Player>();
            for (int i = 0; i < players.Length; i++)
            {
                if(players[i].place < 1)
                {
                    break;
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
