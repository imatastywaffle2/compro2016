using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour
{
    GameObject PickupType;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        //public virtual int Next
        //{
        //get
        // {
        // Random random = new Random();
        //  int randomNumber = random.Next(1, 3);
        //  if (random.Next == 3)
        //  {

        //    }

    }

}
