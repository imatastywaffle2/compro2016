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

        if (other.gameObject.tag == "Player")
        {
            other.transform.position = respawnPosition.position;
        }
    }
}