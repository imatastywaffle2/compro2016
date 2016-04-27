using UnityEngine;
using System.Collections;


public class RotateAndOrbit : MonoBehaviour
{
    public Transform target;
    public float RotationSpeed = 100f;
    public float OrbitDegrees = 1f;
    void Update()
    {
        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        transform.RotateAround(target.position, Vector3.up, OrbitDegrees);
    }
}