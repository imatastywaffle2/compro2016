using UnityEngine;
using System.Collections;

public class GatePassScript : MonoBehaviour
{
    public Gate gate;

    private int count;
   // public GUIText countText;

	void Start ()
    {
        // count = 0;
        //  SetCountText ();
        
	}

    void Update ()
    {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Player player = col.gameObject.GetComponent<Player>();
            if (!gate.PlayersPassed.Contains(player) && gate.GateNumber == player.currentGate + 1){
                player.currentGate++;
                gate.PlayersPassed.Add(player);

            }
          //  count = count + 1;
           // SetCountText();

        }

        //void SetCountText ()
            {
         //   countText.text = "Count: " + count.ToString();
        }

    }

}
