using UnityEngine;
using System.Collections;

public class respawn : MonoBehaviour
{
    public Transform respawnPosition;




    void Start()
    {
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Top")
        {
                 GetComponent<Rigidbody>().isKinematic = true;
            bool falling = false;
        }

        if (other.gameObject.tag == "Platform")
        {
            transform.position = respawnPosition.position;
        }
    }
}