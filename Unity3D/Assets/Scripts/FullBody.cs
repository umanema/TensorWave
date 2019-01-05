using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullBody
{
    public Vector3 nose;
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

    public FullBody(Vector3 nose,
                    Vector3 lShoulder,
                    Vector3 rShoulder,
                    Vector3 lElbow,
                    Vector3 rElbow,
                    Vector3 lWrist,
                    Vector3 rWrist,
                    Vector3 lHip,
                    Vector3 rHip,
                    Vector3 lKnee,
                    Vector3 rKnee,
                    Vector3 lAnkle,
                    Vector3 rAnkle)
    {
        this.nose = nose;
        this.lShoulder = lShoulder;
        this.rShoulder = rShoulder;
        this.lElbow = lElbow;
        this.rElbow = rElbow;
        this.lWrist = lWrist;
        this.rWrist = rWrist;
        this.lHip = lHip;
        this.rHip = rHip;
        this.lKnee = lKnee;
        this.rKnee = rKnee;
        this.lAnkle = lAnkle;
        this.rAnkle = rAnkle;
    }

    public Vector3 returnCoordinatesByPartName (string partName)
    {
        return (Vector3)this.GetType().GetField(partName).GetValue(this);
    }
}
