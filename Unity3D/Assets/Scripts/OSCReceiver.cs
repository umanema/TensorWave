using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCReceiver : MonoBehaviour
{
    public OSC osc;
    public string message = "/oscAddress";
    public BodyControl bodyControl;

    // Start is called before the first frame update
    void Start()
    {
	    osc.SetAddressHandler( message , OnReceive );
    }

    // Update is called once per frame
    void OnReceive(OscMessage message)
    {
        bodyControl.fullBody.nose = convertFloatArrayToVector3(message.GetFloatArray(0, 2), bodyControl._nose);
        //bodyControl.fullBody.nose = new Vector3(bodyControl.fullBody.nose.x, bodyControl.fullBody.nose.y, 0.2f);
        bodyControl.fullBody.lShoulder = convertFloatArrayToVector3(message.GetFloatArray(15, 17), bodyControl._lShoulder);
        bodyControl.fullBody.rShoulder = convertFloatArrayToVector3(message.GetFloatArray(18, 20), bodyControl._rShoulder);
        bodyControl.fullBody.lElbow = convertFloatArrayToVector3(message.GetFloatArray(21, 23), bodyControl._lElbow);
        //bodyControl.fullBody.lElbow = new Vector3(bodyControl.fullBody.lElbow.x*1.7f, bodyControl.fullBody.lElbow.y, bodyControl.fullBody.lElbow.z);

        bodyControl.fullBody.rElbow = convertFloatArrayToVector3(message.GetFloatArray(24, 26), bodyControl._rElbow);
        //bodyControl.fullBody.rElbow = new Vector3(bodyControl.fullBody.rElbow.x * 1.7f, bodyControl.fullBody.rElbow.y, bodyControl.fullBody.rElbow.z);

        bodyControl.fullBody.lWrist = convertFloatArrayToVector3(message.GetFloatArray(27, 29), bodyControl._lWrist);
        //print("RW " + bodyControl.fullBody.rWrist);
        //bodyControl.fullBody.lWrist = new Vector3(bodyControl.fullBody.lWrist.x * -1.7f, bodyControl.fullBody.lWrist.y, bodyControl.fullBody.lWrist.z);


        bodyControl.fullBody.rWrist = convertFloatArrayToVector3(message.GetFloatArray(30, 32), bodyControl._rWrist);
        //print("LW " + bodyControl.fullBody.lWrist);
        //bodyControl.fullBody.rWrist = new Vector3(bodyControl.fullBody.rWrist.x * 1.7f, bodyControl.fullBody.rWrist.y, bodyControl.fullBody.rWrist.z);
        
        bodyControl.fullBody.lHip = convertFloatArrayToVector3(message.GetFloatArray(33, 35), bodyControl._lHip);
        bodyControl.fullBody.rHip = convertFloatArrayToVector3(message.GetFloatArray(36, 38), bodyControl._rHip);
        bodyControl.fullBody.lKnee = convertFloatArrayToVector3(message.GetFloatArray(39, 41), bodyControl._lKnee);
        bodyControl.fullBody.rKnee = convertFloatArrayToVector3(message.GetFloatArray(42, 44), bodyControl._rKnee);
        bodyControl.fullBody.lAnkle = convertFloatArrayToVector3(message.GetFloatArray(45, 47), bodyControl._lAnkle);
        bodyControl.fullBody.rAnkle = convertFloatArrayToVector3(message.GetFloatArray(48, 50), bodyControl._rAnkle);
        

    }

    Vector3 convertFloatArrayToVector3 (float[] array, Vector3 defaultValues)
    {
        Vector3 result = new Vector3 (0,0,0);
        if (array.Length == 0)
        {
            Debug.Log("Array length should be equal '3'. Now it's 1. Missing values will be replaced by defaultValues");
            result = new Vector3(defaultValues.x, defaultValues.y, defaultValues.z);
        }
        if (array.Length == 1)
        {
            Debug.Log("Array length should be equal '3'. Now it's 1. Missing values will be replaced by defaultValues");
            result = new Vector3(array[0], defaultValues.y, defaultValues.z);
        } else if (array.Length == 2)
        {
            Debug.Log("Array length should be equal '3'. Now it's 2. Missing values will be replaced by defaultValues");
            result = new Vector3(array[0], array[1], defaultValues.z);
        } else if (array.Length == 3)
        {
            result = new Vector3(array[0], array[1], array[2]);
        } else if (array.Length > 3)
        {
            Debug.Log("Array length should be equal '3'. Now it's larger, than 3. Array will be cut");
            result = new Vector3(array[0], array[1], array[2]);
        }

        //invert y axis
        result = new Vector3(result.x, result.y, result.z);
        return result;
    }
}
