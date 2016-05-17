using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
    float explosionDuration;

	// Update is called once per frame
	void FixedUpdate () {

        explosionDuration -= Time.deltaTime;
        if (explosionDuration >= 1.2)
        {
            Destroy(gameObject);
        }

    }
}
