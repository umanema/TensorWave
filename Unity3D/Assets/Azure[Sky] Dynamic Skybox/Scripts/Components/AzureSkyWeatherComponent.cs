using System;

namespace UnityEngine.AzureSky
{
	[Serializable]
	public class AzureSkyWeatherComponent
	{
		public int weatherRainGradientColorIndex = 0;
		public Gradient[] weatherRainGradientColor = new Gradient[]
		{
			new Gradient(),
			new Gradient(),
			new Gradient()
		};

		public int weatherSnowGradientColorIndex = 0;
		public Gradient[] weatherSnowGradientColor = new Gradient[]
		{
			new Gradient(),
			new Gradient(),
			new Gradient()
		};

		public int weatherRainIntensityCurveIndex = 0;
		public AnimationCurve[] weatherRainIntensityCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherSnowIntensityCurveIndex = 0;
		public AnimationCurve[] weatherSnowIntensityCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherWindSpeedCurveIndex = 0;
		public AnimationCurve[] weatherWindSpeedCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherWindDirectionCurveIndex = 0;
		public AnimationCurve[] weatherWindDirectionCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		//Sounds Volume.
		public float weatherRainLightVolume = 0.0f;
		public float weatherRainMediumVolume = 0.0f;
		public float weatherRainHeavyVolume = 0.0f;
		public float weatherWindLightVolume = 0.0f;
		public float weatherWindMediumVolume = 0.0f;
		public float weatherWindHeavyVolume = 0.0f;

		//Third-Party Compatibility.
		public int weatherWetnessCurveIndex = 0;
		public AnimationCurve[] weatherWetnessCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherSnowAmountCurveIndex = 0;
		public AnimationCurve[] weatherSnowAmountCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherCovarageCurveIndex = 0;
		public AnimationCurve[] weatherCovarageCurve = new AnimationCurve[]
		{
			AnimationCurve.Linear (0.0f, 0.0f, 24.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f),
			AnimationCurve.Linear (-1.0f, 0.0f, 1.0f, 0.0f)
		};

		public int weatherOutputColor1Index = 0;
		public Gradient[] weatherOutputColor1GradientColor = new Gradient[]
		{
			new Gradient(),
			new Gradient(),
			new Gradient()
		};

		public int weatherOutputColor2Index = 0;
		public Gradient[] weatherOutputColor2GradientColor = new Gradient[]
		{
			new Gradient(),
			new Gradient(),
			new Gradient()
		};

		public int weatherOutputColor3Index = 0;
		public Gradient[] weatherOutputColor3GradientColor = new Gradient[]
		{
			new Gradient(),
			new Gradient(),
			new Gradient()
		};

		public int fastIndexes = 0;
		public void FastIndexesChange()
		{
			weatherRainGradientColorIndex = fastIndexes;
			weatherSnowGradientColorIndex = fastIndexes;
			weatherRainIntensityCurveIndex = fastIndexes;
			weatherSnowIntensityCurveIndex = fastIndexes;
			weatherWindSpeedCurveIndex = fastIndexes;
			weatherWindDirectionCurveIndex = fastIndexes;
			weatherWetnessCurveIndex = fastIndexes;
			weatherSnowAmountCurveIndex = fastIndexes;
			weatherCovarageCurveIndex = fastIndexes;
			weatherOutputColor1Index = fastIndexes;
			weatherOutputColor2Index = fastIndexes;
			weatherOutputColor3Index = fastIndexes;
		}
	}
}