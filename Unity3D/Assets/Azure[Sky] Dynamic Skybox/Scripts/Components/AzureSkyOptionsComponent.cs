using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureSkyOptionsComponent
    {
        public bool startAtCurrentTime  = false;
        public bool startAtCurrentDate  = false;
		public bool followMainCamera = true;
        public float planetRadius = 0.0f;
        public float lightSpeed   = 50.0f;
        public float mieDepth = 0.0f;
        public float sunSize = 1.5f;
        public float moonSize     = 10.0f;
        public Vector3 starfieldPosition;
        public Vector3 starfieldColor;
        public int particlesMode  = 1;
		public bool keepWeatherUpdate = true;
        public int cloudMode      = 1;
        //public int skyModel       = 0;
        public int repeatMode     = 0;
        public int sunsetColorMode = 0;
        public int wavelengthMode = 0;
		public int shaderMode = 0;
        public int useReflectionProbe = 0;
        public int reflectionProbeRefreshMode = 0;
        public float reflectionProbeUpdateTime = 0.0f;
        public bool reflectionProbeUpdateAtFirstFrame = true;
    }
}