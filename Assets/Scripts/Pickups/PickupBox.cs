using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour
{
    public GameObject PickupType;
    public GameObject[] PickupPrefabs;
    public GameObject Mesh;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.gameObject.GetComponent<Vehicle>().ItemPickup)
        {
            Destroy(gameObject.GetComponentInChildren<Transform>());
            other.gameObject.GetComponent<Vehicle>().ItemPickup = PickupType;

        }
    }


}