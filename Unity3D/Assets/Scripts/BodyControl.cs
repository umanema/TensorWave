﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BodyControl : MonoBehaviour
{

    //for calibration
    public Slider ScaleXSlider;
    public Slider ScaleYSlider;
    public Slider OffsetXSlider;
    public Slider OffsetYSlider;

    Vector2 scaleXY = new Vector2(1, 1);
    Vector2 offsetXY = new Vector2(0, 0);

    [Header("Model Body Parts")]
    public GameObject nose;
    public GameObject lShoulder;
    public GameObject rShoulder;
    public GameObject lElbow;
    public GameObject rElbow;
    public GameObject lWrist;
    public GameObject rWrist;
    public GameObject lHip;
    public GameObject rHip;
    public GameObject lKnee;
    public GameObject rKnee;
    public GameObject lAnkle;
    public GameObject rAnkle;

    [Header("OSCReceiver")]
    public OSCReceiver oSCReceiver;

    [Header("Joints")]
    public GameObject skeletonHead;
    public GameObject skeletonLShoulder;
    public GameObject skeletonRShoulder;
    public GameObject skeletonLElbow;
    public GameObject skeletonRElbow;
    public GameObject skeletonLWrist;
    public GameObject skeletonRWrist;
    public GameObject skeletonLHip;
    public GameObject skeletonRHip;
    public GameObject skeletonLKnee;
    public GameObject skeletonRKnee;
    public GameObject skeletonLAnkle;
    public GameObject skeletonRAnkle;

    [HideInInspector]
    public Vector3 _nose;
    [HideInInspector]
    public Vector3 _lShoulder;
    [HideInInspector]
    public Vector3 _rShoulder;
    [HideInInspector]
    public Vector3 _lElbow;
    [HideInInspector]
    public Vector3 _rElbow;
    [HideInInspector]
    public Vector3 _lWrist;
    [HideInInspector]
    public Vector3 _rWrist;
    [HideInInspector]
    public Vector3 _lHip;
    [HideInInspector]
    public Vector3 _rHip;
    [HideInInspector]
    public Vector3 _lKnee;
    [HideInInspector]
    public Vector3 _rKnee;
    [HideInInspector]
    public Vector3 _lAnkle;
    [HideInInspector]
    public Vector3 _rAnkle;

    [HideInInspector]
    public FullBody fullBody;

    SoftJointLimit limit = new SoftJointLimit();


    void Start()
    {
        _nose = skeletonHead.transform.position;
        _lShoulder = skeletonLShoulder.transform.position;
        _rShoulder = skeletonRShoulder.transform.position;
        _lElbow = skeletonLElbow.transform.position;
        _rElbow = skeletonRElbow.transform.position;
        _lWrist = skeletonLWrist.transform.position;
        _rWrist = skeletonRWrist.transform.position;
        _lHip = skeletonLHip.transform.position;
        _rHip = skeletonRHip.transform.position;
        _lKnee = skeletonLKnee.transform.position;
        _rKnee = skeletonRKnee.transform.position;
        _lAnkle = skeletonLAnkle.transform.position;
        _rAnkle = skeletonRAnkle.transform.position;

        fullBody = new FullBody(_nose, _lShoulder, _rShoulder, _lElbow, _rElbow, _lWrist, _rWrist, _lHip, _rHip, _lKnee, _rKnee, _lAnkle, _rAnkle);
    }


    void Update()
    {
        scaleXY = new Vector2(ScaleXSlider.value, ScaleYSlider.value);
        offsetXY = new Vector2(OffsetXSlider.value, OffsetYSlider.value);

        
        MoveJoint(nose, nameof(nose), skeletonHead);
        MoveJoint(lShoulder, nameof(lShoulder), skeletonLShoulder);
        MoveJoint(rShoulder, nameof(rShoulder), skeletonRShoulder);
        MoveJoint(lElbow, nameof(lElbow), skeletonLElbow);
        MoveJoint(rElbow, nameof(rElbow), skeletonRElbow);
        MoveJoint(lWrist, nameof(lWrist), skeletonLWrist);
        MoveJoint(rWrist, nameof(rWrist), skeletonRWrist);
        MoveJoint(lHip, nameof(lHip), skeletonLHip);
        MoveJoint(rHip, nameof(rHip), skeletonRHip);
        MoveJoint(lKnee, nameof(lKnee), skeletonLKnee);
        MoveJoint(rKnee, nameof(rKnee), skeletonRKnee);
        MoveJoint(lAnkle, nameof(lAnkle), skeletonLAnkle);
        MoveJoint(rAnkle, nameof(rAnkle), skeletonRAnkle);
        
        
    }

    void MoveJoint(GameObject part, string partName, GameObject skeleletonBone)
    {

        limit.limit = fullBody.returnCoordinatesByPartName(partName).z*0.2f;
        //limit.limit = 1;
        part.GetComponent<ConfigurableJoint>().linearLimit = limit;
        if (fullBody.returnCoordinatesByPartName(partName).z < 0.2)
        {
            
            skeleletonBone.transform.DOLocalMove((Vector3)this.GetType().GetField("_"+partName).GetValue(this), 1f);
        } else
        {
            /*
            skeleletonBone.transform.localPosition = new Vector3(fullBody.returnCoordinatesByPartName(partName).x * scaleXY.x - offsetXY.x,
                                                             fullBody.returnCoordinatesByPartName(partName).y * scaleXY.y + offsetXY.y,
                                                             0);
            */
            /*
            skeleletonBone.transform.DOLocalMove(new Vector3(fullBody.returnCoordinatesByPartName(partName).x * scaleXY.x - offsetXY.x,
                                                             fullBody.returnCoordinatesByPartName(partName).y * scaleXY.y + offsetXY.y,
                                                             0), 0.3f);
            */


            skeleletonBone.transform.localPosition = Vector3.Lerp((Vector3)this.GetType().GetField("_" + partName).GetValue(this),new Vector3(fullBody.returnCoordinatesByPartName(partName).x * scaleXY.x - offsetXY.x,
                                                             fullBody.returnCoordinatesByPartName(partName).y * scaleXY.y + offsetXY.y,
                                                             0), fullBody.returnCoordinatesByPartName(partName).z);
        }
        

    }
}
