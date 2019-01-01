using System;

namespace UnityEngine.AzureSky
{
    [Serializable]
    public class AzureEditorSettings
    {
        //Sky Controller Inspector.
        public bool showTimeOfDayTab = true;
        public bool showReferencesTab = false;
        public bool showClimateTab = false;
        public bool showOptionsTab = false;
        public bool showOutputTab = false;

        //Profile Inspector.
        public bool showScatteringTab = false;
        public bool showDeepSpaceTab = false;
        public bool showFogScatteringTab = false;
        public bool showCloudsTab = false;
        public bool showLightingTab = false;
        public bool showWeatherTab = false;
    }
}