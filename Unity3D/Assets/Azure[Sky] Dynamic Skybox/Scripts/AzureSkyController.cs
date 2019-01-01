using System.Collections.Generic;
using System;

namespace UnityEngine.AzureSky
{
    [ExecuteInEditMode]
    [AddComponentMenu("Azure[Sky]/Sky Controller")]
    public class AzureSkyController : MonoBehaviour
    {
        #if UNITY_EDITOR
        public AzureEditorSettings editorSettings;
        #endif

        //References.
        public Transform sunTransform;
        public Transform moonTransform;
        public Transform lightTransform;
        public Texture2D moonTexture;
        public Texture2D sunTexture;
        public Texture2D cloudNoise;
        public Cubemap starfieldTexture;
        public Cubemap starNoiseTexture;
		public Material skyMaterial;
		public Material rainMaterial;
		public Material snowMaterial;
		public ParticleSystem rainParticle;
		public ParticleSystem snowParticle;
		public WindZone windZone;
		public AzureSkySoundFX soundFX;
		public Transform skydome;
        public ReflectionProbe reflectionProbe;

        //Internal uses.
        private DateTime m_date;
        private int m_dayOfYear = 1;
        private AzureSkyProfile m_getCalendarProfile;
        private Matrix4x4 m_starfieldMatrix;
        private Matrix4x4 m_noiseMatrix;
        private float m_noiseMatrixRotation = 0.0f;
        private Quaternion m_noiseRotation;
        private float m_timeSinceLastProbeUpdate;
        private float m_timelineCurveTime = 0.0f;
        private float m_timelineGradientTime = 0.0f;
        private float m_sunCurveTime = 0.0f;
        private float m_sunGradientTime = 0.0f;
        private float m_moonCurveTime = 0.0f;
        private float m_moonGradientTime = 0.0f;

        //Time of Day.
        private float m_timeProgression = 0.0f;
        private Vector3 m_sunRealisticPosition = Vector3.one;
        private Vector3 m_moonRealisticPosition = Vector3.one;
        private Quaternion m_sunSimplePosition;
        private Vector3 m_sunLocalDirection;
        private Vector3 m_moonLocalDirection;

        //Change Weather.
        public int weatherNumber = 0;
        public bool isBlendingWeathers = false;
        public float weatherTransitionTime = 0.0f;
        private float m_startTime = 0.0f;
        private float m_transitionTime = 30;
		//Thunder AudioClips.
		public List<AudioClip> thunderAudioClipList = new List<AudioClip>();

        //Profiles.
        public AzureSkyProfile[] calendarProfileList = new AzureSkyProfile[366];
        public List<AzureSkyProfile> standardProfileList = new List<AzureSkyProfile>();
        public List<WeatherProfile> weatherProfileList = new List<WeatherProfile>();

        public  AzureSkyProfile calendarDayProfile;
        private AzureSkyProfile nextCalendarDayProfile;
        public AzureSkyProfile currentDayProfile;

		public AzureSkyProfile sourceWeatherProfile;
        private AzureSkyProfile m_targetWeatherProfile;
        private WeightedDayProfile[] m_weightedProfiles;

        //Components.
        public AzureSkyTimeOfDayComponent timeOfDay = new AzureSkyTimeOfDayComponent();
        public AzureSkyOptionsComponent options = new AzureSkyOptionsComponent();

        //Scattering Properties of the Standard Sky System.
        public float kr = 8.4f;
        public float km = 1.25f;
        public float rayleigh = 1.0f;
        public float mie = 1.0f;
        public float nightIntensity = 2.5f;
        public float sunIntensity = 0.25f;
        public float sunDiskIntensity = 3.0f;
        public float exposure = 1.0f;
        public Color rayleighColor = Color.white;
        public Color mieColor = Color.white;

        private Vector3 m_lambda = new Vector3(680.0f, 550.0f, 440.0f);//Wavelength.
        public float N = 2.545f;//Molecular density. 2.545E25f
        private const float m_pi = Mathf.PI;//Pi number value.
        private const float m_n = 1.00029f;//Refractive index of air.
        private const float m_pn = 0.035f;//Depolatization factor for standard air.
        private Vector3 m_K = new Vector3(686.0f, 678.0f, 666.0f);

		//Deep Space Properties.
        public Color moonColor = Color.white;
        public Color moonBrightColor = Color.white;
        public float moonBrightRange = 50.0f;
        public float moonEmission = 0.5f;
        public float starsScintillation = 5.0f;
        public float starfieldIntensity = 0.0f;
        public float milkyWayIntensity = 0.0f;

        //Fog Scattering Properties of the Standard Sky System.
        public float fogScale = 1.0f;
        public float fogBlend = 0.25f;
        public float fogDistance = 5000.0f;
        public float heightFogBlend = 1.0f;
        public float heightFogDensity = 0.5f;
        public float heightFogDistance = 500.0f;
        public float heightFogStart = 0.0f;
        public float heightFogEnd = 100.0f;

		//Clouds Properties.
        public float dynamicCloudLayer1Altitude = 10.0f;
        public float dynamicCloudLayer1Direction = 1.0f;
        public float dynamicCloudLayer1Speed = 0.1f;
        public Color dynamicCloudLayer1Color1 = Color.white;
        public Color dynamicCloudLayer1Color2 = Color.white;
        public float dynamicCloudLayer1Density = 0.7f;
        private Texture2D staticCloudTextureSource, staticCloudTextureDestination;
        public Color staticCloudColor = Color.white;
        public float staticCloudScattering = 1.0f;
        public float staticCloudExtinction = 0.25f;
        public float staticCloudPower = 2.2f;
        public float staticCloudIntensity = 1.0f;
        public float staticCloudRotationSpeed = 0.0f;
        private float m_cloudRotationSpeed = 0.0f;

        //Lighting Properties.
        public float lightIntensity = 1.0f;
        public Color lightColor = Color.black;
        public float ambientIntensity = 1.0f;
        public Color ambientSkyColor = Color.black;
        public Color ambientEquatorColor = Color.black;
        public Color ambientGroundColor = Color.black;
        public float reflectionIntensity = 1.0f;
        private Light m_lightComponent;
        private float m_sunElevation = 0.0f;

        //Weather Properties.
        public Color rainColor = Color.white;
		public Color snowColor = Color.white;
		public float rainIntensity = 0.0f;
		public float snowIntensity = 0.0f;
		public float windSpeed = 0.0f;
		public float windDirection = 0.0f;
		public float rainLightVolume = 0.0f;
		public float rainMediumVolume = 0.0f;
		public float rainHeavyVolume = 0.0f;
		public float windLightVolume = 0.0f;
		public float windMediumVolume = 0.0f;
		public float windHeavyVolume = 0.0f;
		public float wetness = 0.0f;
		public float snowAmount = 0.0f;
		public float covarage = 0.0f;
		public Color outputColor1 = Color.white;
		public Color outputColor2 = Color.white;
		public Color outputColor3 = Color.white;
		private ParticleSystem.EmissionModule m_rainEmission;
		private ParticleSystem.EmissionModule m_snowEmission;
		private float m_maxRainEmission = 2000.0f;
		private float m_maxSnowEmission = 750.0f;
		private Vector3 m_windRotation = Vector3.zero;
		private float m_maxWindSpeed = 10.0f;

        //Output Lists.
        public List<CurveOutput> curveOuputList = new List<CurveOutput>();
        public List<GradientOutput> gradientOuputList = new List<GradientOutput>();

        //Shader Uniforms.
        public static class Uniforms
        {
			internal static readonly int _CullMode = Shader.PropertyToID("_Azure_CullMode");
            internal static readonly int _SunTexture = Shader.PropertyToID("_Azure_SunTexture");
            internal static readonly int _MoonTexture = Shader.PropertyToID("_Azure_MoonTexture");
            internal static readonly int _CloudNoise= Shader.PropertyToID("_Azure_CloudNoise");
            internal static readonly int _StarfieldTexture = Shader.PropertyToID("_Azure_StarfieldTexture");
            internal static readonly int _StarNoiseTexture = Shader.PropertyToID("_Azure_StarNoiseTexture");
            internal static readonly int _Kr = Shader.PropertyToID("_Azure_Kr");
            internal static readonly int _Km = Shader.PropertyToID("_Azure_Km");
            internal static readonly int _NightIntensity = Shader.PropertyToID("_Azure_NightIntensity");
            internal static readonly int _RayleighColor = Shader.PropertyToID("_Azure_RayleighColor");
            internal static readonly int _MieColor = Shader.PropertyToID("_Azure_MieColor");
            internal static readonly int _SunIntensity = Shader.PropertyToID("_Azure_SunIntensity");
            internal static readonly int _SunDiskIntensity = Shader.PropertyToID("_Azure_SunDiskIntensity");
            internal static readonly int _Exposure = Shader.PropertyToID("_Azure_Exposure");
            internal static readonly int _Pi316 = Shader.PropertyToID("_Azure_Pi316");
            internal static readonly int _Pi14 = Shader.PropertyToID("_Azure_Pi14");
            internal static readonly int _Pi = Shader.PropertyToID("_Azure_Pi");
            internal static readonly int _LightSpeed = Shader.PropertyToID("_Azure_LightSpeed");
            internal static readonly int _MieDepth = Shader.PropertyToID("_Azure_MieDepth");
            internal static readonly int _SunSize = Shader.PropertyToID("_Azure_SunSize");
            internal static readonly int _MoonSize = Shader.PropertyToID("_Azure_MoonSize");
            internal static readonly int _SunDirection = Shader.PropertyToID("_Azure_SunDirection");
            internal static readonly int _MoonDirection = Shader.PropertyToID("_Azure_MoonDirection");
            internal static readonly int _UpMatrix = Shader.PropertyToID("_Azure_UpMatrix");
            internal static readonly int _MoonMatrix = Shader.PropertyToID("_Azure_MoonMatrix");
            internal static readonly int _SunMatrix = Shader.PropertyToID("_Azure_SunMatrix");
            internal static readonly int _RelativeSunMatrix = Shader.PropertyToID("_Azure_RelativeSunMatrix");
            internal static readonly int _StarMatrix = Shader.PropertyToID("_Azure_StarMatrix");
            internal static readonly int _NoiseMatrix = Shader.PropertyToID("_Azure_NoiseMatrix");
            internal static readonly int _Br = Shader.PropertyToID("_Azure_Br");
            internal static readonly int _Bm = Shader.PropertyToID("_Azure_Bm");
            internal static readonly int _MieG = Shader.PropertyToID("_Azure_MieG");
            internal static readonly int _SunsetColorMode = Shader.PropertyToID("_Azure_SunsetColorMode");
            
            internal static readonly int _MoonColor = Shader.PropertyToID("_Azure_MoonColor");
            internal static readonly int _MoonBrightColor = Shader.PropertyToID("_Azure_MoonBrightColor");
            internal static readonly int _MoonBrightRange = Shader.PropertyToID("_Azure_MoonBrightRange");
            internal static readonly int _MoonEmission = Shader.PropertyToID("_Azure_MoonEmission");
            internal static readonly int _StarfieldColorBalance = Shader.PropertyToID("_Azure_StarfieldColorBalance");
            internal static readonly int _StarfieldIntensity = Shader.PropertyToID("_Azure_StarfieldIntensity");
            internal static readonly int _MilkyWayIntensity = Shader.PropertyToID("_Azure_MilkyWayIntensity");
            
			internal static readonly int _FogScale = Shader.PropertyToID("_Azure_FogScale");
            internal static readonly int _FogBlend = Shader.PropertyToID("_Azure_FogBlend");
            internal static readonly int _FogDistance = Shader.PropertyToID("_Azure_FogDistance");
            internal static readonly int _HeightFogBlend = Shader.PropertyToID("_Azure_HeightFogBlend");
            internal static readonly int _HeightFogDensity = Shader.PropertyToID("_Azure_HeightFogDensity");
            internal static readonly int _HeightFogDistance = Shader.PropertyToID("_Azure_HeightFogDistance");
            internal static readonly int _HeightFogStart = Shader.PropertyToID("_Azure_HeightFogStart");
            internal static readonly int _HeightFogEnd = Shader.PropertyToID("_Azure_HeightFogEnd");

            internal static readonly int _DynamicCloudLayer1Altitude = Shader.PropertyToID("_Azure_DynamicCloudLayer1Altitude");
            internal static readonly int _DynamicCloudLayer1Direction = Shader.PropertyToID("_Azure_DynamicCloudLayer1Direction");
            internal static readonly int _DynamicCloudLayer1Speed = Shader.PropertyToID("_Azure_DynamicCloudLayer1Speed");
            internal static readonly int _DynamicCloudLayer1Density = Shader.PropertyToID("_Azure_DynamicCloudLayer1Density");
            internal static readonly int _DynamicCloudLayer1Color1 = Shader.PropertyToID("_Azure_DynamicCloudLayer1Color1");
            internal static readonly int _DynamicCloudLayer1Color2 = Shader.PropertyToID("_Azure_DynamicCloudLayer1Color2");
            internal static readonly int _StaticCloudTextureSource = Shader.PropertyToID("_Azure_StaticCloudTextureSource");
            internal static readonly int _StaticCloudTextureDestination = Shader.PropertyToID("_Azure_StaticCloudTextureDestination");
            internal static readonly int _StaticCloudColor = Shader.PropertyToID("_Azure_StaticCloudColor");
            internal static readonly int _StaticCloudScattering = Shader.PropertyToID("_Azure_StaticCloudScattering");
            internal static readonly int _StaticCloudExtinction = Shader.PropertyToID("_Azure_StaticCloudExtinction");
            internal static readonly int _StaticCloudPower = Shader.PropertyToID("_Azure_StaticCloudPower");
            internal static readonly int _StaticCloudIntensity = Shader.PropertyToID("_Azure_StaticCloudIntensity");
            internal static readonly int _StaticCloudRotationSpeed = Shader.PropertyToID("_Azure_StaticCloudRotationSpeed");
            internal static readonly int _WeatherTransitionTime = Shader.PropertyToID("_Azure_WeatherTransitionTime");

            internal static readonly int _RainColor = Shader.PropertyToID("_TintColor");
			internal static readonly int _SnowColor = Shader.PropertyToID("_TintColor");
        }

        // Read only.
        private bool m_isDaytime = false;
        public bool IsDaytime
        {
            get
            {
                return m_isDaytime;
            }
        }

        //Start.
        void Start ()
        {
            weatherNumber = 0;
			m_rainEmission = rainParticle.emission;
			m_snowEmission = snowParticle.emission;

            // Get components.
            m_lightComponent = lightTransform.GetComponent<Light>();

            //First Shaders Update.
            ConfigureShaders ();
            InitializeShaderUniforms ();
            UpdateShaderUniforms ();

			//Enable or Disable Particles.
			SetParticlesActive (options.particlesMode);

            //Initialize Profiles.
			UpdateProfiles ();

            //Set System Date and Time.(?)
            if (options.startAtCurrentDate) { timeOfDay.ApplySystemDate (); }
            if (options.startAtCurrentTime) { timeOfDay.ApplySystemTime (); }

            //Get the Progression Speed of the 24-hour Day Cycle.
            m_timeProgression = timeOfDay.GetDayLength ();

            //First Update of the Reflection Probe.
            if (options.useReflectionProbe == 1 && options.reflectionProbeRefreshMode == 2)
            {
                if (options.reflectionProbeUpdateAtFirstFrame)
                {
                    reflectionProbe.RenderProbe ();
                }
            }
        }

        //Reset the Calendar.
        void Reset ()
        {
            timeOfDay.UpdateCalendar (timeOfDay.selectableDayList);
        }

        //Update.
        void Update ()
        {
            // Update shader uniforms.
            UpdateShaderUniforms ();
            #if UNITY_EDITOR
            InitializeShaderUniforms ();
            #endif

			// Follow Main Camera.
			if (options.followMainCamera)
			{
				transform.position = Camera.main.transform.position;
			}

            // Get Curves and Gradients time to evaluate.
            m_timelineGradientTime = timeOfDay.hour / 24.0f;
            m_timelineCurveTime = timeOfDay.hour;
            if (timeOfDay.setTimeByCurve)
            {
                timeOfDay.CalculateTimeByCurve ();
                m_timelineGradientTime = timeOfDay.hourByCurve / 24.0f;
                m_timelineCurveTime = timeOfDay.hourByCurve;
            }
            m_sunCurveTime = Vector3.Dot(-sunTransform.transform.forward, transform.up);
            m_sunGradientTime = Mathf.InverseLerp(-1, 1, m_sunCurveTime);
            m_moonCurveTime = Vector3.Dot(-moonTransform.transform.forward, transform.up);
            m_moonGradientTime = Mathf.InverseLerp(-1, 1, m_moonCurveTime);

            // Update Properties.
			if (!isBlendingWeathers)
            {
                N = currentDayProfile.scattering.N;
                kr = currentDayProfile.scattering.kr;
                km = currentDayProfile.scattering.km;
                m_lambda = currentDayProfile.scattering.lambda;
                rayleigh = GetCurveValue (currentDayProfile.scattering.rayleighCurve, currentDayProfile.scattering.rayleighCurveIndex);
                mie = GetCurveValue (currentDayProfile.scattering.mieCurve, currentDayProfile.scattering.mieCurveIndex);
                nightIntensity = GetCurveValue (currentDayProfile.scattering.nightIntensityCurve, currentDayProfile.scattering.nightIntensityCurveIndex);
                sunIntensity = GetCurveValue (currentDayProfile.scattering.sunIntensityCurve, currentDayProfile.scattering.sunIntensityCurveIndex);
                sunDiskIntensity = GetCurveValue (currentDayProfile.scattering.sunDiskIntensityCurve, currentDayProfile.scattering.sunDiskIntensityCurveIndex);
                exposure = GetCurveValue (currentDayProfile.scattering.exposureCurve, currentDayProfile.scattering.exposureCurveIndex);
                rayleighColor = GetGradientValue (currentDayProfile.scattering.rayleighGradientColor, currentDayProfile.scattering.rayleighGradientIndex);
                mieColor = GetGradientValue (currentDayProfile.scattering.mieGradientColor, currentDayProfile.scattering.mieGradientIndex);

                moonColor = GetGradientValue (currentDayProfile.deepSpace.moonColorGradientColor, currentDayProfile.deepSpace.moonColorGradientIndex);
                moonBrightColor = GetGradientValue (currentDayProfile.deepSpace.moonBrightColorGradientColor, currentDayProfile.deepSpace.moonBrightColorGradientIndex);
                moonBrightRange = GetCurveValue (currentDayProfile.deepSpace.moonBrightRangeCurve, currentDayProfile.deepSpace.moonBrightRangeCurveIndex);
                moonEmission = GetCurveValue (currentDayProfile.deepSpace.moonEmissionCurve, currentDayProfile.deepSpace.moonEmissionCurveIndex);
                starfieldIntensity = GetCurveValue (currentDayProfile.deepSpace.starfieldIntensityCurve, currentDayProfile.deepSpace.starfieldIntensityCurveIndex);
                milkyWayIntensity = GetCurveValue (currentDayProfile.deepSpace.milkyWayIntensityCurve, currentDayProfile.deepSpace.milkyWayIntensityCurveIndex);
                starsScintillation = currentDayProfile.deepSpace.starsScintillation;

				fogScale = currentDayProfile.fogScattering.fogScale;
                fogBlend = GetCurveValue (currentDayProfile.fogScattering.fogBlendCurve, currentDayProfile.fogScattering.fogBlendCurveIndex);
                fogDistance = GetCurveValue (currentDayProfile.fogScattering.fogDistanceCurve, currentDayProfile.fogScattering.fogDistanceCurveIndex);
                heightFogBlend = GetCurveValue (currentDayProfile.fogScattering.heightFogBlendCurve, currentDayProfile.fogScattering.heightFogBlendCurveIndex);
                heightFogDensity = GetCurveValue (currentDayProfile.fogScattering.heightFogDensityCurve, currentDayProfile.fogScattering.heightFogDensityCurveIndex);
                heightFogDistance = GetCurveValue (currentDayProfile.fogScattering.heightFogDistanceCurve, currentDayProfile.fogScattering.heightFogDistanceCurveIndex);
                heightFogStart = GetCurveValue (currentDayProfile.fogScattering.heightFogStartCurve, currentDayProfile.fogScattering.heightFogStartCurveIndex);
                heightFogEnd = GetCurveValue (currentDayProfile.fogScattering.heightFogEndCurve, currentDayProfile.fogScattering.heightFogEndCurveIndex);

				switch (options.cloudMode)
				{
                    case 1:
					    dynamicCloudLayer1Altitude = currentDayProfile.clouds.dynamicCloudLayer1Altitude;
					    dynamicCloudLayer1Direction = currentDayProfile.clouds.dynamicCloudLayer1Direction;
					    dynamicCloudLayer1Speed = currentDayProfile.clouds.dynamicCloudLayer1Speed;
					    dynamicCloudLayer1Density = GetCurveValue (currentDayProfile.clouds.dynamicCloudLayer1DensityCurve, currentDayProfile.clouds.dynamicCloudLayer1DensityCurveIndex);
					    dynamicCloudLayer1Color1 = GetGradientValue (currentDayProfile.clouds.dynamicCloudLayer1GradientColor1, currentDayProfile.clouds.dynamicCloudLayer1GradientColor1Index);
					    dynamicCloudLayer1Color2 = GetGradientValue (currentDayProfile.clouds.dynamicCloudLayer1GradientColor2, currentDayProfile.clouds.dynamicCloudLayer1GradientColor2Index);
                        break;
                    case 2:
                        //staticCloudTextureSource = currentDayProfile.clouds.staticCloudTexture;
                        //staticCloudTextureDestination = currentDayProfile.clouds.staticCloudTexture;
                        staticCloudColor = GetGradientValue(currentDayProfile.clouds.staticCloudColor, currentDayProfile.clouds.staticCloudColorIndex);
                        staticCloudScattering = GetCurveValue(currentDayProfile.clouds.staticCloudScatteringCurve, currentDayProfile.clouds.staticCloudScatteringCurveIndex);
                        staticCloudExtinction = GetCurveValue(currentDayProfile.clouds.staticCloudExtinctionCurve, currentDayProfile.clouds.staticCloudExtinctionCurveIndex);
                        staticCloudPower = GetCurveValue(currentDayProfile.clouds.staticCloudPowerCurve, currentDayProfile.clouds.staticCloudPowerCurveIndex);
                        staticCloudIntensity = GetCurveValue(currentDayProfile.clouds.staticCloudIntensityCurve, currentDayProfile.clouds.staticCloudIntensityCurveIndex);
                        staticCloudRotationSpeed = currentDayProfile.clouds.staticCloudRotationSpeed;
                        break;
				}

                lightIntensity = GetCurveValue (currentDayProfile.lighting.directionalLightIntensityCurve, currentDayProfile.lighting.directionalLightIntensityCurveIndex);
                lightColor = GetGradientValue (currentDayProfile.lighting.directionalLightGradientColor, currentDayProfile.lighting.directionalLightGradientColorIndex);
                ambientIntensity = GetCurveValue (currentDayProfile.lighting.ambientIntensityCurve, currentDayProfile.lighting.ambientIntensityCurveIndex);
                ambientSkyColor = GetGradientValue (currentDayProfile.lighting.ambientSkyGradientColor, currentDayProfile.lighting.ambientSkyGradientColorIndex);
                ambientEquatorColor = GetGradientValue (currentDayProfile.lighting.equatorGradientColor, currentDayProfile.lighting.equatorGradientColorIndex);
                ambientGroundColor = GetGradientValue (currentDayProfile.lighting.groundGradientColor, currentDayProfile.lighting.groundGradientColorIndex);
                reflectionIntensity = GetCurveValue (currentDayProfile.lighting.reflectionIntensityCurve, currentDayProfile.lighting.reflectionIntensityCurveIndex);

				if (options.particlesMode == 1 || options.keepWeatherUpdate)
				{
					rainColor = GetGradientValue (currentDayProfile.weather.weatherRainGradientColor, currentDayProfile.weather.weatherRainGradientColorIndex);
					snowColor = GetGradientValue (currentDayProfile.weather.weatherSnowGradientColor, currentDayProfile.weather.weatherSnowGradientColorIndex);
					rainIntensity = GetCurveValue (currentDayProfile.weather.weatherRainIntensityCurve, currentDayProfile.weather.weatherRainIntensityCurveIndex);
					snowIntensity = GetCurveValue (currentDayProfile.weather.weatherSnowIntensityCurve, currentDayProfile.weather.weatherSnowIntensityCurveIndex);
					windSpeed = GetCurveValue (currentDayProfile.weather.weatherWindSpeedCurve, currentDayProfile.weather.weatherWindSpeedCurveIndex);
					windDirection = GetCurveValue (currentDayProfile.weather.weatherWindDirectionCurve, currentDayProfile.weather.weatherWindDirectionCurveIndex);
					rainLightVolume = currentDayProfile.weather.weatherRainLightVolume;
					rainMediumVolume = currentDayProfile.weather.weatherRainMediumVolume;
					rainHeavyVolume = currentDayProfile.weather.weatherRainHeavyVolume;
					windLightVolume = currentDayProfile.weather.weatherWindLightVolume;
					windMediumVolume = currentDayProfile.weather.weatherWindMediumVolume;
					windHeavyVolume = currentDayProfile.weather.weatherWindHeavyVolume;
				}

				if (options.keepWeatherUpdate)
				{
					wetness = GetCurveValue (currentDayProfile.weather.weatherWetnessCurve, currentDayProfile.weather.weatherWetnessCurveIndex);
					snowAmount = GetCurveValue (currentDayProfile.weather.weatherSnowAmountCurve, currentDayProfile.weather.weatherSnowAmountCurveIndex);
					covarage = GetCurveValue (currentDayProfile.weather.weatherCovarageCurve, currentDayProfile.weather.weatherCovarageCurveIndex);
					outputColor1 = GetGradientValue (currentDayProfile.weather.weatherOutputColor1GradientColor, currentDayProfile.weather.weatherOutputColor1Index);
					outputColor2 = GetGradientValue (currentDayProfile.weather.weatherOutputColor2GradientColor, currentDayProfile.weather.weatherOutputColor2Index);
					outputColor3 = GetGradientValue (currentDayProfile.weather.weatherOutputColor3GradientColor, currentDayProfile.weather.weatherOutputColor3Index);
				}
            }

            if (Application.isPlaying && staticCloudRotationSpeed != 0.0f)
            {
                m_cloudRotationSpeed += staticCloudRotationSpeed * Time.deltaTime;
                if (m_cloudRotationSpeed >= 1.0f)
                {
                    m_cloudRotationSpeed -= 1.0f;
                }
            }

            // Directions.
            m_sunLocalDirection = transform.InverseTransformDirection(sunTransform.transform.forward);
            m_moonLocalDirection = transform.InverseTransformDirection(moonTransform.transform.forward);

            //Only in Gameplay.
            if (Application.isPlaying)
            {
                //Set Time of Day.
                timeOfDay.hour += m_timeProgression * Time.deltaTime;
                if (timeOfDay.hour >= 24)
                {
                    timeOfDay.StartNextDay (options.repeatMode);
					UpdateProfiles ();
                }

                //Blend Weathers.
				if (isBlendingWeathers)
				{
					BlendingWeathers ();
				}

                //Rain and Wind Sound Effects Controller.
                SoundPlayController(rainLightVolume, soundFX.rainLight);
                SoundPlayController(rainMediumVolume, soundFX.rainMedium);
                SoundPlayController(rainHeavyVolume, soundFX.rainHeavy);
                SoundPlayController(windLightVolume, soundFX.windLight);
                SoundPlayController(windMediumVolume, soundFX.windMedium);
                SoundPlayController(windHeavyVolume, soundFX.windHeavy);
            }

            //Set the Position of the Sun and Moon in the Sky.
            switch (timeOfDay.timeMode)
            {
                //Simple Time Mode.
                case 0:
                    //Set the Rotation of Directional Lights.
                    m_sunSimplePosition = Quaternion.Euler(0.0f, timeOfDay.longitude, timeOfDay.latitude);
                    m_sunSimplePosition *= Quaternion.Euler(timeOfDay.SetSimpleSunPosition(), 180.0f, 0.0f);
                    sunTransform.transform.localRotation = m_sunSimplePosition;
                    moonTransform.transform.rotation = timeOfDay.SetSimpleMoonPosition(sunTransform);

                    //Fix Skydome Local Rotation.
                    skydome.transform.localRotation = Quaternion.Inverse(transform.rotation);

                    //Update Shader Matrix.
                    Shader.SetGlobalMatrix(Uniforms._UpMatrix, transform.worldToLocalMatrix);
                    Shader.SetGlobalMatrix(Uniforms._SunMatrix, sunTransform.transform.worldToLocalMatrix);
                    Shader.SetGlobalMatrix(Uniforms._RelativeSunMatrix, sunTransform.transform.localToWorldMatrix);
                    m_starfieldMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(options.starfieldPosition), Vector3.one);
                    Shader.SetGlobalMatrix(Uniforms._StarMatrix, m_starfieldMatrix.inverse);
                    break;

                //Realistic Time Mode.
                case 1:
                    //Set the Rotation of Directional Lights.
                    m_sunRealisticPosition = timeOfDay.SetRealisticSunPosition();
                    sunTransform.transform.forward = transform.TransformDirection(m_sunRealisticPosition);
                    m_moonRealisticPosition = timeOfDay.SetRealisticMoonPosition();
                    moonTransform.transform.forward = transform.TransformDirection(m_moonRealisticPosition);

                    //Fix Skydome Local Rotation.
                    skydome.transform.localRotation = Quaternion.Inverse(transform.rotation);

                    //Update Shader Matrix.
                    Shader.SetGlobalMatrix(Uniforms._UpMatrix, transform.worldToLocalMatrix);
                    Shader.SetGlobalMatrix(Uniforms._SunMatrix, sunTransform.transform.worldToLocalMatrix);
                    Shader.SetGlobalMatrix(Uniforms._RelativeSunMatrix, Matrix4x4.identity);
                    Quaternion skyRotation = Quaternion.Euler(90.0f - timeOfDay.latitude, 0.0f, 0.0f) * Quaternion.Euler(0.0f, timeOfDay.longitude, 0.0f) * Quaternion.Euler(0.0f, timeOfDay.lst * Mathf.Rad2Deg, 0.0f);
                    m_starfieldMatrix = Matrix4x4.TRS(Vector3.zero, skyRotation * Quaternion.Euler(options.starfieldPosition), Vector3.one);
                    Shader.SetGlobalMatrix(Uniforms._StarMatrix, m_starfieldMatrix.inverse);
                    break;
            }

            // Get sun elevation and set light position.
            m_sunElevation = Vector3.Dot(-sunTransform.transform.forward, transform.up);
            if (m_sunElevation >= 0.0f)
            {
                lightTransform.transform.localRotation = Quaternion.LookRotation(m_sunLocalDirection);
                m_isDaytime = true;
            }
            else
            {
                lightTransform.transform.localRotation = Quaternion.LookRotation(m_moonLocalDirection);
                m_isDaytime = false;
            }

            //Lighting and Reflections.
            m_lightComponent.intensity = lightIntensity;
            m_lightComponent.color = lightColor;
            RenderSettings.ambientIntensity = ambientIntensity;
            RenderSettings.ambientSkyColor = ambientSkyColor;
            RenderSettings.ambientEquatorColor = ambientEquatorColor;
            RenderSettings.ambientGroundColor = ambientGroundColor;
            if (options.useReflectionProbe == 1)
            {
                reflectionProbe.intensity = reflectionIntensity;
                m_timeSinceLastProbeUpdate += Time.deltaTime;
                if (options.reflectionProbeRefreshMode == 2)
                {
                    if (m_timeSinceLastProbeUpdate >= options.reflectionProbeUpdateTime)
                    {
                        reflectionProbe.RenderProbe ();
                        m_timeSinceLastProbeUpdate = 0;
                    }
                }
            }

			//Particles Controller.
			if (options.particlesMode == 1)
			{
				//Rain Particle.
				if (rainIntensity > 0)
				{
					if (rainParticle.isStopped)
					{
						rainParticle.Play ();
						m_rainEmission = rainParticle.emission;
					}
					m_rainEmission.rateOverTime = rainIntensity * m_maxRainEmission;
				}
				else
					{
						if (rainParticle.isPlaying)
						{
							rainParticle.Stop ();
						}
					}

				//Snow Particle.
				if (snowIntensity > 0)
				{
					if (snowParticle.isStopped)
					{
						snowParticle.Play ();
						m_snowEmission = snowParticle.emission;
					}
					m_snowEmission.rateOverTime = snowIntensity * m_maxSnowEmission;
				}
				else
				{
					if (snowParticle.isPlaying)
					{
						snowParticle.Stop ();
					}
				}
			}

			//Wind Controller.
			m_windRotation.y = windDirection * 360.0f;
			windZone.transform.eulerAngles = m_windRotation;
			windZone.windMain = windSpeed * m_maxWindSpeed;

            //Stars Scintillation.
            if (starsScintillation > 0.0f)
            {
                m_noiseMatrixRotation += starsScintillation * Time.deltaTime;
                m_noiseRotation = Quaternion.Euler(m_noiseMatrixRotation, m_noiseMatrixRotation, m_noiseMatrixRotation);
                m_noiseMatrix = Matrix4x4.TRS(Vector3.zero, m_noiseRotation, new Vector3(1, 1, 1));
            }

			#if UNITY_EDITOR
			if(!Application.isPlaying)
				currentDayProfile = GetCalendarDayProfile();
			#endif
        }

		//Calcule Total Rayleigh.
        private Vector3 GetBetaRay ()
        {
            //Converting the wavelength values given in Inspector for real scale used in formula.
            Vector3 converted_lambda = m_lambda * 1.0e-9f;
            float converted_N = N * 1.0e25f;

            Vector3 Br;
            Br.x = ((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f))) / (3.0f * converted_N * Mathf.Pow(converted_lambda.x, 4.0f))) * 1000.0f;
            Br.y = ((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f))) / (3.0f * converted_N * Mathf.Pow(converted_lambda.y, 4.0f))) * 1000.0f;
            Br.z = ((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f))) / (3.0f * converted_N * Mathf.Pow(converted_lambda.z, 4.0f))) * 1000.0f;

            //Original equation.
            //Br.x = (((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f)))*(6.0f+3.0f*m_pn) ) / ((3.0f * m_N * Mathf.Pow(converted_lambda.x, 4.0f))*(6.0f-7.0f*m_pn) ))*1000.0f;
            //Br.y = (((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f)))*(6.0f+3.0f*m_pn) ) / ((3.0f * m_N * Mathf.Pow(converted_lambda.y, 4.0f))*(6.0f-7.0f*m_pn) ))*1000.0f;
            //Br.z = (((8.0f * Mathf.Pow(m_pi, 3.0f) * (Mathf.Pow(Mathf.Pow(m_n, 2.0f) - 1.0f, 2.0f)))*(6.0f+3.0f*m_pn) ) / ((3.0f * m_N * Mathf.Pow(converted_lambda.z, 4.0f))*(6.0f-7.0f*m_pn) ))*1000.0f;

            return Br;
        }

        //Calcule Total Mie.
        private Vector3 GetBetaMie ()
        {
            //float c = (6544f * Turbidity - 6510f) * 10.0f;
            float c = (0.2f * 2.0f) * 10.0f;
            Vector3 Bm;
            Bm.x = (434.0f * c * m_pi * Mathf.Pow((2.0f * m_pi) / m_lambda.x, 2.0f) * m_K.x) / 3.0f;
            Bm.y = (434.0f * c * m_pi * Mathf.Pow((2.0f * m_pi) / m_lambda.y, 2.0f) * m_K.y) / 3.0f;
            Bm.z = (434.0f * c * m_pi * Mathf.Pow((2.0f * m_pi) / m_lambda.z, 2.0f) * m_K.z) / 3.0f;

            Bm.x = Mathf.Pow(Bm.x, -1.0f);
            Bm.y = Mathf.Pow(Bm.y, -1.0f);
            Bm.z = Mathf.Pow(Bm.z, -1.0f);

            return Bm;
        }
			
        //Evaluate the Curve Properties.
        private float GetCurveValue (AnimationCurve[] curveTarget, int curveMode)
        {
            switch (curveMode)
            {
                //Based on Timeline.
                case 0:
                    return curveTarget[0].Evaluate(m_timelineCurveTime);

                //Based on Sun Elevation.
                case 1:
                    return curveTarget[1].Evaluate(m_sunCurveTime);

                //Based on Moon Elevation.
                case 2:
                    return curveTarget[2].Evaluate(m_moonCurveTime);
            }
            return 0;
        }

        /// <summary>
        /// Get curve output and return as a Float.
        /// </summary>
        /// <param name="element">The element number of the Curve Output list.</param>
        /// <returns></returns>
        public float GetCurveOutput (int element)
        {
            int curveMode = curveOuputList[element].curveIndex;
            switch (curveMode)
            {
                case 0:
                    return curveOuputList[element].curveOutput[0].Evaluate(m_timelineCurveTime);
                case 1:
                    return curveOuputList[element].curveOutput[1].Evaluate(m_sunCurveTime);
                case 2:
                    return curveOuputList[element].curveOutput[2].Evaluate(m_moonCurveTime);
            }
            return 0;
        }
			
        //Evaluate the Gradient Properties.
        private Color GetGradientValue (Gradient[] gradientTarget, int curveMode)
        {
            switch (curveMode)
            {
                //Based on Timeline.
                case 0:
                    return gradientTarget[0].Evaluate(m_timelineGradientTime);

                //Based on Sun Elevation.
                case 1:
                    return gradientTarget[1].Evaluate(m_sunGradientTime);

                //Based on Moon Elevation.
                case 2:
                    return gradientTarget[2].Evaluate(m_moonGradientTime);
            }
            return Color.white;
        }

        /// <summary>
        /// Get gradient output and return as a Color.
        /// </summary>
        /// <param name="element">The element number of the Gradient Output list.</param>
        /// <returns></returns>
        public Color GetGradientOutput (int element)
        {
            int gradientMode = gradientOuputList[element].gradientIndex;
            switch (gradientMode)
            {
                case 0:
                    return gradientOuputList[element].gradientOutput[0].Evaluate(m_timelineGradientTime);
                case 1:
                    return gradientOuputList[element].gradientOutput[1].Evaluate(m_sunGradientTime);
                case 2:
                    return gradientOuputList[element].gradientOutput[2].Evaluate(m_moonGradientTime);
            }
            return Color.white;
        }

        /// <summary>
        /// Get the current day profile from the calendar. It will return a random standard profile if there is no day profile attached to the current calendar day.
        /// </summary>
        /// <returns></returns>
        public AzureSkyProfile GetCalendarDayProfile ()
        {
            m_dayOfYear = timeOfDay.GetDayOfYear ();
            if (calendarProfileList[m_dayOfYear])
            {
                m_getCalendarProfile = calendarProfileList[m_dayOfYear];
            }
            else
				{
					m_getCalendarProfile = standardProfileList[Random.Range(0, (int)standardProfileList.Count)];
				}
            return m_getCalendarProfile;
        }

        /// <summary>
        /// Changes the weather with a smooth transition.
        /// </summary>
        /// <param name="target">The profile number from the Inspector's "Weather Profiles" list.</param>
        public void SetNewWeatherProfile (int target)
        {
			if (!isBlendingWeathers)
            {
                switch (target)
                {
				case 0:
                    //Call Default Weather.
					m_transitionTime = weatherProfileList [target].TransitionTime;
					m_targetWeatherProfile = calendarDayProfile;
					break;

                    default:
                    m_transitionTime = weatherProfileList[target].TransitionTime;
                    m_targetWeatherProfile = weatherProfileList[target].Profile;
                    break;
                }
				weatherTransitionTime = 0.0f;
                m_startTime = Time.time;
				isBlendingWeathers = true;
                weatherNumber = target;
                SetStaticCloudTexture(sourceWeatherProfile.clouds.staticCloudTexture, m_targetWeatherProfile.clouds.staticCloudTexture);
            }
            else
				{
					Debug.Log("A weather transition is still in progress.");
				}
        }

		//Blending Weather Profiles.
        private void BlendingWeathers ()
        {
			//HACK.
			if (weatherNumber == 0 && !isBlendingWeathers) { sourceWeatherProfile = calendarDayProfile; }

			weatherTransitionTime = Mathf.Clamp01((Time.time - m_startTime) / m_transitionTime);
			SetWeightedProfiles(new WeightedDayProfile(sourceWeatherProfile, 1.0f - weatherTransitionTime), new WeightedDayProfile(m_targetWeatherProfile, weatherTransitionTime));
            BlendWeatherProfiles(m_weightedProfiles);
            Shader.SetGlobalFloat(Uniforms._WeatherTransitionTime, weatherTransitionTime);
            if (weatherTransitionTime == 1.0f)
            {
				isBlendingWeathers = false;
				weatherTransitionTime = 0.0f;
                m_startTime = 0.0f;
				sourceWeatherProfile = m_targetWeatherProfile;
                currentDayProfile = m_targetWeatherProfile;
                SetStaticCloudTexture(currentDayProfile.clouds.staticCloudTexture, currentDayProfile.clouds.staticCloudTexture);
            }
        }

        private void SetWeightedProfiles (params WeightedDayProfile[] profiles)
        {
            m_weightedProfiles = profiles;
        }

        //Based on PlayWayWater weight system.
        private void BlendWeatherProfiles (WeightedDayProfile[] profiles)
        {
            N = 0.0f;
            kr = 0.0f;
            km = 0.0f;
            m_lambda = Vector3.zero;
            rayleigh = 0;
            mie = 0.0f;
            nightIntensity = 0.0f;
            sunIntensity = 0.0f;
            sunDiskIntensity = 0.0f;
            exposure = 0.0f;
            rayleighColor = Color.black;
            mieColor = Color.black;

            moonColor = Color.black;
            moonBrightColor = Color.black;
            moonBrightRange = 0.0f;
            moonEmission = 0.0f;
            starsScintillation = 0.0f;
            starfieldIntensity = 0.0f;
            milkyWayIntensity = 0.0f;

			fogScale = 0.0f;
            fogBlend = 0.0f;
            fogDistance = 0.0f;
            heightFogBlend = 0.0f;
            heightFogDensity = 0.0f;
            heightFogDistance = 0.0f;
            heightFogStart = 0.0f;
            heightFogEnd = 0.0f;

            dynamicCloudLayer1Altitude = 0.0f;
            dynamicCloudLayer1Direction = 0.0f;
            dynamicCloudLayer1Speed = 0.0f;
            dynamicCloudLayer1Color1 = Color.black;
            dynamicCloudLayer1Color2 = Color.black;
            dynamicCloudLayer1Density = 0.0f;
            staticCloudColor = Color.black;
            staticCloudScattering = 0.0f;
            staticCloudExtinction = 0.0f;
            staticCloudPower = 0.0f;
            staticCloudIntensity = 0.0f;
            staticCloudRotationSpeed = 0.0f;

            lightIntensity = 0.0f;
            lightColor = Color.black;
            ambientIntensity = 0.0f;
            ambientSkyColor = Color.black;
            ambientEquatorColor = Color.black;
            ambientGroundColor = Color.black;
            reflectionIntensity = 0.0f;

			rainColor = Color.black;
			snowColor = Color.black;
			rainIntensity = 0.0f;
			snowIntensity = 0.0f;
			windSpeed = 0.0f;
			windDirection = 0.0f;
			rainLightVolume = 0.0f;
			rainMediumVolume = 0.0f;
			rainHeavyVolume = 0.0f;
			windLightVolume = 0.0f;
			windMediumVolume = 0.0f;
			windHeavyVolume = 0.0f;
			wetness = 0.0f;
			snowAmount = 0.0f;
			covarage = 0.0f;
			outputColor1 = Color.black;
			outputColor2 = Color.black;
			outputColor3 = Color.black;

            for (int i = 0; i < profiles.Length; ++i)
            {
                var profile = profiles[i].profile;
                float weight = profiles[i].weight;

                N += profile.scattering.N * weight;
                kr += profile.scattering.kr * weight;
                km += profile.scattering.km * weight;
                m_lambda += profile.scattering.lambda * weight;
                rayleigh += GetCurveValue (profile.scattering.rayleighCurve, profile.scattering.rayleighCurveIndex) * weight;
                mie += GetCurveValue (profile.scattering.mieCurve, profile.scattering.mieCurveIndex) * weight;
                nightIntensity += GetCurveValue (profile.scattering.nightIntensityCurve, profile.scattering.nightIntensityCurveIndex) * weight;
                sunIntensity += GetCurveValue (profile.scattering.sunIntensityCurve, profile.scattering.sunIntensityCurveIndex) * weight;
                sunDiskIntensity += GetCurveValue (profile.scattering.sunDiskIntensityCurve, profile.scattering.sunDiskIntensityCurveIndex) * weight;
                exposure += GetCurveValue (profile.scattering.exposureCurve, profile.scattering.exposureCurveIndex) * weight;
                rayleighColor += GetGradientValue (profile.scattering.rayleighGradientColor, profile.scattering.rayleighGradientIndex) * weight;
                mieColor += GetGradientValue (profile.scattering.mieGradientColor, profile.scattering.mieGradientIndex) * weight;

                moonColor += GetGradientValue (profile.deepSpace.moonColorGradientColor, profile.deepSpace.moonColorGradientIndex) * weight;
                moonBrightColor += GetGradientValue (profile.deepSpace.moonBrightColorGradientColor, profile.deepSpace.moonBrightColorGradientIndex) * weight;
                moonBrightRange += GetCurveValue (profile.deepSpace.moonBrightRangeCurve, profile.deepSpace.moonBrightRangeCurveIndex) * weight;
                moonEmission += GetCurveValue (profile.deepSpace.moonEmissionCurve, profile.deepSpace.moonEmissionCurveIndex) * weight;
                starsScintillation += profile.deepSpace.starsScintillation * weight;
                starfieldIntensity += GetCurveValue (profile.deepSpace.starfieldIntensityCurve, profile.deepSpace.starfieldIntensityCurveIndex) * weight;
                milkyWayIntensity += GetCurveValue (profile.deepSpace.milkyWayIntensityCurve, profile.deepSpace.milkyWayIntensityCurveIndex) * weight;

				fogScale += profile.fogScattering.fogScale * weight;
                fogBlend += GetCurveValue (profile.fogScattering.fogBlendCurve, profile.fogScattering.fogBlendCurveIndex) * weight;
                fogDistance += GetCurveValue (profile.fogScattering.fogDistanceCurve, profile.fogScattering.fogDistanceCurveIndex) * weight;
                heightFogBlend += GetCurveValue (profile.fogScattering.heightFogBlendCurve, profile.fogScattering.heightFogBlendCurveIndex) * weight;
                heightFogDensity += GetCurveValue (profile.fogScattering.heightFogDensityCurve, profile.fogScattering.heightFogDensityCurveIndex) * weight;
                heightFogDistance += GetCurveValue (profile.fogScattering.heightFogDistanceCurve, profile.fogScattering.heightFogDistanceCurveIndex) * weight;
                heightFogStart += GetCurveValue (profile.fogScattering.heightFogStartCurve, profile.fogScattering.heightFogStartCurveIndex) * weight;
                heightFogEnd += GetCurveValue (profile.fogScattering.heightFogEndCurve, profile.fogScattering.heightFogEndCurveIndex) * weight;

				switch (options.cloudMode)
				{
                    case 1:
					    dynamicCloudLayer1Altitude += profile.clouds.dynamicCloudLayer1Altitude * weight;
					    dynamicCloudLayer1Direction += profile.clouds.dynamicCloudLayer1Direction * weight;
					    dynamicCloudLayer1Speed += profile.clouds.dynamicCloudLayer1Speed * weight;
					    dynamicCloudLayer1Density += GetCurveValue (profile.clouds.dynamicCloudLayer1DensityCurve, profile.clouds.dynamicCloudLayer1DensityCurveIndex) * weight;
					    dynamicCloudLayer1Color1 += GetGradientValue (profile.clouds.dynamicCloudLayer1GradientColor1, profile.clouds.dynamicCloudLayer1GradientColor1Index) * weight;
					    dynamicCloudLayer1Color2 += GetGradientValue (profile.clouds.dynamicCloudLayer1GradientColor2, profile.clouds.dynamicCloudLayer1GradientColor2Index) * weight;
                        break;

                    case 2:
                        staticCloudColor += GetGradientValue(profile.clouds.staticCloudColor, profile.clouds.staticCloudColorIndex) * weight;
                        staticCloudScattering += GetCurveValue(profile.clouds.staticCloudScatteringCurve, profile.clouds.staticCloudScatteringCurveIndex) * weight;
                        staticCloudExtinction += GetCurveValue(profile.clouds.staticCloudExtinctionCurve, profile.clouds.staticCloudExtinctionCurveIndex) * weight;
                        staticCloudPower += GetCurveValue(profile.clouds.staticCloudPowerCurve, profile.clouds.staticCloudPowerCurveIndex) * weight;
                        staticCloudIntensity += GetCurveValue(profile.clouds.staticCloudIntensityCurve, profile.clouds.staticCloudIntensityCurveIndex) * weight;
                        staticCloudRotationSpeed += profile.clouds.staticCloudRotationSpeed * weight;
                        break;
				}

                lightIntensity += GetCurveValue (profile.lighting.directionalLightIntensityCurve, profile.lighting.directionalLightIntensityCurveIndex) * weight;
                lightColor += GetGradientValue (profile.lighting.directionalLightGradientColor, profile.lighting.directionalLightGradientColorIndex) * weight;
                ambientIntensity += GetCurveValue (profile.lighting.ambientIntensityCurve, profile.lighting.ambientIntensityCurveIndex) * weight;
                ambientSkyColor += GetGradientValue (profile.lighting.ambientSkyGradientColor, profile.lighting.ambientSkyGradientColorIndex) * weight;
                ambientEquatorColor += GetGradientValue (profile.lighting.equatorGradientColor, profile.lighting.equatorGradientColorIndex) * weight;
                ambientGroundColor += GetGradientValue (profile.lighting.groundGradientColor, profile.lighting.groundGradientColorIndex) * weight;
                reflectionIntensity += GetCurveValue (profile.lighting.reflectionIntensityCurve, profile.lighting.reflectionIntensityCurveIndex) * weight;

				if (options.particlesMode == 1 || options.keepWeatherUpdate)
				{
					rainColor += GetGradientValue (profile.weather.weatherRainGradientColor, profile.weather.weatherRainGradientColorIndex) * weight;
					snowColor += GetGradientValue (profile.weather.weatherSnowGradientColor, profile.weather.weatherSnowGradientColorIndex) * weight;
					rainIntensity += GetCurveValue (profile.weather.weatherRainIntensityCurve, profile.weather.weatherRainIntensityCurveIndex) * weight;
					snowIntensity += GetCurveValue (profile.weather.weatherSnowIntensityCurve, profile.weather.weatherSnowIntensityCurveIndex) * weight;
					windSpeed += GetCurveValue (profile.weather.weatherWindSpeedCurve, profile.weather.weatherWindSpeedCurveIndex) * weight;
					windDirection += GetCurveValue (profile.weather.weatherWindDirectionCurve, profile.weather.weatherWindDirectionCurveIndex) * weight;
					rainLightVolume += profile.weather.weatherRainLightVolume * weight;
					rainMediumVolume += profile.weather.weatherRainMediumVolume * weight;
					rainHeavyVolume += profile.weather.weatherRainHeavyVolume * weight;
					windLightVolume += profile.weather.weatherWindLightVolume * weight;
					windMediumVolume += profile.weather.weatherWindMediumVolume * weight;
					windHeavyVolume += profile.weather.weatherWindHeavyVolume * weight;
				}

				if (options.keepWeatherUpdate)
				{
					wetness += GetCurveValue (profile.weather.weatherWetnessCurve, profile.weather.weatherWetnessCurveIndex) * weight;
					snowAmount += GetCurveValue (profile.weather.weatherSnowAmountCurve, profile.weather.weatherSnowAmountCurveIndex) * weight;
					covarage += GetCurveValue (profile.weather.weatherCovarageCurve, profile.weather.weatherCovarageCurveIndex) * weight;
					outputColor1 += GetGradientValue (profile.weather.weatherOutputColor1GradientColor, profile.weather.weatherOutputColor1Index) * weight;
					outputColor2 += GetGradientValue (profile.weather.weatherOutputColor2GradientColor, profile.weather.weatherOutputColor2Index) * weight;
					outputColor3 += GetGradientValue (profile.weather.weatherOutputColor3GradientColor, profile.weather.weatherOutputColor3Index) * weight;
				}
            }
        }

		/// <summary>
		/// Updates the profiles and calendar days.
		/// </summary>
		public void UpdateProfiles ()
		{
			timeOfDay.UpdateCalendar (timeOfDay.selectableDayList);
			calendarDayProfile = GetCalendarDayProfile ();
			if (weatherNumber == 0)
			{
				currentDayProfile = calendarDayProfile;
				if (!isBlendingWeathers)
				{
					sourceWeatherProfile = calendarDayProfile;
				}
			}
		}

		//Start and Stop Sound Effects.
		private void SoundPlayController(float volume, AudioSource sound)
		{
            sound.volume = volume;
            if (volume > 0)
			{
				if (!sound.isPlaying) sound.Play ();
			}
			else if (sound.isPlaying) sound.Stop ();
		}

		/// <summary>
		/// It plays the thunder sound fx from the Inspector's "Thunder Audio Clips" list.
		/// </summary>
		/// <param name="element">The element number from the Inspector's "Thunder Audio Clips" list.</param>
		public void PlayThunderAudioClip(int element)
		{
			if (!soundFX.thunder.isPlaying)
			{
				soundFX.thunder.clip = thunderAudioClipList [element];
				soundFX.thunder.Play ();
			}
			else
				{
					Debug.Log("The Thunder Audio Source is currently in use, wait until the current clip finishes and try again.");
				}
		}

		/// <summary>
		/// It plays a random thunder sound fx from the Inspector's "Thunder Audio Clips" list.
		/// </summary>
		public void PlayThunderAudioClipRandom()
		{
			if (!soundFX.thunder.isPlaying)
			{
				soundFX.thunder.clip = thunderAudioClipList [Random.Range(0, (int)thunderAudioClipList.Count)];
				soundFX.thunder.Play ();
			}
			else
			{
				Debug.Log("The Thunder Audio Source is currently in use, wait until the current clip finishes and try again.");
			}
		}

        //It Needs to be Updated Only Once When the Scene Starts.
        private void InitializeShaderUniforms ()
        {
            Shader.SetGlobalTexture (Uniforms._SunTexture, sunTexture);
            Shader.SetGlobalTexture (Uniforms._MoonTexture, moonTexture);
            Shader.SetGlobalTexture (Uniforms._CloudNoise, cloudNoise);
            if (!isBlendingWeathers)
            {
                SetStaticCloudTexture(currentDayProfile.clouds.staticCloudTexture, currentDayProfile.clouds.staticCloudTexture);
            }
            Shader.SetGlobalTexture (Uniforms._StarfieldTexture, starfieldTexture);
            Shader.SetGlobalTexture (Uniforms._StarNoiseTexture, starNoiseTexture);
            Shader.SetGlobalVector (Uniforms._MieG, new Vector3(0.4375f, 1.5625f, 1.5f));
            Shader.SetGlobalFloat (Uniforms._Pi316, 0.0596831f);
            Shader.SetGlobalFloat (Uniforms._Pi14, 0.07957747f);
            Shader.SetGlobalFloat (Uniforms._Pi, m_pi);
            Shader.SetGlobalFloat (Uniforms._LightSpeed, options.lightSpeed);
            Shader.SetGlobalFloat (Uniforms._MieDepth, options.mieDepth);
            Shader.SetGlobalFloat (Uniforms._SunSize, options.sunSize);
            Shader.SetGlobalFloat (Uniforms._MoonSize, options.moonSize);
            Shader.SetGlobalInt (Uniforms._SunsetColorMode, options.sunsetColorMode);
        }

        //It Needs to be Constantly Updated.
        private void UpdateShaderUniforms ()
        {
            Shader.SetGlobalFloat (Uniforms._Kr, kr);
            Shader.SetGlobalFloat (Uniforms._Km, km);
            Shader.SetGlobalFloat (Uniforms._NightIntensity, nightIntensity);
            Shader.SetGlobalColor (Uniforms._RayleighColor, rayleighColor);
            Shader.SetGlobalColor (Uniforms._MieColor, mieColor);
            Shader.SetGlobalFloat (Uniforms._SunIntensity, sunIntensity * 60.0f);
            Shader.SetGlobalFloat (Uniforms._SunDiskIntensity, sunDiskIntensity);
            Shader.SetGlobalFloat (Uniforms._Exposure, exposure);
            Shader.SetGlobalVector (Uniforms._SunDirection, transform.InverseTransformDirection (-sunTransform.transform.forward));
            Shader.SetGlobalVector (Uniforms._MoonDirection, transform.InverseTransformDirection (- moonTransform.transform.forward));
            switch (options.wavelengthMode)
            {
                case 0:
                    Shader.SetGlobalVector (Uniforms._Br, new Vector3(0.00519673f, 0.0121427f, 0.0296453f) * rayleigh);
                    Shader.SetGlobalVector (Uniforms._Bm, new Vector3(0.005721017f, 0.004451339f, 0.003146905f) * mie);
                    break;
                case 1:
                    Shader.SetGlobalVector (Uniforms._Br, GetBetaRay() * rayleigh);
                    Shader.SetGlobalVector (Uniforms._Bm, GetBetaMie() * mie);
                    break;
            }

            Shader.SetGlobalColor (Uniforms._MoonColor, moonColor);
            Shader.SetGlobalColor (Uniforms._MoonBrightColor, moonBrightColor);
            Shader.SetGlobalFloat (Uniforms._MoonBrightRange, moonBrightRange);
            Shader.SetGlobalFloat (Uniforms._MoonEmission, moonEmission);
            Shader.SetGlobalMatrix (Uniforms._MoonMatrix, moonTransform.transform.worldToLocalMatrix);
            Shader.SetGlobalVector (Uniforms._StarfieldColorBalance, options.starfieldColor);
            Shader.SetGlobalFloat (Uniforms._StarfieldIntensity, starfieldIntensity);
            Shader.SetGlobalFloat (Uniforms._MilkyWayIntensity, milkyWayIntensity);
            Shader.SetGlobalMatrix (Uniforms._NoiseMatrix, m_noiseMatrix);

			Shader.SetGlobalFloat (Uniforms._FogScale, fogScale);
            Shader.SetGlobalFloat (Uniforms._FogBlend, fogBlend);
            Shader.SetGlobalFloat (Uniforms._FogDistance, fogDistance);
            Shader.SetGlobalFloat (Uniforms._HeightFogBlend, heightFogBlend);
            Shader.SetGlobalFloat (Uniforms._HeightFogDensity, heightFogDensity);
            Shader.SetGlobalFloat (Uniforms._HeightFogDistance, heightFogDistance);
            Shader.SetGlobalFloat (Uniforms._HeightFogStart, options.planetRadius + heightFogStart);
            Shader.SetGlobalFloat (Uniforms._HeightFogEnd, options.planetRadius + heightFogEnd);

			//TODO Cloud Layers.
			switch (options.cloudMode)
			{
                case 1:
				    Shader.SetGlobalFloat (Uniforms._DynamicCloudLayer1Altitude, dynamicCloudLayer1Altitude);
				    Shader.SetGlobalFloat (Uniforms._DynamicCloudLayer1Direction, dynamicCloudLayer1Direction);
				    Shader.SetGlobalFloat (Uniforms._DynamicCloudLayer1Speed, dynamicCloudLayer1Speed);
				    Shader.SetGlobalFloat (Uniforms._DynamicCloudLayer1Density, Mathf.Lerp (25.0f, 0.0f, dynamicCloudLayer1Density));
				    Shader.SetGlobalColor (Uniforms._DynamicCloudLayer1Color1, dynamicCloudLayer1Color1);
				    Shader.SetGlobalColor (Uniforms._DynamicCloudLayer1Color2, dynamicCloudLayer1Color2);
                    break;

                case 2:
                    Shader.SetGlobalColor(Uniforms._StaticCloudColor, staticCloudColor);
                    Shader.SetGlobalFloat(Uniforms._StaticCloudScattering, staticCloudScattering);
                    Shader.SetGlobalFloat(Uniforms._StaticCloudExtinction, staticCloudExtinction);
                    Shader.SetGlobalFloat(Uniforms._StaticCloudPower, staticCloudPower);
                    Shader.SetGlobalFloat(Uniforms._StaticCloudIntensity, staticCloudIntensity);
                    Shader.SetGlobalFloat(Uniforms._StaticCloudRotationSpeed, m_cloudRotationSpeed);
                    break;
			}

			if (options.particlesMode == 1)
			{
				rainColor.a = 0.5f;
				snowColor.a = 0.5f;
				rainMaterial.SetColor (Uniforms._RainColor, rainColor);
				snowMaterial.SetColor (Uniforms._SnowColor, snowColor);
			}
        }

        private void SetStaticCloudTexture(Texture2D source, Texture2D destination)
        {
            Shader.SetGlobalTexture(Uniforms._StaticCloudTextureSource, source);
            Shader.SetGlobalTexture(Uniforms._StaticCloudTextureDestination, destination);
        }

		/// <summary>
		/// Configure the sky material with its appropriate shader.
		/// </summary>
		public void ConfigureShaders ()
		{
			switch (options.shaderMode)
			{
			case 0://Pixel Shader.
				skydome.gameObject.SetActive (false);
				RenderSettings.skybox = skyMaterial;
				Shader.SetGlobalInt (Uniforms._CullMode, 2);
				switch (options.cloudMode)
				{
				case 0:
					skyMaterial.shader = Shader.Find ("Azure[Sky]/Pixel Sky");
					break;

				case 1:
					skyMaterial.shader = Shader.Find ("Azure[Sky]/Pixel Dynamic Cloud");
					break;

                case 2:
                    skyMaterial.shader = Shader.Find("Azure[Sky]/Pixel Static Cloud");
                    break;
                }
				break;
			case 1://Vertex Shader.
				skydome.gameObject.SetActive (true);
				RenderSettings.skybox = null;
				Shader.SetGlobalInt (Uniforms._CullMode, 1);
				switch (options.cloudMode)
				{
				case 0:
					skyMaterial.shader = Shader.Find ("Azure[Sky]/Vertex Sky");
					break;

				case 1:
					skyMaterial.shader = Shader.Find ("Azure[Sky]/Vertex Dynamic Cloud");
					break;

                case 2:
					skyMaterial.shader = Shader.Find ("Azure[Sky]/Vertex Static Cloud");
					break;
				}
				break;
			}
		}

		/// <summary>
		/// Activates/Deactivates the Particles.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetParticlesActive (bool value)
		{
			if (rainParticle) rainParticle.gameObject.SetActive (value);
			if (snowParticle) snowParticle.gameObject.SetActive (value);
		}

		/// <summary>
		/// Activates/Deactivates the Particles.
		/// </summary>
		/// <param name="value">Value.</param>
		public void SetParticlesActive (int value)
		{
			bool isTrue = value != 0;
			if (rainParticle) rainParticle.gameObject.SetActive (isTrue);
			if (snowParticle) snowParticle.gameObject.SetActive (isTrue);
		}
    }
}