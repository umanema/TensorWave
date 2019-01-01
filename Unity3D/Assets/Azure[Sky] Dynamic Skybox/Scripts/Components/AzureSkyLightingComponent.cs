using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyLightingComponent
    {
        public int directionalLightIntensityCurveIndex = 0;
        public AnimationCurve[] directionalLightIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int directionalLightGradientColorIndex = 0;
        public Gradient[] directionalLightGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int ambientIntensityCurveIndex = 0;
        public AnimationCurve[] ambientIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int ambientSkyGradientColorIndex = 0;
        public Gradient[] ambientSkyGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int equatorGradientColorIndex = 0;
        public Gradient[] equatorGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int groundGradientColorIndex = 0;
        public Gradient[] groundGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int reflectionIntensityCurveIndex = 0;
        public AnimationCurve[] reflectionIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			directionalLightIntensityCurveIndex = fastIndexes;
			directionalLightGradientColorIndex = fastIndexes;
			ambientIntensityCurveIndex = fastIndexes;
			ambientSkyGradientColorIndex = fastIndexes;
			equatorGradientColorIndex = fastIndexes;
			groundGradientColorIndex = fastIndexes;
			reflectionIntensityCurveIndex = fastIndexes;
		}
    }
}