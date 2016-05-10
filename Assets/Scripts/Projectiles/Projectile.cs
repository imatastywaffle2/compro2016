using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour
{

    public bool trackTarget;
    public float stunDuration = 2;

    public int shooterId;

    // Speed
    public float speed = 700;

    // Target
    public Transform target;
    Pickup Pickups;
    private float fraction;
    private Vector3 onUpdatePos;
    private Vector3 latestCorrectPos;

    void Start()
    {
        this.latestCorrectPos = transform.position;
        this.onUpdatePos = transform.position;

    }

    void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        shooterId = info.sender.ID;
    }

    void FixedUpdate()
    {
        if (trackTarget)
        {
            if (target)
            {
                transform.LookAt(target.position);
                // Fly towards the target        
                GetComponent<Rigidbody>().velocity = transform.forward * speed;
            }
        }

        //if (!this.photonView.isMine)
        //{
        //    this.fraction = this.fraction + Time.deltaTime * 9;
        //    transform.localPosition = Vector3.Lerp(this.onUpdatePos, this.latestCorrectPos, this.fraction); // set our pos between A and B

        //}
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Player>().playerID != shooterId)
        {
            other.gameObject.GetComponent<Vehicle>().Stun();
           
            //photonView.RPC("HitTarget", PhotonTargets.All);

            Destroy(gameObject);

        }
    }
    void OnTriggerEnter(Collider otherShip)
    {
        //make a code for detecting a ship that isnt yourself.
        if(otherShip.gameObject.tag == "Player") {
            Debug.Log("Projectile player id " + otherShip.gameObject.GetComponent<Player>().playerID + " " + shooterId);
            if (target == null && otherShip.gameObject.tag == "Player" && otherShip.gameObject.GetComponent<Player>().playerID != shooterId)
            {
                target = otherShip.transform;
            
            }
        }
    } 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.isWriting)
        //{
        //    Vector3 pos = transform.localPosition;
        //    Quaternion rot = transform.localRotation;
        //    stream.Serialize(ref pos);
        //    stream.Serialize(ref rot);
        //}
        //else
        //{
        //    // Receive latest state information
        //    Vector3 pos = Vector3.zero;
        //    Quaternion rot = Quaternion.identity;

        //    stream.Serialize(ref pos);
        //    stream.Serialize(ref rot);

        //    this.latestCorrectPos = pos;                // save this to move towards it in FixedUpdate()
        //    this.onUpdatePos = transform.localPosition; // we interpolate from here to latestCorrectPos
        //    this.fraction = 0;                          // reset the fraction we alreay moved. see Update()

        //    transform.localRotation = rot;              // this sample doesn't smooth rotation
        //}
    }
}
