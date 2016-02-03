using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
    public GameObject Prefab;

    // Spawn Delay in seconds
    public float interval = 30;
    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("SpawnNext", interval, interval);
    }
	void SpawnNext()
    {
        Instantiate(Prefab, transform.position, Quaternion.identity);
    }
	
}
