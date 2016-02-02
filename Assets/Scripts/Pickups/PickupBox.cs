using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour {

    GameObject PickupType;

    // make sure only if tagged player
        void OnTriggerEnter(Collider other)
        {
            Destroy(gameObject);
        }   

    
}
