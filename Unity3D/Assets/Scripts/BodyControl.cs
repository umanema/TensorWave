using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControl : MonoBehaviour
{
    public GameObject Head;
    //public GameObject LShin;
    //public GameObject RShin;
    public OSCReceiver oSCReceiver;

    public Vector3 nose = new Vector3(0, 3, 0.2f);
    public Vector3 lShoulder;
    public Vector3 rShoulder;
    public Vector3 lElbow;
    public Vector3 rElbow;
    public Vector3 lWrist;
    public Vector3 rWrist;
    public Vector3 lHip;
    public Vector3 rHip;
    public Vector3 lKnee;
    public Vector3 rKnee;
    public Vector3 lAnkle;
    public Vector3 rAnkle;

    [HideInInspector]
    public FullBody fullBody;

    SoftJointLimit limit = new SoftJointLimit();
    // Start is called before the first frame update
    void Start()
    {
        fullBody = new FullBody(nose, lShoulder, rShoulder, lElbow, rElbow, lWrist, rWrist, lHip, rHip, lKnee, rKnee, lAnkle, rAnkle);
    }

    // Update is called once per frame
    void Update()
    {
        limit.limit = nose.z;
        Head.GetComponent<ConfigurableJoint>().linearLimit = limit;
        Head.GetComponent<Rigidbody>().MovePosition(new Vector3(fullBody.nose.x, fullBody.nose.y, 0));

        //LShin.GetComponent<Rigidbody>().MovePosition(new Vector3(1, 0, 0));
        //RShin.GetComponent<Rigidbody>().MovePosition(new Vector3(-1, 0, 0));
    }
}
