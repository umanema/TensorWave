using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyScatteringComponent
    {
        public Vector3 lambda = new Vector3(680.0f, 550.0f, 440.0f);//Wavelength.
        public float N = 2.545E25f;//Molecular density.
        public float kr = 8.4f;
        public float km = 1.25f;

        public int rayleighCurveIndex = 0;
        public AnimationCurve[] rayleighCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int mieCurveIndex = 0;
        public AnimationCurve[] mieCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.0f, 24.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f),
            AnimationCurve.Linear (-1.0f, 1.0f, 1.0f, 1.0f)
        };

        public int nightIntensityCurveIndex = 0;
        public AnimationCurve[] nightIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.25f, 24.0f, 0.25f),
            AnimationCurve.Linear (-1.0f, 0.25f, 1.0f, 0.25f),
            AnimationCurve.Linear (-1.0f, 0.25f, 1.0f, 0.25f)
        };

        public int rayleighGradientIndex = 0;
        public Gradient[] rayleighGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int mieGradientIndex = 0;
        public Gradient[] mieGradientColor = new Gradient[]
        {
            new Gradient(),
            new Gradient(),
            new Gradient()
        };

        public int sunIntensityCurveIndex = 0;
        public AnimationCurve[] sunIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 0.5f ,24.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f),
            AnimationCurve.Linear (-1.0f, 0.5f, 1.0f, 0.5f)
        };

        public int sunDiskIntensityCurveIndex = 0;
        public AnimationCurve[] sunDiskIntensityCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 3.0f ,24.0f, 3.0f),
            AnimationCurve.Linear (-1.0f, 3.0f, 1.0f, 3.0f),
            AnimationCurve.Linear (-1.0f, 3.0f, 1.0f, 3.0f)
        };

        public int exposureCurveIndex = 0;
        public AnimationCurve[] exposureCurve = new AnimationCurve[]
        {
            AnimationCurve.Linear (0.0f, 1.5f ,24.0f, 1.5f),
            AnimationCurve.Linear (-1.0f, 1.5f, 1.0f, 1.5f),
            AnimationCurve.Linear (-1.0f, 1.5f, 1.0f, 1.5f)
        };

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			rayleighCurveIndex = fastIndexes;
			mieCurveIndex = fastIndexes;
			nightIntensityCurveIndex = fastIndexes;
			rayleighGradientIndex = fastIndexes;
			mieGradientIndex = fastIndexes;
			sunIntensityCurveIndex = fastIndexes;
			sunDiskIntensityCurveIndex = fastIndexes;
			exposureCurveIndex = fastIndexes;
		}
    }
}