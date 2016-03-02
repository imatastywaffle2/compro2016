using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GatePassScript : MonoBehaviour
{
    List<Player> PlayersPassed = new List< Player>();

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
            if (!PlayersPassed.Contains(col.gameObject.GetComponent<Player>()))
                PlayersPassed.Add(col.gameObject.GetComponent<Player>());

          //  count = count + 1;
           // SetCountText();

        }

        //void SetCountText ()
            {
         //   countText.text = "Count: " + count.ToString();
        }

    }

}
