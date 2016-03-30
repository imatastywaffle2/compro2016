using UnityEngine;
using System.Collections;

public class PickupBox : MonoBehaviour
{
    public GameObject PickupType;
    public GameObject[] PickupPrefabs;


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !other.gameObject.GetComponent<Vehicle>().ItemPickup)
        {
            Destroy(gameObject.GetComponentInChildren<Transform>());
            other.gameObject.GetComponent<Vehicle>().ItemPickup = PickupType;
        }
    }
    public void GetPickup()
    {
        int num = Random.Range(1, 4);
        if (num == 1)
        {
            PickupType = PickupPrefabs[1];
        }
        if (num == 2)
        {
            PickupType = PickupPrefabs[2];
        }
        if (num == 3)
        {
            PickupType = PickupPrefabs[3];
        }
    }
    void Start()
    {
        GetPickup();
    }


}