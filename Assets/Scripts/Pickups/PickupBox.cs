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
            
        }   

}
