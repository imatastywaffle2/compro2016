using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour
{
   public GameObject PickupType;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }





    }

        //public int Next
        //{
        //get
        // {
        // Random random = new Random();
        //  int randomNumber = random.Next(1, 3);
        //  if (random.Next == 3)
        //  {

        //    }
}
