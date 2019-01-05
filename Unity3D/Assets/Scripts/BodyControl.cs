using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyControl : MonoBehaviour
{
    public GameObject nose;
    public GameObject lElbow;
    public GameObject lWrist;
    //public GameObject LShin;
    //public GameObject RShin;
    public OSCReceiver oSCReceiver;

    public Vector3 _nose;
    public Vector3 lShoulder;
    public Vector3 rShoulder;
    public Vector3 _lElbow;
    public Vector3 rElbow;
    public Vector3 _lWrist;
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
        fullBody = new FullBody(_nose, lShoulder, rShoulder, _lElbow, rElbow, _lWrist, rWrist, lHip, rHip, lKnee, rKnee, lAnkle, rAnkle);
    }

    // Update is called once per frame
    void Update()
    {
        MoveJoint(nose, nameof(nose));
        MoveJoint(lWrist, nameof(lWrist));
    }

    void MoveJoint(GameObject part, string partName)
    {
        limit.limit = fullBody.returnCoordinatesByPartName(partName).z;
        part.GetComponent<ConfigurableJoint>().linearLimit = limit;
        part.GetComponent<Rigidbody>().MovePosition(new Vector3(fullBody.returnCoordinatesByPartName(partName).x, fullBody.returnCoordinatesByPartName(partName).y, 0));
        Debug.Log(partName);
        Debug.Log(new Vector3(fullBody.returnCoordinatesByPartName(partName).x, fullBody.returnCoordinatesByPartName(partName).y, 0));

    }
}
