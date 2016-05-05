using UnityEngine;
using System.Collections;
using System;

public class Move : Photon.MonoBehaviour, IPunObservable
{
    public GameObject Vehicle;
    bool inMotion; //Is this object in motion
    float horizontalspeed; //How fast you can move side to side
    float acceleration;
    float rotateSpeed = 1.2f;
    float turnSpeed = 1.5f;
    //float tiltAngle = 5; maybe use later
    private Vector3 mousePosition;
    float invertOrNot;
    float velocityZerotoOne;
    public ParticleSystem[] engines;
    Vehicle Vehicles;
    public float stunDuration;
    InputInformation InputInfo;

    private Vector3 latestCorrectPos;
    private Vector3 onUpdatePos;
    private float fraction;

    private float enginesOn = 0;


    // Use this for initialization
    void Start ()
    {
        this.latestCorrectPos = transform.position;
        this.onUpdatePos = transform.position;

        Vehicles = GetComponent<Vehicle>();
        InputInfo = GetComponent<InputInformation>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 speed = Vector3.zero;

        if (this.photonView.isMine && !Vehicles.Stunned)
        {
            acceleration = Vehicles.fowardAccel;
            horizontalspeed = Vehicles.horizontalSpeed;
            stunDuration = Vehicles.vehicleStun;

            GetComponent<Rigidbody>().AddForce(transform.forward * (acceleration + Vehicles.boostSpeed) * InputInfo.Forward());
            GetComponent<Rigidbody>().AddForce(transform.right * horizontalspeed * InputInfo.SideMovement());
            transform.Rotate(Vector3.forward * InputInfo.Rotate() * rotateSpeed);
            transform.Rotate(Vector3.right * turnSpeed * InputInfo.AxisY());
            transform.Rotate(Vector3.up * turnSpeed * InputInfo.AxisX());
            enginesOn = InputInfo.Forward();
            

        }
        else
        {
            this.fraction = this.fraction + Time.deltaTime * 9;
            transform.localPosition = Vector3.Lerp(this.onUpdatePos, this.latestCorrectPos, this.fraction); // set our pos between A and B
            speed = latestCorrectPos - onUpdatePos;
        }

        foreach (ParticleSystem engine in engines)
        {
            if (enginesOn > 0)
                engine.Play();
            else
                engine.Stop();
        }
    }



    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref enginesOn);
        }
        else
        {
            // Receive latest state information
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;

            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
            stream.Serialize(ref enginesOn);

            this.latestCorrectPos = pos;                // save this to move towards it in FixedUpdate()
            this.onUpdatePos = transform.localPosition; // we interpolate from here to latestCorrectPos
            this.fraction = 0;                          // reset the fraction we alreay moved. see Update()

            transform.localRotation = rot;              // this sample doesn't smooth rotation
        }
    }
}