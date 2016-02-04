using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour {

    GameObject PickupType;

    
        void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }   

    
}
