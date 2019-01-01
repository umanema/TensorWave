using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public struct WeatherProfile
    {
        public AzureSkyProfile Profile;
        public float TransitionTime;
    }

    public struct WeightedDayProfile
    {
        public AzureSkyProfile profile;
        public float weight;

        public WeightedDayProfile(AzureSkyProfile profile, float weight)
        {
            this.profile = profile;
            this.weight = weight;
        }
    }
}