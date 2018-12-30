using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OSCReceiver : MonoBehaviour
{
    public OSC osc;
    public string message = "/oscAddress";
    // Start is called before the first frame update
    void Start()
    {
	   osc.SetAddressHandler( message , OnReceive );

    }

    // Update is called once per frame
    void OnReceive(OscMessage message)
    {
        Debug.Log(message.GetFloat(0));
    }
}
