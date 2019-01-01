namespace UnityEngine.AzureSky
{
    [CreateAssetMenu(fileName = "Day Profile", menuName = "Azure[Sky] Dynamic Skybox/New Day Profile", order = 1)]
    public class AzureSkyProfile : ScriptableObject
    {
        #if UNITY_EDITOR
        public AzureEditorSettings editorSettings;
        #endif

        //Components.
        public AzureSkyScatteringComponent scattering = new AzureSkyScatteringComponent();
        public AzureSkyDeepSpaceComponent deepSpace = new AzureSkyDeepSpaceComponent();
        public AzureSkyFogScatteringComponent fogScattering = new AzureSkyFogScatteringComponent();
        public AzureSkyCloudsComponent clouds = new AzureSkyCloudsComponent();
        public AzureSkyLightingComponent lighting = new AzureSkyLightingComponent();
		public AzureSkyWeatherComponent weather = new AzureSkyWeatherComponent();
    }
}