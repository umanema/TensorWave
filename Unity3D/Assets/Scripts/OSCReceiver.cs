using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCReceiver : MonoBehaviour
{
    public OSC osc;
    public string message = "/oscAddress";

    Vector3 wrist;
    
    // Start is called before the first frame update
    void Start()
    {
	    osc.SetAddressHandler( message , OnReceive );
    }

    // Update is called once per frame
    void OnReceive(OscMessage message)
    {
        wrist = convertFloatArrayToVector3(message.GetFloatArray(0, 2));
    }

    Vector3 convertFloatArrayToVector3 (float[] array)
    {
        Vector3 result = new Vector3 (0,0,0);
        if (array.Length == 1)
        {
            Debug.Log("Array length should be equal '3'. Now it's 1. Missing values will be replaced by zeros");
            result = new Vector3(array[0], 0, 0);
        } else if (array.Length == 2)
        {
            Debug.Log("Array length should be equal '3'. Now it's 2. Missing values will be replaced by zeros");
            result = new Vector3(array[0], array[1], 0);
        } else if (array.Length == 3)
        {
            return new Vector3(array[0], array[1], array[2]);
        } else if (array.Length > 3)
        {
            Debug.Log("Array length should be equal '3'. Now it's larger, than 3. Array will be cut");
            result = new Vector3(array[0], array[1], array[2]);
        }
        return result;
    }
}
