using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour {
    float Speed = 20;
    float BoostTime = 3;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<Vehicle>().Boost(Speed, BoostTime);
        }
    }
	
}
