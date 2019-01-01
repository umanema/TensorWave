using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyFogScatteringComponent
    {
		public float fogScale = 0.9f;

        public int fogBlendCurveIndex = 0;
        public AnimationCurve[] fogBlendCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.25f, 24.0f, 0.25f),
            AnimationCurve.Linear (-1.0f, 0.25f, 1.0f, 0.25f),
            AnimationCurve.Linear (-1.0f, 0.25f, 1.0f, 0.25f)
        };

        public int fogDistanceCurveIndex = 0;
        public AnimationCurve[] fogDistanceCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 5000.0f, 24.0f, 5000.0f),
            AnimationCurve.Linear (-1.0f, 5000.0f, 1.0f, 5000.0f),
            AnimationCurve.Linear (-1.0f, 5000.0f, 1.0f, 5000.0f)
        };

        public int heightFogBlendCurveIndex = 0;
        public AnimationCurve[] heightFogBlendCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int heightFogDensityCurveIndex = 0;
        public AnimationCurve[] heightFogDensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.5f, 24.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f)
        };

        public int heightFogDistanceCurveIndex = 0;
        public AnimationCurve[] heightFogDistanceCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 500.0f, 24.0f, 500.0f),
            AnimationCurve.Linear (-1.0f, 500.0f, 1.0f, 500.0f),
            AnimationCurve.Linear (-1.0f, 500.0f, 1.0f, 500.0f)
        };

        public int heightFogStartCurveIndex = 0;
        public AnimationCurve[] heightFogStartCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
            AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
        };

        public int heightFogEndCurveIndex = 0;
        public AnimationCurve[] heightFogEndCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 100.0f, 24.0f, 100.0f),
            AnimationCurve.Linear (-1.0f, 100.0f, 1.0f, 100.0f),
            AnimationCurve.Linear (-1.0f, 100.0f, 1.0f, 100.0f)
        };

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			fogBlendCurveIndex = fastIndexes;
			fogDistanceCurveIndex = fastIndexes;
			heightFogBlendCurveIndex = fastIndexes;
			heightFogDensityCurveIndex = fastIndexes;
			heightFogDistanceCurveIndex = fastIndexes;
			heightFogStartCurveIndex = fastIndexes;
			heightFogEndCurveIndex = fastIndexes;
		}
    }
}