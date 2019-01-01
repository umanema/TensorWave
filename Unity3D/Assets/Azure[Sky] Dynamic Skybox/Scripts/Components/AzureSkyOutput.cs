using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class CurveOutput
    {
        public int curveIndex = 0;
        public AnimationCurve[] curveOutput = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.5f, 24.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f)
        };
    }

    [Serializable]
    public class GradientOutput
    {
        public int gradientIndex = 0;
        public Gradient[] gradientOutput = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };
    }
}