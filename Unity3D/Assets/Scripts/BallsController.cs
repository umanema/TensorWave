using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsController : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        nose.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 1f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1f) * 0.25f + 0.5f);
        lShoulder.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 2f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2f) * 0.25f + 0.5f);
        rShoulder.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 1.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.75f) * 0.25f + 0.5f);
        lElbow.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 2.25f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.25f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.25f) * 0.25f + 0.5f);
        rElbow.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 0.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 0.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 0.75f) * 0.25f + 0.5f);
        lWrist.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 1.25f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.25f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.25f) * 0.25f + 0.5f);
        rWrist.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 2.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.75f) * 0.25f + 0.5f);
        lHip.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 3f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3f) * 0.25f + 0.5f);
        rHip.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 1.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 1.5f) * 0.25f + 0.5f);
        lKnee.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 2.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 2.5f) * 0.25f + 0.5f);
        rKnee.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 3.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3.5f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3.5f) * 0.25f + 0.5f);
        lAnkle.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 3.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3.75f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 3.75f) * 0.25f + 0.5f);
        rAnkle.transform.localScale = new Vector3(Mathf.Sin(Time.fixedTime * 4f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 4f) * 0.25f + 0.5f, Mathf.Sin(Time.fixedTime * 4f) * 0.25f + 0.5f);
    }
}
