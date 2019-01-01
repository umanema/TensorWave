using UnityEngine;
using UnityEngine.AzureSky;
using System;

namespace UnityEditor.AzureSky
{
    [CustomEditor(typeof(AzureSkyProfile))]
    public class AzureSkyProfileEditor : Editor
    {
        AzureSkyProfile m_Target;

        //Popups.
        private string[] m_curveMode = new string[] { "Timeline", "Sun", "Moon" };

        //Show-Hide strings.
        private string m_showHideScattering;
        private string m_showHideDeepSpace;
        private string m_showHideFogScattering;
        private string m_showHideClouds;
        private string m_showHideLighting;
        private string m_showHideWeather;

        //Custom GUI.
        private int m_labelWidth = 92;
        private Texture2D m_logoTex, m_tabTex;
        private Rect m_bgRect;
        private string m_installPath;
        private string m_inspectorGUIPath;
        private Color m_col1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);//Normal.
        private Color m_col2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);//All Transparent.
        private Color m_col3 = new Color(0.75f, 1.0f, 0.75f, 1.0f);//Green;
        private Color m_col4 = new Color(1.0f, 0.5f, 0.5f, 1.0f);//Red;
        private Color m_curveColor = Color.green;

        //Serialized Properties.
        SerializedProperty m_rayleighCurve;
        SerializedProperty m_mieCurve;
        SerializedProperty m_nightIntensityCurve;
        SerializedProperty m_rayleighGradientColor;
        SerializedProperty m_mieGradientColor;
        SerializedProperty m_sunIntensityCurve;
        SerializedProperty m_sunDiskIntensityCurve;
        SerializedProperty m_exposureCurve;
        //Deep Space.
        SerializedProperty m_moonGradientColor;
        SerializedProperty m_moonBrightGradientColor;
        SerializedProperty m_moonBrightRangeCurve;
        SerializedProperty m_moonEmissionCurve;
        SerializedProperty m_starfieldCurve;
        SerializedProperty m_milkyWayCurve;
        //Fog Scattering.
        SerializedProperty m_fogBlendCurve;
        SerializedProperty m_fogDistanceCurve;
        SerializedProperty m_heightFogBlendCurve;
        SerializedProperty m_heightFogDensityCurve;
        SerializedProperty m_heightFogDistanceCurve;
        SerializedProperty m_heightFogStartCurve;
        SerializedProperty m_heightFogEndCurve;
        //Clouds.
        SerializedProperty m_dynamicCloudLayer1GradientColor1;
        SerializedProperty m_dynamicCloudLayer1GradientColor2;
        SerializedProperty m_dynamicCloudLayer1DensityCurve;
        SerializedProperty m_staticCloudColor;
        SerializedProperty m_staticCloudScatteringCurve;
        SerializedProperty m_staticCloudExtinctionCurve;
        SerializedProperty m_staticCloudPowerCurve;
        SerializedProperty m_staticCloudIntensityCurve;
        //Lighting.
        SerializedProperty m_directionalLightIntensityCurve;
        SerializedProperty m_directionalLightGradientColor;
        SerializedProperty m_moonDirectionalLightIntensityCurve;
        SerializedProperty m_moonDirectionalLightGradientColor;
        SerializedProperty m_ambientIntensityCurve;
        SerializedProperty m_ambientSkyGradientColor;
        SerializedProperty m_equatorGradientColor;
        SerializedProperty m_groundGradientColor;
        SerializedProperty m_reflectionIntensityCurve;
		//Weather
		SerializedProperty m_weatherRainGradientColor;
		SerializedProperty m_weatherSnowGradientColor;
		SerializedProperty m_weatherRainIntensityCurve;
		SerializedProperty m_weatherSnowIntensityCurve;
		SerializedProperty m_weatherWindSpeedCurve;
		SerializedProperty m_weatherWindDirectionCurve;
		SerializedProperty m_weatherWetnessCurve;
		SerializedProperty m_weatherSnowAmountCurve;
		SerializedProperty m_weatherCovarageCurve;
		SerializedProperty m_weatherOutputColor1GradientColor;
		SerializedProperty m_weatherOutputColor2GradientColor;
		SerializedProperty m_weatherOutputColor3GradientColor;

        void OnEnable()
        {
            //Get Target.
            m_Target = (AzureSkyProfile)target;

            //InspectorGUI folder path.
            string scriptLocation = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            m_installPath = scriptLocation.Replace("/Editor/AzureSkyProfileEditor.cs", "");
            m_inspectorGUIPath = m_installPath + "/Editor/InspectorGUI";

            //Get Serialized Properties.
            m_rayleighCurve = serializedObject.FindProperty("scattering.rayleighCurve");
            m_mieCurve = serializedObject.FindProperty("scattering.mieCurve");
            m_nightIntensityCurve = serializedObject.FindProperty("scattering.nightIntensityCurve");
            m_rayleighGradientColor = serializedObject.FindProperty("scattering.rayleighGradientColor");
            m_mieGradientColor = serializedObject.FindProperty("scattering.mieGradientColor");
            m_sunIntensityCurve = serializedObject.FindProperty("scattering.sunIntensityCurve");
            m_sunDiskIntensityCurve = serializedObject.FindProperty("scattering.sunDiskIntensityCurve");
            m_exposureCurve = serializedObject.FindProperty("scattering.exposureCurve");
            //Deep Space.
            m_moonGradientColor = serializedObject.FindProperty("deepSpace.moonColorGradientColor");
            m_moonBrightGradientColor = serializedObject.FindProperty("deepSpace.moonBrightColorGradientColor");
            m_moonBrightRangeCurve = serializedObject.FindProperty("deepSpace.moonBrightRangeCurve");
            m_moonEmissionCurve = serializedObject.FindProperty("deepSpace.moonEmissionCurve");
            m_starfieldCurve = serializedObject.FindProperty("deepSpace.starfieldIntensityCurve");
            m_milkyWayCurve = serializedObject.FindProperty("deepSpace.milkyWayIntensityCurve");
            //Fog Scattering.
            m_fogBlendCurve = serializedObject.FindProperty("fogScattering.fogBlendCurve");
            m_fogDistanceCurve = serializedObject.FindProperty("fogScattering.fogDistanceCurve");
            m_heightFogBlendCurve = serializedObject.FindProperty("fogScattering.heightFogBlendCurve");
            m_heightFogDensityCurve = serializedObject.FindProperty("fogScattering.heightFogDensityCurve");
            m_heightFogDistanceCurve = serializedObject.FindProperty("fogScattering.heightFogDistanceCurve");
            m_heightFogStartCurve = serializedObject.FindProperty("fogScattering.heightFogStartCurve");
            m_heightFogEndCurve = serializedObject.FindProperty("fogScattering.heightFogEndCurve");
            //Clouds.
            m_dynamicCloudLayer1GradientColor1 = serializedObject.FindProperty("clouds.dynamicCloudLayer1GradientColor1");
            m_dynamicCloudLayer1GradientColor2 = serializedObject.FindProperty("clouds.dynamicCloudLayer1GradientColor2");
            m_dynamicCloudLayer1DensityCurve = serializedObject.FindProperty("clouds.dynamicCloudLayer1DensityCurve");
            m_staticCloudColor = serializedObject.FindProperty("clouds.staticCloudColor");
            m_staticCloudScatteringCurve = serializedObject.FindProperty("clouds.staticCloudScatteringCurve");
            m_staticCloudExtinctionCurve = serializedObject.FindProperty("clouds.staticCloudExtinctionCurve");
            m_staticCloudPowerCurve = serializedObject.FindProperty("clouds.staticCloudPowerCurve");
            m_staticCloudIntensityCurve = serializedObject.FindProperty("clouds.staticCloudIntensityCurve");
            //Lighting.
            m_directionalLightIntensityCurve = serializedObject.FindProperty("lighting.directionalLightIntensityCurve");
            m_directionalLightGradientColor = serializedObject.FindProperty("lighting.directionalLightGradientColor");
            m_ambientIntensityCurve = serializedObject.FindProperty("lighting.ambientIntensityCurve");
            m_ambientSkyGradientColor = serializedObject.FindProperty("lighting.ambientSkyGradientColor");
            m_equatorGradientColor = serializedObject.FindProperty("lighting.equatorGradientColor");
            m_groundGradientColor = serializedObject.FindProperty("lighting.groundGradientColor");
            m_reflectionIntensityCurve = serializedObject.FindProperty("lighting.reflectionIntensityCurve");
			//Weather.
			m_weatherRainGradientColor = serializedObject.FindProperty("weather.weatherRainGradientColor");
			m_weatherSnowGradientColor = serializedObject.FindProperty("weather.weatherSnowGradientColor");
			m_weatherRainIntensityCurve = serializedObject.FindProperty("weather.weatherRainIntensityCurve");
			m_weatherSnowIntensityCurve = serializedObject.FindProperty("weather.weatherSnowIntensityCurve");
			m_weatherWindSpeedCurve = serializedObject.FindProperty("weather.weatherWindSpeedCurve");
			m_weatherWindDirectionCurve = serializedObject.FindProperty("weather.weatherWindDirectionCurve");
			m_weatherWetnessCurve = serializedObject.FindProperty("weather.weatherWetnessCurve");
			m_weatherSnowAmountCurve = serializedObject.FindProperty("weather.weatherSnowAmountCurve");
			m_weatherCovarageCurve = serializedObject.FindProperty("weather.weatherCovarageCurve");
			m_weatherOutputColor1GradientColor = serializedObject.FindProperty("weather.weatherOutputColor1GradientColor");
			m_weatherOutputColor2GradientColor = serializedObject.FindProperty("weather.weatherOutputColor2GradientColor");
			m_weatherOutputColor3GradientColor = serializedObject.FindProperty("weather.weatherOutputColor3GradientColor");
        }
        
        public override void OnInspectorGUI()
        {
            //Start.
            //-------------------------------------------------------------------------------------------------------
            Undo.RecordObject(m_Target, "Undo Day Profile");

            //Show-Hide tab text.
            //-------------------------------------------------------------------------------------------------------
            if (m_Target.editorSettings.showScatteringTab) m_showHideScattering = "| Hide"; else m_showHideScattering = "| Show";
            if (m_Target.editorSettings.showDeepSpaceTab) m_showHideDeepSpace = "| Hide"; else m_showHideDeepSpace = "| Show";
            if (m_Target.editorSettings.showFogScatteringTab) m_showHideFogScattering = "| Hide"; else m_showHideFogScattering = "| Show";
            if (m_Target.editorSettings.showCloudsTab) m_showHideClouds = "| Hide"; else m_showHideClouds = "| Show";
            if (m_Target.editorSettings.showLightingTab) m_showHideLighting = "| Hide"; else m_showHideLighting = "| Show";
            if (m_Target.editorSettings.showWeatherTab) m_showHideWeather = "| Hide"; else m_showHideWeather = "| Show";

            //Logo and Textures.
            //-------------------------------------------------------------------------------------------------------
            m_logoTex = AssetDatabase.LoadAssetAtPath(m_inspectorGUIPath + "/AzureSkyProfileBar.png", typeof(Texture2D)) as Texture2D;
            m_tabTex = AssetDatabase.LoadAssetAtPath(m_inspectorGUIPath + "/InspectorTab.png", typeof(Texture2D)) as Texture2D;
            EditorGUILayout.Space();
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(m_bgRect.x, m_bgRect.y, 261, 56), m_logoTex);
            GUILayout.Space(40);



            //Scattering Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showScatteringTab = !m_Target.editorSettings.showScatteringTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showScatteringTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showScatteringTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "SCATTERING", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideScattering);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
			EditorGUILayout.Space();
            if (m_Target.editorSettings.showScatteringTab)
            {
                //Molecular Density.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Air Density", GUILayout.Width(m_labelWidth));
                m_Target.scattering.N= EditorGUILayout.Slider(m_Target.scattering.N, 0.01f, 3.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.N = 2.545f;
				}
                EditorGUILayout.EndHorizontal();


                //Kr.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Kr", GUILayout.Width(m_labelWidth));
                m_Target.scattering.kr = EditorGUILayout.Slider(m_Target.scattering.kr, 0.0f, 25.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.kr = 8.4f;
				}
                EditorGUILayout.EndHorizontal();
                //Km.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Km", GUILayout.Width(m_labelWidth));
                m_Target.scattering.km = EditorGUILayout.Slider(m_Target.scattering.km, 0.0f, 5.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.km = 1.25f;
				}
                EditorGUILayout.EndHorizontal();


                //Wavelength R.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Wavelength R", GUILayout.Width(m_labelWidth));
                m_Target.scattering.lambda.x = EditorGUILayout.Slider(m_Target.scattering.lambda.x, 0, 1500);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.lambda.x = 680.0f;
				}
                EditorGUILayout.EndHorizontal();
                //Wavelength G.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Wavelength G", GUILayout.Width(m_labelWidth));
                m_Target.scattering.lambda.y = EditorGUILayout.Slider(m_Target.scattering.lambda.y, 0, 1500);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.lambda.y = 550.0f;
				}
                EditorGUILayout.EndHorizontal();
                //Wavelength B.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Wavelength B", GUILayout.Width(m_labelWidth));
                m_Target.scattering.lambda.z = EditorGUILayout.Slider(m_Target.scattering.lambda.z, 0, 1500);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.scattering.lambda.z = 440.0f;
				}
                EditorGUILayout.EndHorizontal();
				EditorGUILayout.HelpBox ("The above properties will not take effect if the Wavelength is set to Precomputed.", MessageType.Info);
                EditorGUILayout.Space();

                //Rayleigh Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rayleigh", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.rayleighCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_rayleighCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 5), GUIContent.none);
                }
                if (m_Target.scattering.rayleighCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_rayleighCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                if (m_Target.scattering.rayleighCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_rayleighCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                m_Target.scattering.rayleighCurveIndex = EditorGUILayout.Popup(m_Target.scattering.rayleighCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.rayleighCurveIndex == 0) m_rayleighCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.scattering.rayleighCurveIndex == 1) m_rayleighCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.scattering.rayleighCurveIndex == 2) m_rayleighCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Mie Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mie", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.mieCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_mieCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 30), GUIContent.none);
                }
                if (m_Target.scattering.mieCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_mieCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 30), GUIContent.none);
                }
                if (m_Target.scattering.mieCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_mieCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 30), GUIContent.none);
                }
                m_Target.scattering.mieCurveIndex = EditorGUILayout.Popup(m_Target.scattering.mieCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.mieCurveIndex == 0) m_mieCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.scattering.mieCurveIndex == 1) m_mieCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.scattering.mieCurveIndex == 2) m_mieCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Night Intensity Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Night Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.nightIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_nightIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 5), GUIContent.none);
                }
                if (m_Target.scattering.nightIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_nightIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                if (m_Target.scattering.nightIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_nightIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                m_Target.scattering.nightIntensityCurveIndex = EditorGUILayout.Popup(m_Target.scattering.nightIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.nightIntensityCurveIndex == 0) m_nightIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.5f, 24.0f, 1.5f);
					if (m_Target.scattering.nightIntensityCurveIndex == 1) m_nightIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.5f, 1.0f, 1.5f);
					if (m_Target.scattering.nightIntensityCurveIndex == 2) m_nightIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.5f, 1.0f, 1.5f);
				}
                EditorGUILayout.EndHorizontal();

                //Rayleigh Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rayleigh Color", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.rayleighGradientIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_rayleighGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.scattering.rayleighGradientIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_rayleighGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.scattering.rayleighGradientIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_rayleighGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.scattering.rayleighGradientIndex = EditorGUILayout.Popup(m_Target.scattering.rayleighGradientIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Mie Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mie Color", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.mieGradientIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_mieGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.scattering.mieGradientIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_mieGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.scattering.mieGradientIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_mieGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.scattering.mieGradientIndex = EditorGUILayout.Popup(m_Target.scattering.mieGradientIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					
				}
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();

                //Sun Intensity Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sun Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.sunIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_sunIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
                }
                if (m_Target.scattering.sunIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_sunIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                if (m_Target.scattering.sunIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_sunIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                m_Target.scattering.sunIntensityCurveIndex = EditorGUILayout.Popup(m_Target.scattering.sunIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.sunIntensityCurveIndex == 0) m_sunIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.25f, 24.0f, 0.25f);
					if (m_Target.scattering.sunIntensityCurveIndex == 1) m_sunIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
					if (m_Target.scattering.sunIntensityCurveIndex == 2) m_sunIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
				}
                EditorGUILayout.EndHorizontal();

                //Sun Disk Intensity Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sun Disk", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.sunDiskIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_sunDiskIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 5), GUIContent.none);
                }
                if (m_Target.scattering.sunDiskIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_sunDiskIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                if (m_Target.scattering.sunDiskIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_sunDiskIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                m_Target.scattering.sunDiskIntensityCurveIndex = EditorGUILayout.Popup(m_Target.scattering.sunDiskIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.sunDiskIntensityCurveIndex == 0) m_sunDiskIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 3.0f, 24.0f, 3.0f);
					if (m_Target.scattering.sunDiskIntensityCurveIndex == 1) m_sunDiskIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 3.0f, 1.0f, 3.0f);
					if (m_Target.scattering.sunDiskIntensityCurveIndex == 2) m_sunDiskIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 3.0f, 1.0f, 3.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Exposure Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Exposure", GUILayout.Width(m_labelWidth));
                if (m_Target.scattering.exposureCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_exposureCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 10), GUIContent.none);
                }
                if (m_Target.scattering.exposureCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_exposureCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 10), GUIContent.none);
                }
                if (m_Target.scattering.exposureCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_exposureCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 10), GUIContent.none);
                }
                m_Target.scattering.exposureCurveIndex = EditorGUILayout.Popup(m_Target.scattering.exposureCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.scattering.exposureCurveIndex == 0) m_exposureCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.5f, 24.0f, 1.5f);
					if (m_Target.scattering.exposureCurveIndex == 1) m_exposureCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.5f, 1.0f, 1.5f);
					if (m_Target.scattering.exposureCurveIndex == 2) m_exposureCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.5f, 1.0f, 1.5f);
				}
                EditorGUILayout.EndHorizontal();

				//Fast Index.
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.scattering.FastIndexesChange ();
				}
				m_Target.scattering.fastIndexes = EditorGUILayout.Popup(m_Target.scattering.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();
            }



            //Deep Space Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showDeepSpaceTab = !m_Target.editorSettings.showDeepSpaceTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showDeepSpaceTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showDeepSpaceTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "DEEP SPACE", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideDeepSpace);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            EditorGUILayout.Space();
            if (m_Target.editorSettings.showDeepSpaceTab)
            {
                GUILayout.Label("Moon:");
                //Moon Disck Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Disck Color", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.moonColorGradientIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_moonGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.deepSpace.moonColorGradientIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_moonGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.deepSpace.moonColorGradientIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_moonGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.deepSpace.moonColorGradientIndex = EditorGUILayout.Popup(m_Target.deepSpace.moonColorGradientIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Moon Bright Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bright Color", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.moonBrightColorGradientIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_moonBrightGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.deepSpace.moonBrightColorGradientIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_moonBrightGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.deepSpace.moonBrightColorGradientIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_moonBrightGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.deepSpace.moonBrightColorGradientIndex = EditorGUILayout.Popup(m_Target.deepSpace.moonBrightColorGradientIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Moon Bright Range Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bright Range", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.moonBrightRangeCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_moonBrightRangeCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 10, 24, 200), GUIContent.none);
                }
                if (m_Target.deepSpace.moonBrightRangeCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_moonBrightRangeCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 10, 2, 200), GUIContent.none);
                }
                if (m_Target.deepSpace.moonBrightRangeCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_moonBrightRangeCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 10, 2, 200), GUIContent.none);
                }
                m_Target.deepSpace.moonBrightRangeCurveIndex = EditorGUILayout.Popup(m_Target.deepSpace.moonBrightRangeCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.deepSpace.moonBrightRangeCurveIndex == 0) m_moonBrightRangeCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 50.0f, 24.0f, 50.0f);
					if (m_Target.deepSpace.moonBrightRangeCurveIndex == 1) m_moonBrightRangeCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 50.0f, 1.0f, 50.0f);
					if (m_Target.deepSpace.moonBrightRangeCurveIndex == 2) m_moonBrightRangeCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 50.0f, 1.0f, 50.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Moon Emission Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Emission", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.moonEmissionCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_moonEmissionCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0.25f, 24, 9.75f), GUIContent.none);
                }
                if (m_Target.deepSpace.moonEmissionCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_moonEmissionCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0.25f, 2, 9.75f), GUIContent.none);
                }
                if (m_Target.deepSpace.moonEmissionCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_moonEmissionCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0.25f, 2, 9.75f), GUIContent.none);
                }
                m_Target.deepSpace.moonEmissionCurveIndex = EditorGUILayout.Popup(m_Target.deepSpace.moonEmissionCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.deepSpace.moonEmissionCurveIndex == 0) m_moonEmissionCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 10.0f, 24.0f, 10.0f);
					if (m_Target.deepSpace.moonEmissionCurveIndex == 1) m_moonEmissionCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 10.0f, 1.0f, 10.0f);
					if (m_Target.deepSpace.moonEmissionCurveIndex == 2) m_moonEmissionCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 10.0f, 1.0f, 10.0f);
				}
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                GUILayout.Label("Stars:");
                //Stars Scintillation.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Scintillation", GUILayout.Width(m_labelWidth));
                m_Target.deepSpace.starsScintillation = EditorGUILayout.Slider(m_Target.deepSpace.starsScintillation, 0, 20);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.deepSpace.starsScintillation = 5.0f;
				}
                EditorGUILayout.EndHorizontal();
                //Starfield Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Starfield", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.starfieldIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_starfieldCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 5), GUIContent.none);
                }
                if (m_Target.deepSpace.starfieldIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_starfieldCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                if (m_Target.deepSpace.starfieldIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_starfieldCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 5), GUIContent.none);
                }
                m_Target.deepSpace.starfieldIntensityCurveIndex = EditorGUILayout.Popup(m_Target.deepSpace.starfieldIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.deepSpace.starfieldIntensityCurveIndex == 0) m_starfieldCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.deepSpace.starfieldIntensityCurveIndex == 1) m_starfieldCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.deepSpace.starfieldIntensityCurveIndex == 2) m_starfieldCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Milky Way Curve.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Milky Way", GUILayout.Width(m_labelWidth));
                if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_milkyWayCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
                }
                if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_milkyWayCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_milkyWayCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                m_Target.deepSpace.milkyWayIntensityCurveIndex = EditorGUILayout.Popup(m_Target.deepSpace.milkyWayIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 0) m_milkyWayCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 1) m_milkyWayCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.deepSpace.milkyWayIntensityCurveIndex == 2) m_milkyWayCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
                EditorGUILayout.EndHorizontal();

				//Fast Index.
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.deepSpace.FastIndexesChange ();
				}
				m_Target.deepSpace.fastIndexes = EditorGUILayout.Popup(m_Target.deepSpace.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();
            }



            //Fog Scattering Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showFogScatteringTab = !m_Target.editorSettings.showFogScatteringTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showFogScatteringTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showFogScatteringTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "FOG SCATTERING", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideFogScattering);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            EditorGUILayout.Space();
            if (m_Target.editorSettings.showFogScatteringTab)
            {
				//Fog Scale.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Scale", GUILayout.Width(m_labelWidth));
				m_Target.fogScattering.fogScale = EditorGUILayout.Slider(m_Target.fogScattering.fogScale , 0.75f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.fogScattering.fogScale  = 1.0f;
				}
				EditorGUILayout.EndHorizontal();

                //Fog Blend.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Fog Blend", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.fogBlendCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_fogBlendCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 2), GUIContent.none);
                }
                if (m_Target.fogScattering.fogBlendCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_fogBlendCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 2), GUIContent.none);
                }
                if (m_Target.fogScattering.fogBlendCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_fogBlendCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 2), GUIContent.none);
                }
                m_Target.fogScattering.fogBlendCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.fogBlendCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.fogBlendCurveIndex == 0) m_fogBlendCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.25f, 24.0f, 0.25f);
					if (m_Target.fogScattering.fogBlendCurveIndex == 1) m_fogBlendCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
					if (m_Target.fogScattering.fogBlendCurveIndex == 2) m_fogBlendCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
				}
                EditorGUILayout.EndHorizontal();

                //Fog Distance.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Fog Distance", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.fogDistanceCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_fogDistanceCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 25000), GUIContent.none);
                }
                if (m_Target.fogScattering.fogDistanceCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_fogDistanceCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 25000), GUIContent.none);
                }
                if (m_Target.fogScattering.fogDistanceCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_fogDistanceCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 25000), GUIContent.none);
                }
                m_Target.fogScattering.fogDistanceCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.fogDistanceCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.fogDistanceCurveIndex == 0) m_fogDistanceCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 5000.0f, 24.0f, 5000.0f);
					if (m_Target.fogScattering.fogDistanceCurveIndex == 1) m_fogDistanceCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 5000.0f, 1.0f, 5000.0f);
					if (m_Target.fogScattering.fogDistanceCurveIndex == 2) m_fogDistanceCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 5000.0f, 1.0f, 5000.0f);
				}
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                //Height Fog Blend.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Height Blend", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.heightFogBlendCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_heightFogBlendCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 2), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogBlendCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_heightFogBlendCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 2), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogBlendCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_heightFogBlendCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 2), GUIContent.none);
                }
                m_Target.fogScattering.heightFogBlendCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.heightFogBlendCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.heightFogBlendCurveIndex == 0) m_heightFogBlendCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.fogScattering.heightFogBlendCurveIndex == 1) m_heightFogBlendCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.fogScattering.heightFogBlendCurveIndex == 2) m_heightFogBlendCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Height Fog Density.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Height Density", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.heightFogDensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_heightFogDensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogDensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_heightFogDensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogDensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_heightFogDensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                m_Target.fogScattering.heightFogDensityCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.heightFogDensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.heightFogDensityCurveIndex == 0) m_heightFogDensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.5f, 24.0f, 0.5f);
					if (m_Target.fogScattering.heightFogDensityCurveIndex == 1) m_heightFogDensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.5f, 1.0f, 0.5f);
					if (m_Target.fogScattering.heightFogDensityCurveIndex == 2) m_heightFogDensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.5f, 1.0f, 0.5f);
				}
                EditorGUILayout.EndHorizontal();

                //Height Fog Distance.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Height Distance", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.heightFogDistanceCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_heightFogDistanceCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogDistanceCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_heightFogDistanceCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogDistanceCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_heightFogDistanceCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1500), GUIContent.none);
                }
                m_Target.fogScattering.heightFogDistanceCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.heightFogDistanceCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.heightFogDistanceCurveIndex == 0) m_heightFogDistanceCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 500.0f, 24.0f, 500.0f);
					if (m_Target.fogScattering.heightFogDistanceCurveIndex == 1) m_heightFogDistanceCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 500.0f, 1.0f, 500.0f);
					if (m_Target.fogScattering.heightFogDistanceCurveIndex == 2) m_heightFogDistanceCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 500.0f, 1.0f, 500.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Height Fog Start.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Height Start", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.heightFogStartCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_heightFogStartCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogStartCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_heightFogStartCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogStartCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_heightFogStartCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 500), GUIContent.none);
                }
                m_Target.fogScattering.heightFogStartCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.heightFogStartCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.heightFogStartCurveIndex == 0) m_heightFogStartCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.fogScattering.heightFogStartCurveIndex == 1) m_heightFogStartCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.fogScattering.heightFogStartCurveIndex == 2) m_heightFogStartCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Height Fog End.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Height End", GUILayout.Width(m_labelWidth));
                if (m_Target.fogScattering.heightFogEndCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_heightFogEndCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 2500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogEndCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_heightFogEndCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 2500), GUIContent.none);
                }
                if (m_Target.fogScattering.heightFogEndCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_heightFogEndCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 2500), GUIContent.none);
                }
                m_Target.fogScattering.heightFogEndCurveIndex = EditorGUILayout.Popup(m_Target.fogScattering.heightFogEndCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.fogScattering.heightFogEndCurveIndex == 0) m_heightFogEndCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 100.0f, 24.0f, 100.0f);
					if (m_Target.fogScattering.heightFogEndCurveIndex == 1) m_heightFogEndCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 100.0f, 1.0f, 100.0f);
					if (m_Target.fogScattering.heightFogEndCurveIndex == 2) m_heightFogEndCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 100.0f, 1.0f, 100.0f);
				}
                EditorGUILayout.EndHorizontal();

				//Fast Index.
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.fogScattering.FastIndexesChange ();
				}
				m_Target.fogScattering.fastIndexes = EditorGUILayout.Popup(m_Target.fogScattering.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();
            }



            //Clouds Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showCloudsTab = !m_Target.editorSettings.showCloudsTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showCloudsTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showCloudsTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "CLOUDS", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideClouds);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            EditorGUILayout.Space();
            if (m_Target.editorSettings.showCloudsTab)
            {
                GUILayout.Label("Dynamic Clouds:");
                //Altitude.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Altitude", GUILayout.Width(m_labelWidth));
                m_Target.clouds.dynamicCloudLayer1Altitude = EditorGUILayout.Slider(m_Target.clouds.dynamicCloudLayer1Altitude, 0, 20);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.clouds.dynamicCloudLayer1Altitude = 7.5f;
				}
                EditorGUILayout.EndHorizontal();
                //Direction.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Direction", GUILayout.Width(m_labelWidth));
                m_Target.clouds.dynamicCloudLayer1Direction = EditorGUILayout.Slider(m_Target.clouds.dynamicCloudLayer1Direction, -3, 3);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.clouds.dynamicCloudLayer1Direction = 1.0f;
				}
                EditorGUILayout.EndHorizontal();
                //Speed.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Speed", GUILayout.Width(m_labelWidth));
                m_Target.clouds.dynamicCloudLayer1Speed = EditorGUILayout.Slider(m_Target.clouds.dynamicCloudLayer1Speed, 0, 1);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.clouds.dynamicCloudLayer1Speed = 0.1f;
				}
                EditorGUILayout.EndHorizontal();

                //Density.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Density", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
                }
                m_Target.clouds.dynamicCloudLayer1DensityCurveIndex = EditorGUILayout.Popup(m_Target.clouds.dynamicCloudLayer1DensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 0) m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.7f, 24.0f, 0.7f);
					if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 1) m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.7f, 1.0f, 0.7f);
					if (m_Target.clouds.dynamicCloudLayer1DensityCurveIndex == 2) m_dynamicCloudLayer1DensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.7f, 1.0f, 0.7f);
				}
                EditorGUILayout.EndHorizontal();

                //Color1.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color1", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.dynamicCloudLayer1GradientColor1Index == 0)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor1.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1GradientColor1Index == 1)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor1.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1GradientColor1Index == 2)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor1.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.clouds.dynamicCloudLayer1GradientColor1Index = EditorGUILayout.Popup(m_Target.clouds.dynamicCloudLayer1GradientColor1Index, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					
				}
                EditorGUILayout.EndHorizontal();

                //Color2.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color2", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.dynamicCloudLayer1GradientColor2Index == 0)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor2.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1GradientColor2Index == 1)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor2.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.clouds.dynamicCloudLayer1GradientColor2Index == 2)
                {
                    EditorGUILayout.PropertyField(m_dynamicCloudLayer1GradientColor2.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.clouds.dynamicCloudLayer1GradientColor2Index = EditorGUILayout.Popup(m_Target.clouds.dynamicCloudLayer1GradientColor2Index, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Static Clouds//
                EditorGUILayout.Space();
                GUILayout.Label("Static Clouds:");

                // Static Cloud Texture.
                GUI.color = m_col3;
                if (!m_Target.clouds.staticCloudTexture)
                {
                    GUI.color = m_col4;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Texture", GUILayout.Width(m_labelWidth));
                m_Target.clouds.staticCloudTexture = (Texture2D)EditorGUILayout.ObjectField(m_Target.clouds.staticCloudTexture, typeof(Texture2D), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

                // Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.staticCloudColorIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_staticCloudColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudColorIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_staticCloudColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudColorIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_staticCloudColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.clouds.staticCloudColorIndex = EditorGUILayout.Popup(m_Target.clouds.staticCloudColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
                if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {

                }
                EditorGUILayout.EndHorizontal();

                // Scattering.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Scattering", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.staticCloudScatteringCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_staticCloudScatteringCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0.0f, 0.0f, 24.0f, 1.5f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudScatteringCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_staticCloudScatteringCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1.5f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudScatteringCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_staticCloudScatteringCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1.5f), GUIContent.none);
                }
                m_Target.clouds.staticCloudScatteringCurveIndex = EditorGUILayout.Popup(m_Target.clouds.staticCloudScatteringCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {
                    if (m_Target.clouds.staticCloudScatteringCurveIndex == 0) m_staticCloudScatteringCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
                    if (m_Target.clouds.staticCloudScatteringCurveIndex == 1) m_staticCloudScatteringCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
                    if (m_Target.clouds.staticCloudScatteringCurveIndex == 2) m_staticCloudScatteringCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
                }
                EditorGUILayout.EndHorizontal();

                // Cloud Extinction.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Extinction", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.staticCloudExtinctionCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_staticCloudExtinctionCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0.0f, 0.0f, 24.0f, 1.0f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudExtinctionCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_staticCloudExtinctionCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1.0f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudExtinctionCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_staticCloudExtinctionCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1.0f), GUIContent.none);
                }
                m_Target.clouds.staticCloudExtinctionCurveIndex = EditorGUILayout.Popup(m_Target.clouds.staticCloudExtinctionCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {
                    if (m_Target.clouds.staticCloudExtinctionCurveIndex == 0) m_staticCloudExtinctionCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.25f, 24.0f, 0.25f);
                    if (m_Target.clouds.staticCloudExtinctionCurveIndex == 1) m_staticCloudExtinctionCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
                    if (m_Target.clouds.staticCloudExtinctionCurveIndex == 2) m_staticCloudExtinctionCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.25f, 1.0f, 0.25f);
                }
                EditorGUILayout.EndHorizontal();

                // Cloud Power.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Power", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.staticCloudPowerCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_staticCloudPowerCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0.0f, 1.8f, 24.0f, 2.4f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudPowerCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_staticCloudPowerCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 1.8f, 2, 2.4f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudPowerCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_staticCloudPowerCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 1.8f, 2, 2.4f), GUIContent.none);
                }
                m_Target.clouds.staticCloudPowerCurveIndex = EditorGUILayout.Popup(m_Target.clouds.staticCloudPowerCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {
                    if (m_Target.clouds.staticCloudPowerCurveIndex == 0) m_staticCloudPowerCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 2.2f, 24.0f, 2.2f);
                    if (m_Target.clouds.staticCloudPowerCurveIndex == 1) m_staticCloudPowerCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 2.2f, 1.0f, 2.2f);
                    if (m_Target.clouds.staticCloudPowerCurveIndex == 2) m_staticCloudPowerCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 2.2f, 1.0f, 2.2f);
                }
                EditorGUILayout.EndHorizontal();

                // Cloud Intensity.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.clouds.staticCloudIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_staticCloudIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0.0f, 0.0f, 24.0f, 2.0f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_staticCloudIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0.0f, 2, 2.0f), GUIContent.none);
                }
                if (m_Target.clouds.staticCloudIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_staticCloudIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0.0f, 2, 2.0f), GUIContent.none);
                }
                m_Target.clouds.staticCloudIntensityCurveIndex = EditorGUILayout.Popup(m_Target.clouds.staticCloudIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {
                    if (m_Target.clouds.staticCloudIntensityCurveIndex == 0) m_staticCloudIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
                    if (m_Target.clouds.staticCloudIntensityCurveIndex == 1) m_staticCloudIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
                    if (m_Target.clouds.staticCloudIntensityCurveIndex == 2) m_staticCloudIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
                }
                EditorGUILayout.EndHorizontal();

                // Rotation Speed.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Rotation Speed", GUILayout.Width(m_labelWidth));
                m_Target.clouds.staticCloudRotationSpeed = EditorGUILayout.Slider(m_Target.clouds.staticCloudRotationSpeed, -0.01f, 0.01f);
                if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
                {
                    m_Target.clouds.staticCloudRotationSpeed = 0.0f;
                }
                EditorGUILayout.EndHorizontal();

                //Fast Index.
                EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.clouds.FastIndexesChange ();
				}
				m_Target.clouds.fastIndexes = EditorGUILayout.Popup(m_Target.clouds.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();
            }



            //Lighting Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showLightingTab = !m_Target.editorSettings.showLightingTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showLightingTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showLightingTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "LIGHTING", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideLighting);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            EditorGUILayout.Space();
            if (m_Target.editorSettings.showLightingTab)
            {
                GUILayout.Label("Directional Light:");
                // Directional Light Intensity.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.directionalLightIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_directionalLightIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 8), GUIContent.none);
                }
                if (m_Target.lighting.directionalLightIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_directionalLightIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                if (m_Target.lighting.directionalLightIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_directionalLightIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                m_Target.lighting.directionalLightIntensityCurveIndex = EditorGUILayout.Popup(m_Target.lighting.directionalLightIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.lighting.directionalLightIntensityCurveIndex == 0) m_directionalLightIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.lighting.directionalLightIntensityCurveIndex == 1) m_directionalLightIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.lighting.directionalLightIntensityCurveIndex == 2) m_directionalLightIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

                // Directional Light Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.directionalLightGradientColorIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_directionalLightGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.lighting.directionalLightGradientColorIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_directionalLightGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.lighting.directionalLightGradientColorIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_directionalLightGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.lighting.directionalLightGradientColorIndex = EditorGUILayout.Popup(m_Target.lighting.directionalLightGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                m_bgRect = EditorGUILayout.GetControlRect();
                GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, -17).x, GUILayoutUtility.GetRect(m_bgRect.width, 1).y+3, m_bgRect.width, 1), m_tabTex);
                EditorGUILayout.Space();

                GUILayout.Label("Ambient:");
                //Ambient Intensity.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.ambientIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_ambientIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 8), GUIContent.none);
                }
                if (m_Target.lighting.ambientIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_ambientIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                if (m_Target.lighting.ambientIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_ambientIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                m_Target.lighting.ambientIntensityCurveIndex = EditorGUILayout.Popup(m_Target.lighting.ambientIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.lighting.ambientIntensityCurveIndex == 0) m_ambientIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.lighting.ambientIntensityCurveIndex == 1) m_ambientIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.lighting.ambientIntensityCurveIndex == 2) m_ambientIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

                //Ambient/Sky Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Ambient Color", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.ambientSkyGradientColorIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_ambientSkyGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.lighting.ambientSkyGradientColorIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_ambientSkyGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.lighting.ambientSkyGradientColorIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_ambientSkyGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.lighting.ambientSkyGradientColorIndex = EditorGUILayout.Popup(m_Target.lighting.ambientSkyGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Equator Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Equator Color", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.equatorGradientColorIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_equatorGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.lighting.equatorGradientColorIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_equatorGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.lighting.equatorGradientColorIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_equatorGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.lighting.equatorGradientColorIndex = EditorGUILayout.Popup(m_Target.lighting.equatorGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                //Ground Color.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Ground Color", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.groundGradientColorIndex == 0)
                {
                    EditorGUILayout.PropertyField(m_groundGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
                }
                if (m_Target.lighting.groundGradientColorIndex == 1)
                {
                    EditorGUILayout.PropertyField(m_groundGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
                }
                if (m_Target.lighting.groundGradientColorIndex == 2)
                {
                    EditorGUILayout.PropertyField(m_groundGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
                }
                m_Target.lighting.groundGradientColorIndex = EditorGUILayout.Popup(m_Target.lighting.groundGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
                EditorGUILayout.EndHorizontal();

                m_bgRect = EditorGUILayout.GetControlRect();
                GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, -17).x, GUILayoutUtility.GetRect(m_bgRect.width, 1).y + 3, m_bgRect.width, 1), m_tabTex);
                EditorGUILayout.Space();

                GUILayout.Label("Reflection:");
                //Reflection Intensity.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Intensity", GUILayout.Width(m_labelWidth));
                if (m_Target.lighting.reflectionIntensityCurveIndex == 0)
                {
                    m_curveColor = Color.green;
                    EditorGUILayout.CurveField(m_reflectionIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 8), GUIContent.none);
                }
                if (m_Target.lighting.reflectionIntensityCurveIndex == 1)
                {
                    m_curveColor = Color.yellow;
                    EditorGUILayout.CurveField(m_reflectionIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                if (m_Target.lighting.reflectionIntensityCurveIndex == 2)
                {
                    m_curveColor = Color.cyan;
                    EditorGUILayout.CurveField(m_reflectionIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 8), GUIContent.none);
                }
                m_Target.lighting.reflectionIntensityCurveIndex = EditorGUILayout.Popup(m_Target.lighting.reflectionIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.lighting.reflectionIntensityCurveIndex == 0) m_reflectionIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 1.0f, 24.0f, 1.0f);
					if (m_Target.lighting.reflectionIntensityCurveIndex == 1) m_reflectionIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
					if (m_Target.lighting.reflectionIntensityCurveIndex == 2) m_reflectionIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 1.0f, 1.0f, 1.0f);
				}
                EditorGUILayout.EndHorizontal();

				//Fast Index.
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.lighting.FastIndexesChange ();
				}
				m_Target.lighting.fastIndexes = EditorGUILayout.Popup(m_Target.lighting.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();
            }



            //Weather Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showWeatherTab = !m_Target.editorSettings.showWeatherTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showWeatherTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showWeatherTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "WEATHER", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideWeather);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            EditorGUILayout.Space();
            if (m_Target.editorSettings.showWeatherTab)
            {
				//Rain Color.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Color", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherRainGradientColorIndex == 0)
				{
					EditorGUILayout.PropertyField(m_weatherRainGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
				}
				if (m_Target.weather.weatherRainGradientColorIndex == 1)
				{
					EditorGUILayout.PropertyField(m_weatherRainGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
				}
				if (m_Target.weather.weatherRainGradientColorIndex == 2)
				{
					EditorGUILayout.PropertyField(m_weatherRainGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
				}
				m_Target.weather.weatherRainGradientColorIndex = EditorGUILayout.Popup(m_Target.weather.weatherRainGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
				EditorGUILayout.EndHorizontal();

				//Snow Color.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Snow Color", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherSnowGradientColorIndex == 0)
				{
					EditorGUILayout.PropertyField(m_weatherSnowGradientColor.GetArrayElementAtIndex(0), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowGradientColorIndex == 1)
				{
					EditorGUILayout.PropertyField(m_weatherSnowGradientColor.GetArrayElementAtIndex(1), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowGradientColorIndex == 2)
				{
					EditorGUILayout.PropertyField(m_weatherSnowGradientColor.GetArrayElementAtIndex(2), GUIContent.none);
				}
				m_Target.weather.weatherSnowGradientColorIndex = EditorGUILayout.Popup(m_Target.weather.weatherSnowGradientColorIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
				EditorGUILayout.EndHorizontal();

				//Rain Intensity.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Intensity", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherRainIntensityCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherRainIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherRainIntensityCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherRainIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherRainIntensityCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherRainIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherRainIntensityCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherRainIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherRainIntensityCurveIndex == 0) m_weatherRainIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherRainIntensityCurveIndex == 1) m_weatherRainIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherRainIntensityCurveIndex == 2) m_weatherRainIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				//Snow Intensity.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Snow Intensity", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherSnowIntensityCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherSnowIntensityCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowIntensityCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherSnowIntensityCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowIntensityCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherSnowIntensityCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherSnowIntensityCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherSnowIntensityCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherSnowIntensityCurveIndex == 0) m_weatherSnowIntensityCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherSnowIntensityCurveIndex == 1) m_weatherSnowIntensityCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherSnowIntensityCurveIndex == 2) m_weatherSnowIntensityCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				//Wind Speed.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Speed", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherWindSpeedCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherWindSpeedCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherWindSpeedCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherWindSpeedCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherWindSpeedCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherWindSpeedCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherWindSpeedCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherWindSpeedCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherWindSpeedCurveIndex == 0) m_weatherWindSpeedCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.01f, 24.0f, 0.01f);
					if (m_Target.weather.weatherWindSpeedCurveIndex == 1) m_weatherWindSpeedCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.01f, 1.0f, 0.01f);
					if (m_Target.weather.weatherWindSpeedCurveIndex == 2) m_weatherWindSpeedCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.01f, 1.0f, 0.01f);
				}
				EditorGUILayout.EndHorizontal();

				//Wind Direction.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Direction", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherWindDirectionCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherWindDirectionCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, -1, 24, 2), GUIContent.none);
				}
				if (m_Target.weather.weatherWindDirectionCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherWindDirectionCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, -1, 2, 2), GUIContent.none);
				}
				if (m_Target.weather.weatherWindDirectionCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherWindDirectionCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, -1, 2, 2), GUIContent.none);
				}
				m_Target.weather.weatherWindDirectionCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherWindDirectionCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherWindDirectionCurveIndex == 0) m_weatherWindDirectionCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherWindDirectionCurveIndex == 1) m_weatherWindDirectionCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherWindDirectionCurveIndex == 2) m_weatherWindDirectionCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
				GUILayout.Label("Sounds Volume:");

				//Rain Light.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Light", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherRainLightVolume = EditorGUILayout.Slider(m_Target.weather.weatherRainLightVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherRainLightVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				//Rain Medium.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Medium", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherRainMediumVolume = EditorGUILayout.Slider(m_Target.weather.weatherRainMediumVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherRainMediumVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				//Rain Heavy.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Heavy", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherRainHeavyVolume = EditorGUILayout.Slider(m_Target.weather.weatherRainHeavyVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherRainHeavyVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				//Wind Light.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Light", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherWindLightVolume = EditorGUILayout.Slider(m_Target.weather.weatherWindLightVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherWindLightVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				//Wind Medium.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Medium", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherWindMediumVolume = EditorGUILayout.Slider(m_Target.weather.weatherWindMediumVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherWindMediumVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				//Wind Heavy.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Heavy", GUILayout.Width(m_labelWidth));
				m_Target.weather.weatherWindHeavyVolume = EditorGUILayout.Slider(m_Target.weather.weatherWindHeavyVolume, 0.0f, 1.0f);
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					m_Target.weather.weatherWindHeavyVolume = 0.0f;
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.Space();
				GUILayout.Label("Third-Party Asset Compatibility:");

				//Wetness.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wetness", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherWetnessCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherWetnessCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherWetnessCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherWetnessCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherWetnessCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherWetnessCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherWetnessCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherWetnessCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherWetnessCurveIndex == 0) m_weatherWetnessCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherWetnessCurveIndex == 1) m_weatherWetnessCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherWetnessCurveIndex == 2) m_weatherWetnessCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				//Snow Amount.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Snow Amount", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherSnowAmountCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherSnowAmountCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowAmountCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherSnowAmountCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherSnowAmountCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherSnowAmountCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherSnowAmountCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherSnowAmountCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherSnowAmountCurveIndex == 0) m_weatherSnowAmountCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherSnowAmountCurveIndex == 1) m_weatherSnowAmountCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherSnowAmountCurveIndex == 2) m_weatherSnowAmountCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				//Covarage.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Covarage", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherCovarageCurveIndex == 0)
				{
					m_curveColor = Color.green;
					EditorGUILayout.CurveField(m_weatherCovarageCurve.GetArrayElementAtIndex(0), m_curveColor, new Rect(0, 0, 24, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherCovarageCurveIndex == 1)
				{
					m_curveColor = Color.yellow;
					EditorGUILayout.CurveField(m_weatherCovarageCurve.GetArrayElementAtIndex(1), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				if (m_Target.weather.weatherCovarageCurveIndex == 2)
				{
					m_curveColor = Color.cyan;
					EditorGUILayout.CurveField(m_weatherCovarageCurve.GetArrayElementAtIndex(2), m_curveColor, new Rect(-1, 0, 2, 1), GUIContent.none);
				}
				m_Target.weather.weatherCovarageCurveIndex = EditorGUILayout.Popup(m_Target.weather.weatherCovarageCurveIndex, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("R", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{
					if (m_Target.weather.weatherCovarageCurveIndex == 0) m_weatherCovarageCurve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.0f, 24.0f, 0.0f);
					if (m_Target.weather.weatherCovarageCurveIndex == 1) m_weatherCovarageCurve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
					if (m_Target.weather.weatherCovarageCurveIndex == 2) m_weatherCovarageCurve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.0f, 1.0f, 0.0f);
				}
				EditorGUILayout.EndHorizontal();

				//Output Color1.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Output Color1", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherOutputColor1Index == 0)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor1GradientColor.GetArrayElementAtIndex(0), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor1Index == 1)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor1GradientColor.GetArrayElementAtIndex(1), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor1Index == 2)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor1GradientColor.GetArrayElementAtIndex(2), GUIContent.none);
				}
				m_Target.weather.weatherOutputColor1Index = EditorGUILayout.Popup(m_Target.weather.weatherOutputColor1Index, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
				EditorGUILayout.EndHorizontal();

				//Output Color2.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Output Color2", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherOutputColor2Index == 0)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor2GradientColor.GetArrayElementAtIndex(0), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor2Index == 1)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor2GradientColor.GetArrayElementAtIndex(1), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor2Index == 2)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor2GradientColor.GetArrayElementAtIndex(2), GUIContent.none);
				}
				m_Target.weather.weatherOutputColor2Index = EditorGUILayout.Popup(m_Target.weather.weatherOutputColor2Index, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
				EditorGUILayout.EndHorizontal();

				//Output Color3.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Output Color3", GUILayout.Width(m_labelWidth));
				if (m_Target.weather.weatherOutputColor3Index == 0)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor3GradientColor.GetArrayElementAtIndex(0), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor3Index == 1)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor3GradientColor.GetArrayElementAtIndex(1), GUIContent.none);
				}
				if (m_Target.weather.weatherOutputColor3Index == 2)
				{
					EditorGUILayout.PropertyField(m_weatherOutputColor3GradientColor.GetArrayElementAtIndex(2), GUIContent.none);
				}
				m_Target.weather.weatherOutputColor3Index = EditorGUILayout.Popup(m_Target.weather.weatherOutputColor3Index, m_curveMode, GUILayout.Width(62), GUILayout.Height(15));
				if (GUILayout.Button("", EditorStyles.miniButton, GUILayout.Width(18), GUILayout.Height(15)))
				{

				}
				EditorGUILayout.EndHorizontal();

				//Fast Index.
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal("Box");
				if (GUILayout.Button("Chage all indexes to", EditorStyles.miniButton, GUILayout.Height(15)))
				{
					m_Target.weather.FastIndexesChange ();
				}
				m_Target.weather.fastIndexes = EditorGUILayout.Popup(m_Target.weather.fastIndexes, m_curveMode, GUILayout.Width(80), GUILayout.Height(15));
				EditorGUILayout.EndHorizontal();

				GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 0).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y, m_bgRect.width, 1), m_tabTex);
            }



            //Refresh the Inspector.
            //-------------------------------------------------------------------------------------------------------
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
            EditorUtility.SetDirty(target);
        }
    }
}