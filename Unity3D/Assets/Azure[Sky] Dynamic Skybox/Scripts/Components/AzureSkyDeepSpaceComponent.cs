using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyDeepSpaceComponent
    {
        public int moonColorGradientIndex = 0;
        public Gradient[] moonColorGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int moonBrightColorGradientIndex = 0;
        public Gradient[] moonBrightColorGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int moonBrightRangeCurveIndex = 0;
        public AnimationCurve[] moonBrightRangeCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 50.0f, 24.0f, 50.0f),
            AnimationCurve.Linear (-1.0f, 50.0f, 1.0f, 50.0f),
            AnimationCurve.Linear (-1.0f, 50.0f, 1.0f, 50.0f)
        };

        public int moonEmissionCurveIndex = 0;
        public AnimationCurve[] moonEmissionCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 10.0f, 24.0f, 10.0f),
            AnimationCurve.Linear (-1.0f, 10.0f, 1.0f, 10.0f),
            AnimationCurve.Linear (-1.0f, 10.0f, 1.0f, 10.0f)
        };

        public float starsScintillation = 5.0f;

        public int starfieldIntensityCurveIndex = 0;
        public AnimationCurve[] starfieldIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
        };

        public int milkyWayIntensityCurveIndex = 0;
        public AnimationCurve[] milkyWayIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
        };

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			moonColorGradientIndex = fastIndexes;
			moonBrightColorGradientIndex = fastIndexes;
			moonBrightRangeCurveIndex = fastIndexes;
			moonEmissionCurveIndex = fastIndexes;
			starfieldIntensityCurveIndex = fastIndexes;
			milkyWayIntensityCurveIndex = fastIndexes;
		}
    }
}