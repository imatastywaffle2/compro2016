using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public GameObject[] Prefab;
    public GameObject SpawnedPickup;


    // Spawn Delay in seconds
    public float interval = 15;
    // Use this for initialization
    void Start ()
    {
        //if something to make it not spawn if one is already there
        InvokeRepeating("SpawnNext", interval, interval);
    }
	void SpawnNext()
    {
        int randomNumber = Random.Range(0, 3);
        if(SpawnedPickup == null)
            SpawnedPickup = (GameObject)Instantiate(Prefab[randomNumber], transform.position, Quaternion.identity);
    }
    void Update()
    {

      
    }
	
}
