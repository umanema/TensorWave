using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyCloudsComponent
    {
        public float dynamicCloudLayer1Altitude = 10.0f;
        public float dynamicCloudLayer1Direction = 1.0f;
        public float dynamicCloudLayer1Speed = 0.1f;

        public int dynamicCloudLayer1GradientColor1Index = 0;
        public Gradient[] dynamicCloudLayer1GradientColor1 = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int dynamicCloudLayer1GradientColor2Index = 0;
        public Gradient[] dynamicCloudLayer1GradientColor2 = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int dynamicCloudLayer1DensityCurveIndex = 0;
        public AnimationCurve[] dynamicCloudLayer1DensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.7f, 24.0f, 0.7f),
            AnimationCurve.Linear (-1.0f, 0.7f, 1.0f, 0.7f),
            AnimationCurve.Linear (-1.0f, 0.7f, 1.0f, 0.7f)
        };

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			dynamicCloudLayer1GradientColor1Index = fastIndexes;
			dynamicCloudLayer1GradientColor2Index = fastIndexes;
			dynamicCloudLayer1DensityCurveIndex = fastIndexes;
            staticCloudColorIndex = fastIndexes;
            staticCloudScatteringCurveIndex = fastIndexes;
            staticCloudExtinctionCurveIndex = fastIndexes;
            staticCloudPowerCurveIndex = fastIndexes;
            staticCloudIntensityCurveIndex = fastIndexes;
        }

        public Texture2D staticCloudTexture;
        public int staticCloudColorIndex = 0;
        public Gradient[] staticCloudColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int staticCloudScatteringCurveIndex = 0;
        public AnimationCurve[] staticCloudScatteringCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int staticCloudExtinctionCurveIndex = 0;
        public AnimationCurve[] staticCloudExtinctionCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear(0.0f, 0.25f, 24.0f, 0.25f),
            AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f),
            AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f)
        };

        public int staticCloudPowerCurveIndex = 0;
        public AnimationCurve[] staticCloudPowerCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear(0.0f, 2.2f, 24.0f, 2.2f),
            AnimationCurve.Linear(-1.0f, 2.2f, 1.0f, 2.2f),
            AnimationCurve.Linear(-1.0f, 2.2f, 1.0f, 2.2f)
        };

        public int staticCloudIntensityCurveIndex = 0;
        public AnimationCurve[] staticCloudIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public float staticCloudRotationSpeed = 0.0f;
    }
}