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
            if (gate.GateNumber == player.currentGate + 1)
                {
                player.currentGate++;
                gate.PlayersPassed.Add(player);
                if (col.gameObject.layer == 8)
                        gate.previousGate(player.currentGate);
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
