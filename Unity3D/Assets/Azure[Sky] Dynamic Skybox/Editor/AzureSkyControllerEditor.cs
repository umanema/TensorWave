using UnityEngine;
using UnityEngine.AzureSky;
using System;
using UnityEditorInternal;

namespace UnityEditor.AzureSky
{
    [CustomEditor(typeof(AzureSkyController))]
    public class AzureSkyControllerEditor : Editor
    {
        AzureSkyController m_Target;
        private string m_dayProfileName = "None Day Profile";
		private int m_dayProfileInfo = 0;

        //Popups.
        private string[] m_timeMode = new string[] { "Simple", "Realistic" };
        private string[] m_repeatMode = new string[] { "Off", "By Day", "By Month", "By Year" };
        private string[] m_particleMode = new string[] { "Off", "On" };
        private string[] m_cloudMode = new string[] { "Off", "Dynamic 2D", "Static 2D" };
		//private string[] m_skyMode = new string[] { "Standard"};//{ "Standard", "Precomputed" };
        private string[] m_sunsetColor = new string[] { "Realistic", "Stylized" };
        private string[] m_wavelengthMode = new string[] {"Precomputed", "Real-time" };
		private string[] m_shaderMode = new string[] {"Pixel Shader - Skybox", "Vertex Shader - Skydome" };
        private string[] m_useReflectionProbe = new string[] { "Off", "On" };
        private string[] m_reflectionRefreshMode = new string[] { "On Awake", "Every Frame", "Via Scripting" };
        private string[] m_curveMode = new string[] { "Timeline", "Sun", "Moon" };

        //Show-Hide strings.
        private string m_showHideTimeOfDay;
        private string m_showHideReferences;
        private string showClimateTab;
        private string m_showHideOptions;
        private string m_showHideOutputs;

        //Custom GUI.
        private int m_labelWidth = 84;
        private Texture2D m_logoTex, m_tabTex;
        private Rect m_bgRect;
        private string m_installPath;
        private string m_inspectorGUIPath;
        private Color m_col1 = new Color(1.0f, 1.0f, 1.0f, 1.0f);//Normal.
        private Color m_col2 = new Color(0.0f, 0.0f, 0.0f, 0.0f);//All Transparent.
        private Color m_col3 = new Color(0.35f, 0.65f, 1.0f, 1.0f);//Blue.
        private Color m_col4 = new Color(0.95f, 1.0f, 0.0f, 0.6f);//Yellow semi transparent.
        private Color m_col5 = new Color(0.75f, 1.0f, 0.75f, 1.0f);//Green;
        private Color m_col6 = new Color(1.0f, 0.5f, 0.5f, 1.0f);//Red;
        private Color m_col7 = new Color(0.35f, 0.65f, 1.0f, 0.5f);//Blue semi transparent.
        private Color m_curveColor = Color.yellow;

        //Calendar vars.
        private bool m_goToDate = false;
        private int m_goToMonth = DateTime.Today.Month;
        private int m_goToDay = DateTime.Today.Day;
        private int m_goToYear = DateTime.Today.Year;
        private int m_daysInMonth = 30;
        private int m_dayOfYear = 1;
        private int m_selectedDay;
        private AzureSkyProfile m_calendarDayProfile;
        private string[] m_month = new string[]
        {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        private string[] m_week = new string[]
        {
            "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
        };

        //Serialized Properties.
        SerializedProperty m_dayCycleCurve;

        //Reorderable Lists.
        private ReorderableList m_reorderableStandardProfileList;
        private ReorderableList m_reorderableWeatherProfileList;
		private ReorderableList m_reorderableThunderList;
        private ReorderableList m_reorderableCurveOutputList;
        private ReorderableList m_reorderableGradientOutputList;

        private SerializedProperty m_serializedStandardProfileList;
        private SerializedProperty m_serializedWeatherProfileList;
		private SerializedProperty m_serializedThunderList;
        private SerializedProperty m_serializedCurveOuputList;
        private SerializedProperty m_serializedGradientOuputList;

        void OnEnable()
        {
            //Get target.
            m_Target = (AzureSkyController)target;

            m_dayOfYear = m_Target.timeOfDay.GetDayOfYear();

            //InspectorGUI folder path.
            string scriptLocation = AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this));
            m_installPath = scriptLocation.Replace("/Editor/AzureSkyControllerEditor.cs", "");
            m_inspectorGUIPath = m_installPath + "/Editor/InspectorGUI";

            //Get Serialized Properties.
            m_dayCycleCurve = serializedObject.FindProperty("timeOfDay.dayCycleCurve");

            //First Calendar Update.
			m_Target.UpdateProfiles ();

            //Create Weather Profile List.
            //-------------------------------------------------------------------------------------------------------
            m_serializedWeatherProfileList = serializedObject.FindProperty("weatherProfileList");
            m_reorderableWeatherProfileList = new ReorderableList(serializedObject, m_serializedWeatherProfileList, false, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    string standardProfileName = "None Day Profile";
                    var element = m_reorderableWeatherProfileList.serializedProperty.GetArrayElementAtIndex(index);
                    var transition = element.FindPropertyRelative("TransitionTime");
                    var myList = m_Target.weatherProfileList[index];
                    if (index > 0)
                    {
                        EditorGUI.LabelField(rect, "profile " + index.ToString());
                        //Profile field.
                        GUI.color = m_col2;//Set transparent color.
                        myList.Profile = (AzureSkyProfile)EditorGUI.ObjectField(new Rect(rect.x + 65, rect.y, rect.width - 100 - 28, EditorGUIUtility.singleLineHeight), GUIContent.none, myList.Profile, typeof(AzureSkyProfile), false);
                        GUI.color = m_col1;//standard Color.

                        //Custom Profile field.
                        GUI.color = m_col6;//Red Color.
                        if (m_Target.weatherProfileList[index].Profile)
                        {
                            GUI.color = m_col5;//Green Color.
                            standardProfileName = m_Target.weatherProfileList[index].Profile.name;
                        }
                        EditorGUI.LabelField(new Rect(rect.x + 65, rect.y, rect.width - 100 - 28, EditorGUIUtility.singleLineHeight), standardProfileName, EditorStyles.objectField);
                        GUI.color = m_col1;//Return to standard color.

                        //Transition Time field.
                        EditorGUI.PropertyField(new Rect(rect.x + rect.width - 61, rect.y, 28, EditorGUIUtility.singleLineHeight), transition, GUIContent.none);
                    }
                    else
                    {
                        EditorGUI.LabelField(rect, "profile " + index.ToString());

                        //Custom Profile field.
                        //GUI.color = m_col4;//Transparent Yellow.
                        standardProfileName = "Default";
                        EditorGUI.LabelField(new Rect(rect.x + 65, rect.y, rect.width - 118 - 28, EditorGUIUtility.singleLineHeight), standardProfileName, EditorStyles.textField);
                        GUI.color = m_col1;//Return to standard color.

                        //Transition Time field.
                        EditorGUI.PropertyField(new Rect(rect.x + rect.width - 61, rect.y, 28, EditorGUIUtility.singleLineHeight), transition, GUIContent.none);
                    }
                    //Go Buttom.
                    if (GUI.Button(new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight), "Go"))
                    {
                        if (Application.isPlaying) { m_Target.SetNewWeatherProfile (index); }
                    }
                    m_Target.weatherProfileList[index] = myList;
                },

                onAddCallback = (ReorderableList l) =>
                {
                    var index = l.serializedProperty.arraySize;
                    l.serializedProperty.arraySize++;
                    l.index = index;

                    var element = l.serializedProperty.GetArrayElementAtIndex(index);
                    element.FindPropertyRelative("TransitionTime").floatValue = 30.0f;
                },

                onRemoveCallback = (ReorderableList l) =>
                {
                    if (l.index>0)
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Weather Profiles", EditorStyles.boldLabel);
                },

                drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.SetPixel(0, 0, m_col7);
                    tex.Apply();
                    if (active)
                        GUI.DrawTexture(rect, tex as Texture);
                }
        	};






			//Create Thunder List.
			//-------------------------------------------------------------------------------------------------------
			m_serializedThunderList = serializedObject.FindProperty("thunderAudioClipList");
			m_reorderableThunderList = new ReorderableList(serializedObject, m_serializedThunderList, false, true, true, true)
			{
				drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
				{
					rect.y += 2;
					string audioClipName = "None Clip";
					EditorGUI.LabelField(rect, "element " + index.ToString());
					Rect fieldRect = new Rect(rect.x + 65, rect.y, rect.width - 100 - 28, EditorGUIUtility.singleLineHeight);

					//Profile field.
					GUI.color = m_col2;//Set transparent color.
					m_Target.thunderAudioClipList[index] = (AudioClip)EditorGUI.ObjectField(fieldRect , GUIContent.none, m_Target.thunderAudioClipList[index], typeof(AudioClip), false);
					GUI.color = m_col1;//standard Color.

					//Custom Profile field.
					GUI.color = m_col6;//Red Color.
					if (m_Target.thunderAudioClipList[index])
					{
						GUI.color = m_col5;//Green Color.
						audioClipName = m_Target.thunderAudioClipList[index].name;
					}
					EditorGUI.LabelField(fieldRect, audioClipName, EditorStyles.objectField);
					GUI.color = m_col1;//Return to standard color.

					if (GUI.Button(new Rect(rect.x + rect.width - 61, rect.y, 61, EditorGUIUtility.singleLineHeight), "Play"))
					{
						if (Application.isPlaying) { m_Target.PlayThunderAudioClip (index); }
					}
				},

				onAddCallback = (ReorderableList l) =>
				{
					var index = l.serializedProperty.arraySize;
					l.serializedProperty.arraySize++;
					l.index = index;
				},

				onRemoveCallback = (ReorderableList l) =>
				{
					ReorderableList.defaultBehaviours.DoRemoveButton(l);
				},

				drawHeaderCallback = (Rect rect) =>
				{
					EditorGUI.LabelField(rect, "Thunder Audio Clips", EditorStyles.boldLabel);
				},

				drawElementBackgroundCallback = (rect, index, active, focused) =>
				{
					Texture2D tex = new Texture2D(1, 1);
					tex.SetPixel(0, 0, m_col7);
					tex.Apply();
					if (active)
						GUI.DrawTexture(rect, tex as Texture);
				}
			};








            //Create Standard Profile List.
            //-------------------------------------------------------------------------------------------------------
            m_serializedStandardProfileList = serializedObject.FindProperty("standardProfileList");
            m_reorderableStandardProfileList = new ReorderableList(serializedObject, m_serializedStandardProfileList, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    string standardProfileName = "None Day Profile";
                    Rect fieldRect = new Rect(rect.x + 65, rect.y, rect.width - 65, EditorGUIUtility.singleLineHeight);
                   
                    GUI.color = m_col2;//Set transparent color.
                    EditorGUI.LabelField(rect, "profile " + index.ToString());
                    m_Target.standardProfileList[index] = (AzureSkyProfile)EditorGUI.ObjectField(fieldRect, GUIContent.none, m_Target.standardProfileList[index], typeof(AzureSkyProfile), false);

                    //Custom profile field.
                    GUI.color = m_col6;//Red Color.
                    if (m_Target.standardProfileList[index])
                    {
                        standardProfileName = m_Target.standardProfileList[index].name;
                        GUI.color = m_col5;//Green Color.
                    }
                    EditorGUI.LabelField(rect, "profile " + index.ToString());
                    EditorGUI.LabelField(fieldRect, standardProfileName, EditorStyles.objectField);
                    GUI.color = m_col1;//Return to standard color.
                },

                onAddCallback = (ReorderableList l) =>
                {
                    var index = l.serializedProperty.arraySize;
                    l.serializedProperty.arraySize++;
                    l.index = index;
                },

                onRemoveCallback = (ReorderableList l) =>
                {
                    if (l.index > 0)
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Standard Profiles", EditorStyles.boldLabel);
                },

                drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.SetPixel(0, 0, m_col7);
                    tex.Apply();
                    if (active)
                        GUI.DrawTexture(rect, tex as Texture);
                }
            };

            //Create Curve Output List.
            //-------------------------------------------------------------------------------------------------------
            m_serializedCurveOuputList = serializedObject.FindProperty("curveOuputList");
            m_reorderableCurveOutputList = new ReorderableList(serializedObject, m_serializedCurveOuputList, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    EditorGUI.LabelField(rect, "element " + index.ToString());
                    m_Target.curveOuputList[index].curveIndex = EditorGUI.Popup(new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), m_Target.curveOuputList[index].curveIndex, m_curveMode);

                    switch (m_Target.curveOuputList[index].curveIndex)
                    {
                        case 0:
                            m_curveColor = Color.green;
                            //EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), curve.GetArrayElementAtIndex(0), GUIContent.none);
                            EditorGUI.CurveField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), "", m_Target.curveOuputList[index].curveOutput[0], m_curveColor, new Rect(0, 0, 24, 1));
                            break;

                        case 1:
                            m_curveColor = Color.yellow;
                            //EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), curve.GetArrayElementAtIndex(1), GUIContent.none);
                            EditorGUI.CurveField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), "", m_Target.curveOuputList[index].curveOutput[1], m_curveColor, new Rect(-1, 0, 2, 1));
                            break;

                        case 2:
                            m_curveColor = Color.cyan;
                            //EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), curve.GetArrayElementAtIndex(2), GUIContent.none);
                            EditorGUI.CurveField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), "", m_Target.curveOuputList[index].curveOutput[2], m_curveColor, new Rect(-1, 0, 2, 1));
                            break;
                    }
                },

                onAddCallback = (ReorderableList l) =>
                {
                    var index = l.serializedProperty.arraySize;
                    l.serializedProperty.arraySize++;
                    l.index = index;

                    var element = m_reorderableCurveOutputList.serializedProperty.GetArrayElementAtIndex(index);
                    var curve = element.FindPropertyRelative("curveOutput");

                    curve.GetArrayElementAtIndex(0).animationCurveValue = AnimationCurve.Linear(0.0f, 0.5f, 24.0f, 0.5f);
                    curve.GetArrayElementAtIndex(1).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.5f, 1.0f, 0.5f);
                    curve.GetArrayElementAtIndex(2).animationCurveValue = AnimationCurve.Linear(-1.0f, 0.5f, 1.0f, 0.5f);
                },

                onRemoveCallback = (ReorderableList l) =>
                {
                    if (l.index > 0)
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Curve Outputs", EditorStyles.boldLabel);
                },

                drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.SetPixel(0, 0, m_col7);
                    tex.Apply();
                    if (active)
                        GUI.DrawTexture(rect, tex as Texture);
                }
            };

            //Create Gradient Output List.
            //-------------------------------------------------------------------------------------------------------
            m_serializedGradientOuputList = serializedObject.FindProperty("gradientOuputList");
            m_reorderableGradientOutputList = new ReorderableList(serializedObject, m_serializedGradientOuputList, true, true, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 2;
                    EditorGUI.LabelField(rect, "element " + index.ToString());
                    var element = m_reorderableGradientOutputList.serializedProperty.GetArrayElementAtIndex(index);
                    var gradient = element.FindPropertyRelative("gradientOutput");

                    m_Target.gradientOuputList[index].gradientIndex = EditorGUI.Popup(new Rect(rect.x + rect.width - 60, rect.y, 60, EditorGUIUtility.singleLineHeight), m_Target.gradientOuputList[index].gradientIndex, m_curveMode);

                    switch (m_Target.gradientOuputList[index].gradientIndex)
                    {
                        case 0:
                            EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), gradient.GetArrayElementAtIndex(0), GUIContent.none);
                            break;

                        case 1:
                            EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), gradient.GetArrayElementAtIndex(1), GUIContent.none);
                            break;

                        case 2:
                            EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - 130, EditorGUIUtility.singleLineHeight), gradient.GetArrayElementAtIndex(2), GUIContent.none);
                            break;
                    }
                },

                onAddCallback = (ReorderableList l) =>
                {
                    var index = l.serializedProperty.arraySize;
                    l.serializedProperty.arraySize++;
                    l.index = index;
                },

                onRemoveCallback = (ReorderableList l) =>
                {
                    if (l.index > 0)
                    {
                        ReorderableList.defaultBehaviours.DoRemoveButton(l);
                    }
                },

                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, "Gradient Outputs", EditorStyles.boldLabel);
                },

                drawElementBackgroundCallback = (rect, index, active, focused) =>
                {
                    Texture2D tex = new Texture2D(1, 1);
                    tex.SetPixel(0, 0, m_col7);
                    tex.Apply();
                    if (active)
                        GUI.DrawTexture(rect, tex as Texture);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            //Start.
            //-------------------------------------------------------------------------------------------------------
            serializedObject.Update();
            Undo.RecordObject(m_Target, "Undo Azure Sky Controller");

            //Used to pick up the correct element from the array when it is leap year.
            //-------------------------------------------------------------------------------------------------------
			m_dayOfYear = m_Target.timeOfDay.GetDayOfYear ();

            //Show-Hide tab text.
            //-------------------------------------------------------------------------------------------------------
            if (m_Target.editorSettings.showTimeOfDayTab) m_showHideTimeOfDay = "| Hide"; else m_showHideTimeOfDay = "| Show";
            if (m_Target.editorSettings.showOptionsTab) m_showHideOptions = "| Hide"; else m_showHideOptions = "| Show";
            if (m_Target.editorSettings.showOutputTab) m_showHideOutputs = "| Hide"; else m_showHideOutputs = "| Show";
            if (m_Target.editorSettings.showReferencesTab) m_showHideReferences = "| Hide"; else m_showHideReferences = "| Show";
            if (m_Target.editorSettings.showClimateTab) showClimateTab = "| Hide"; else showClimateTab = "| Show";

            //Logo and Textures.
            //-------------------------------------------------------------------------------------------------------
            m_logoTex = AssetDatabase.LoadAssetAtPath(m_inspectorGUIPath + "/AzureSkyControllerBar.png", typeof(Texture2D)) as Texture2D;
            m_tabTex = AssetDatabase.LoadAssetAtPath(m_inspectorGUIPath + "/InspectorTab.png", typeof(Texture2D)) as Texture2D;
            EditorGUILayout.Space();
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(m_bgRect.x, m_bgRect.y, 261, 56), m_logoTex);
            //GUILayout.Space(24);
            GUILayout.Space(32);
            EditorGUILayout.BeginHorizontal();
            //GUILayout.Space(60);
            GUILayout.Label("Version 4.3.1", EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();




            //Time of Day Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showTimeOfDayTab = !m_Target.editorSettings.showTimeOfDayTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showTimeOfDayTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showTimeOfDayTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "TIME OF DAY", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideTimeOfDay);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            if (m_Target.editorSettings.showTimeOfDayTab)
            {
                //Goto Date.
                //-------------------------------------------------------------------------------------------------------
                if (m_goToDate)
                {
                    GUI.color = m_col3;//Set blue color.
                    EditorGUILayout.BeginVertical("Box");
                    GUI.color = m_col1;//Return to standard color.
                    EditorGUILayout.BeginHorizontal("Box");
                    GUILayout.Label("Month:", GUILayout.Width(45));
                    m_goToMonth = EditorGUILayout.DelayedIntField(Mathf.Clamp(m_goToMonth, 1, 12), GUILayout.Width(20));
                    m_goToMonth = Mathf.Clamp(m_goToMonth, 1, 12);
                    EditorGUILayout.Space();
                    GUILayout.Label("Day:", GUILayout.Width(30));
                    m_daysInMonth = DateTime.DaysInMonth(m_goToYear, m_goToMonth);
                    m_goToDay = EditorGUILayout.DelayedIntField(Mathf.Clamp(m_goToDay, 1, m_daysInMonth), GUILayout.Width(20));
                    m_goToDay = Mathf.Clamp(m_goToDay, 1, m_daysInMonth);
                    EditorGUILayout.Space();
                    GUILayout.Label("Year:", GUILayout.Width(35));
                    m_goToYear = EditorGUILayout.DelayedIntField(Mathf.Clamp(m_goToYear, 1, 9999), GUILayout.Width(35));
                    m_goToYear = Mathf.Clamp(m_goToYear, 1, 9999);
                    EditorGUILayout.EndHorizontal();
                    if (GUILayout.Button("Goto"))
                    {
						m_Target.timeOfDay.GotoDate (m_goToMonth, m_goToDay, m_goToYear);
						m_Target.UpdateProfiles ();
                        m_goToDate = false;
                    }
                    EditorGUILayout.EndVertical();
                }
                //Calendar Header.
                //-------------------------------------------------------------------------------------------------------
                GUI.color = m_col3;//Set blue color.
                EditorGUILayout.BeginHorizontal("Box");
                GUI.color = m_col1;//Return to standard color.
                //Decrease Year Button.
                if (GUILayout.Button("<<", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                {
                    m_Target.timeOfDay.year--;
                    if (m_Target.timeOfDay.year < 0) { m_Target.timeOfDay.month = 9999; }
					m_Target.UpdateProfiles ();
                }
                //Decrease Month Button.
                if (GUILayout.Button("<", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
                {
                    m_Target.timeOfDay.month--;
                    if(m_Target.timeOfDay.month < 1) { m_Target.timeOfDay.month = 12; }
					m_Target.UpdateProfiles ();
                }
                //Center Button.
                if(GUILayout.Button(m_month[m_Target.timeOfDay.month - 1] + " " + m_Target.timeOfDay.day.ToString("00") + ", " + m_Target.timeOfDay.year.ToString("0000"), EditorStyles.miniButtonMid))
                {
                    m_goToMonth = DateTime.Today.Month;
                    m_goToDay = DateTime.Today.Day;
                    m_goToYear = DateTime.Today.Year;
                    m_goToDate = !m_goToDate;
                }
                //Increase Month Button.
                if (GUILayout.Button(">", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                {
                    m_Target.timeOfDay.month++;
                    if (m_Target.timeOfDay.month > 12) { m_Target.timeOfDay.month = 1; }
					m_Target.UpdateProfiles ();
                }
                //Increase Year Button.
                if (GUILayout.Button(">>", EditorStyles.miniButtonRight, GUILayout.Width(25)))
                {
                    m_Target.timeOfDay.year++;
                    if (m_Target.timeOfDay.year > 9999) { m_Target.timeOfDay.month = 0; }
					m_Target.UpdateProfiles ();
                }
                EditorGUILayout.EndHorizontal();

                //Calendar.
                //-------------------------------------------------------------------------------------------------------
                GUILayout.Space(-3);
                //Weekdays.
                EditorGUILayout.BeginHorizontal("Box");
                GUILayout.Label("Sun");
                GUILayout.Label("Mon");
                GUILayout.Label("Tue");
                GUILayout.Label("Wed");
                GUILayout.Label("Thu");
                GUILayout.Label("Fri");
                GUILayout.Label("Sat");
                EditorGUILayout.EndHorizontal();
                //Selectable Days.
                GUILayout.Space(-5);
                m_selectedDay = m_Target.timeOfDay.day;
                EditorGUILayout.BeginVertical("Box");
                m_Target.timeOfDay.selectableDayInt = GUILayout.SelectionGrid(m_Target.timeOfDay.selectableDayInt, m_Target.timeOfDay.selectableDayList, 7);
                if (m_Target.timeOfDay.selectableDayList[m_Target.timeOfDay.selectableDayInt] != "")
                {
                    m_Target.timeOfDay.day = m_Target.timeOfDay.selectableDayInt + 1 - m_Target.timeOfDay.GetDayOfWeek(1);
                }
                else
                    {
                        m_Target.timeOfDay.selectableDayInt = m_Target.timeOfDay.day - 1 + m_Target.timeOfDay.GetDayOfWeek(1);
                    }
                EditorGUILayout.EndVertical();
                //Check if a new calendar day has been selected.
                if (m_selectedDay != m_Target.timeOfDay.day)
                {
					m_Target.UpdateProfiles ();
                }

                //Day of Year Profile.
                //-------------------------------------------------------------------------------------------------------
                GUILayout.Space(-5);
                EditorGUILayout.BeginVertical("Box");
				m_calendarDayProfile = m_Target.calendarProfileList[m_dayOfYear];//Save calendar profile status before change.

				//Standard Objectfield.
                EditorGUILayout.BeginHorizontal();
                GUI.color = m_col2;//Set transparent.
                m_bgRect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(m_bgRect, "Day Profile");
                m_Target.calendarProfileList[m_dayOfYear] = (AzureSkyProfile)EditorGUI.ObjectField(new Rect(m_bgRect.x+81, m_bgRect.y, m_bgRect.width-81, EditorGUIUtility.singleLineHeight), GUIContent.none, m_Target.calendarProfileList[m_dayOfYear], typeof(AzureSkyProfile), false);
                EditorGUILayout.EndHorizontal();

                //Configuring Custom Objectield.
                GUILayout.Space(-18);
                GUI.color = m_col5;//Set green color.
                if (!m_Target.calendarProfileList[m_dayOfYear]) { GUI.color = m_col6; }//Set red color.
                EditorGUILayout.BeginHorizontal();
                if (m_Target.calendarProfileList[m_dayOfYear])
                {
                    m_dayProfileName = m_Target.calendarProfileList[m_dayOfYear].name;
					m_dayProfileInfo = 0;
                }
                else
	                {
						if (m_Target.calendarDayProfile)
	                    {
	                        m_dayProfileName = "Standard Profile";
							m_dayProfileInfo = 1;
	                        GUI.color = m_col4;//Set semi-transparent green color.
	                    }
	                    else
		                    {
		                        m_dayProfileName = "None Day Profile";
								m_dayProfileInfo = 2;
		                        GUI.color = m_col6;
		                    }
	                }

				//Draw Custom Objectfield.
                m_bgRect = EditorGUILayout.GetControlRect();
                EditorGUI.LabelField(m_bgRect, "Day Profile");
                EditorGUI.LabelField(new Rect(m_bgRect.x+81, m_bgRect.y, m_bgRect.width-81, EditorGUIUtility.singleLineHeight), m_dayProfileName, EditorStyles.objectField);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;//Return to standard color.
                EditorGUILayout.EndVertical();

				//Draw Profile Info.
				switch (m_dayProfileInfo)
                {
				case 1:
					EditorGUILayout.HelpBox ("Using a random day profile from the Standard Profiles list.", MessageType.Info);
					break;
				case 2:
					EditorGUILayout.HelpBox ("Trying to get a random day profile from the Standard Profiles list, it seems that the standard profiles list does not exist " +
						"or does not have all profiles set. Please check the Standard Profiles list setting on the Options tab and start the day again.", MessageType.Error);
					break;
                }

				//Apply new profile to the calendar when the use change in the Inspector.
                if(m_calendarDayProfile != m_Target.calendarProfileList[m_dayOfYear])
                {
                    m_Target.calendarDayProfile = m_Target.calendarProfileList[m_dayOfYear];
                }
                m_bgRect = EditorGUILayout.GetControlRect();
                GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, -17).x, GUILayoutUtility.GetRect(m_bgRect.width, 1).y - 2, m_bgRect.width, 1), m_tabTex);
                EditorGUILayout.Space();
                //-------------------------------------------------------------------------------------------------------

                //Time Mode.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Time Mode", GUILayout.Width(m_labelWidth));
                m_Target.timeOfDay.timeMode = EditorGUILayout.Popup(m_Target.timeOfDay.timeMode, m_timeMode);
                EditorGUILayout.EndHorizontal();
                //Timeline.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Timeline", GUILayout.Width(m_labelWidth));
                m_Target.timeOfDay.hour = EditorGUILayout.Slider(m_Target.timeOfDay.hour, 0, 24);
                EditorGUILayout.EndHorizontal();
                //Latitude.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Latitude", GUILayout.Width(m_labelWidth));
                m_Target.timeOfDay.latitude = EditorGUILayout.Slider(m_Target.timeOfDay.latitude, -90, 90);
                EditorGUILayout.EndHorizontal();
                //Longitude.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Longitude", GUILayout.Width(m_labelWidth));
                m_Target.timeOfDay.longitude = EditorGUILayout.Slider(m_Target.timeOfDay.longitude, -180, 180);
                EditorGUILayout.EndHorizontal();
                //UTC.
                //if (m_Target.timeOfDay.timeMode == 1)
                //{
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("UTC", GUILayout.Width(m_labelWidth));
                    m_Target.timeOfDay.utc = EditorGUILayout.Slider(m_Target.timeOfDay.utc, -12, 12);
                    EditorGUILayout.EndHorizontal();
                //}
                //Day Cycle.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Day Cycle in Minutes");
                m_Target.timeOfDay.dayCycle = EditorGUILayout.FloatField(m_Target.timeOfDay.dayCycle, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                //Time Curve.
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                GUILayout.Label("Day and Night Length", EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                //Set Time by Curve?
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Set Time of Day by Curve?");
                m_Target.timeOfDay.setTimeByCurve = EditorGUILayout.Toggle(m_Target.timeOfDay.setTimeByCurve, GUILayout.Width(15));
                EditorGUILayout.EndHorizontal();
                //Day and Night Length Curve Field.
                EditorGUILayout.BeginHorizontal();
                GUI.color = m_col3;
                if (GUILayout.Button("R", GUILayout.Width(25), GUILayout.Height(25)))
                {
                    m_dayCycleCurve.animationCurveValue = AnimationCurve.Linear(0, 0, 24, 24);
                }
                GUI.color = m_col1;
                EditorGUILayout.CurveField(m_dayCycleCurve, m_curveColor, new Rect(0, 0, 24, 24), GUIContent.none, GUILayout.Height(25));
                EditorGUILayout.EndHorizontal();
                //Draw Current Date and Time.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Current Time:");
                GUILayout.Label(m_week[m_Target.timeOfDay.GetDayOfWeek()]
                                + " " + m_Target.timeOfDay.GetTime().x.ToString("00")
                                + ":" + m_Target.timeOfDay.GetTime().y.ToString("00"), GUILayout.ExpandWidth(false));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.Space();



            //References Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showReferencesTab = !m_Target.editorSettings.showReferencesTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showReferencesTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showReferencesTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "REFERENCES", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideReferences);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            if (m_Target.editorSettings.showReferencesTab)
            {
                // Sun Transform.
                GUI.color = m_col5;
                if (!m_Target.sunTransform)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sun", GUILayout.Width(m_labelWidth));
                m_Target.sunTransform = (Transform)EditorGUILayout.ObjectField(m_Target.sunTransform, typeof(Transform), true);
                EditorGUILayout.EndHorizontal();

                // Moon Transform.
                GUI.color = m_col5;
                if (!m_Target.moonTransform)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Moon", GUILayout.Width(m_labelWidth));
                m_Target.moonTransform = (Transform)EditorGUILayout.ObjectField(m_Target.moonTransform, typeof(Transform), true);
                EditorGUILayout.EndHorizontal();

                // Light Transform.
                GUI.color = m_col5;
                if (!m_Target.lightTransform)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light", GUILayout.Width(m_labelWidth));
                m_Target.lightTransform = (Transform)EditorGUILayout.ObjectField(m_Target.lightTransform, typeof(Transform), true);
                EditorGUILayout.EndHorizontal();

                // Sun Texture.
                GUI.color = m_col5;
                if (!m_Target.sunTexture)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sun Texture", GUILayout.Width(m_labelWidth));
                m_Target.sunTexture = (Texture2D)EditorGUILayout.ObjectField(m_Target.sunTexture, typeof(Texture2D), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

                // Moon Texture.
                GUI.color = m_col5;
                if (!m_Target.moonTexture)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Moon Texture", GUILayout.Width(m_labelWidth));
                m_Target.moonTexture = (Texture2D)EditorGUILayout.ObjectField(m_Target.moonTexture, typeof(Texture2D), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

                // Cloud Noise Texture.
                GUI.color = m_col5;
                if (!m_Target.cloudNoise)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Cloud Noise", GUILayout.Width(m_labelWidth));
                m_Target.cloudNoise = (Texture2D)EditorGUILayout.ObjectField(m_Target.cloudNoise, typeof(Texture2D), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

                // Starfield Cubemap.
                GUI.color = m_col5;
                if (!m_Target.starfieldTexture)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Starfield", GUILayout.Width(m_labelWidth));
                m_Target.starfieldTexture = (Cubemap)EditorGUILayout.ObjectField(m_Target.starfieldTexture, typeof(Cubemap), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

                // Star Noise Cubemap.
                GUI.color = m_col5;
                if (!m_Target.starNoiseTexture)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Star Noise", GUILayout.Width(m_labelWidth));
                m_Target.starNoiseTexture = (Cubemap)EditorGUILayout.ObjectField(m_Target.starNoiseTexture, typeof(Cubemap), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;

				// Sky Material.
				GUI.color = m_col5;
				if (!m_Target.skyMaterial)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Sky Material", GUILayout.Width(m_labelWidth));
				m_Target.skyMaterial = (Material)EditorGUILayout.ObjectField(m_Target.skyMaterial, typeof(Material), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Rain Material.
				GUI.color = m_col5;
				if (!m_Target.rainMaterial)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Material", GUILayout.Width(m_labelWidth));
				m_Target.rainMaterial = (Material)EditorGUILayout.ObjectField(m_Target.rainMaterial, typeof(Material), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Snow Material.
				GUI.color = m_col5;
				if (!m_Target.snowMaterial)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Snow Material", GUILayout.Width(m_labelWidth));
				m_Target.snowMaterial = (Material)EditorGUILayout.ObjectField(m_Target.snowMaterial, typeof(Material), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Rain Particle.
				GUI.color = m_col5;
				if (!m_Target.rainParticle)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Rain Particle", GUILayout.Width(m_labelWidth));
				m_Target.rainParticle = (ParticleSystem)EditorGUILayout.ObjectField(m_Target.rainParticle, typeof(ParticleSystem), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Snow Particle.
				GUI.color = m_col5;
				if (!m_Target.snowParticle)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Snow Particle", GUILayout.Width(m_labelWidth));
				m_Target.snowParticle = (ParticleSystem)EditorGUILayout.ObjectField(m_Target.snowParticle, typeof(ParticleSystem), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Wind Zone.
				GUI.color = m_col5;
				if (!m_Target.windZone)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wind Zone", GUILayout.Width(m_labelWidth));
				m_Target.windZone = (WindZone)EditorGUILayout.ObjectField(m_Target.windZone, typeof(WindZone), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// SoundFX.
				GUI.color = m_col5;
				if (!m_Target.soundFX)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("SoundFX", GUILayout.Width(m_labelWidth));
				m_Target.soundFX = (AzureSkySoundFX)EditorGUILayout.ObjectField(m_Target.soundFX, typeof(AzureSkySoundFX), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

				// Skydome.
				GUI.color = m_col5;
				if (!m_Target.skydome)
				{
					GUI.color = m_col6;
				}
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Skydome", GUILayout.Width(m_labelWidth));
				m_Target.skydome = (Transform)EditorGUILayout.ObjectField(m_Target.skydome, typeof(Transform), true);
				EditorGUILayout.EndHorizontal();
				GUI.color = m_col1;

                // Reflection Probe.
                GUI.color = m_col5;
                if (!m_Target.reflectionProbe)
                {
                    GUI.color = m_col6;
                }
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Refl. Probe", GUILayout.Width(m_labelWidth));
                m_Target.reflectionProbe = (ReflectionProbe)EditorGUILayout.ObjectField(m_Target.reflectionProbe, typeof(ReflectionProbe), true);
                EditorGUILayout.EndHorizontal();
                GUI.color = m_col1;
            }
            EditorGUILayout.Space();



            //Climate Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showClimateTab = !m_Target.editorSettings.showClimateTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showClimateTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showClimateTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "CLIMATE", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), showClimateTab);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            if (m_Target.editorSettings.showClimateTab)
            {
				//Progress Bar.
				EditorGUILayout.Space ();
				Rect rect = GUILayoutUtility.GetRect (18, 18, "TextField");
				EditorGUI.ProgressBar (rect, m_Target.weatherTransitionTime, "Weather Transition Progress");

                //Weather Profiles List.
                m_reorderableWeatherProfileList.DoLayoutList();

				//Thunder AudioClip List.
				EditorGUILayout.Space();
				m_reorderableThunderList.DoLayoutList();

				if (m_Target.thunderAudioClipList.Count > 0)
				{
					GUILayout.Space (-17);
					EditorGUILayout.BeginHorizontal();
					if (GUILayout.Button ("Play Random Audio Clip", EditorStyles.miniButton))
					{
						if (Application.isPlaying) m_Target.PlayThunderAudioClipRandom ();
					}
					GUILayout.Label("", GUILayout.Width(53));
					EditorGUILayout.EndHorizontal();
				}
            }
            EditorGUILayout.Space();



            //Options Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showOptionsTab = !m_Target.editorSettings.showOptionsTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showOptionsTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showOptionsTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "OPTIONS", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideOptions);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            if (m_Target.editorSettings.showOptionsTab)
            {
                //Time and Date system toogles.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Start at Current Time System");
                m_Target.options.startAtCurrentTime = EditorGUILayout.Toggle(m_Target.options.startAtCurrentTime, GUILayout.Width(15));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Start at Current Date System");
                m_Target.options.startAtCurrentDate = EditorGUILayout.Toggle(m_Target.options.startAtCurrentDate, GUILayout.Width(15));
                EditorGUILayout.EndHorizontal();
				//Follow Main Camera Toogle.
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Follow Active Main Camera");
				m_Target.options.followMainCamera = EditorGUILayout.Toggle(m_Target.options.followMainCamera, GUILayout.Width(15));
				EditorGUILayout.EndHorizontal();
                //Planet Radius.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Planet Radius");
                m_Target.options.planetRadius = EditorGUILayout.FloatField(m_Target.options.planetRadius, GUILayout.Width(50));
                EditorGUILayout.EndHorizontal();
                //Light Speed.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Light Speed", GUILayout.Width(m_labelWidth));
                m_Target.options.lightSpeed = EditorGUILayout.Slider(m_Target.options.lightSpeed, 0, 100);
                EditorGUILayout.EndHorizontal();
                //Fog Scattering Mie Distance.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Mie Distance", GUILayout.Width(m_labelWidth));
                m_Target.options.mieDepth = EditorGUILayout.Slider(m_Target.options.mieDepth, 0, 1);
                EditorGUILayout.EndHorizontal();
                //Sun Size.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Sun Size", GUILayout.Width(m_labelWidth));
                m_Target.options.sunSize = EditorGUILayout.Slider(m_Target.options.sunSize, 0.5f, 10.0f);
                EditorGUILayout.EndHorizontal();
                //Moon Size.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Moon Size", GUILayout.Width(m_labelWidth));
                m_Target.options.moonSize = EditorGUILayout.Slider(m_Target.options.moonSize, 1, 20);
                EditorGUILayout.EndHorizontal();


                //Starfield Position.
                EditorGUILayout.BeginVertical("Box");
                GUILayout.Label("Starfield Position:");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Position X", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldPosition.x = EditorGUILayout.Slider(m_Target.options.starfieldPosition.x, 0.0f, 360.0f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Position Y", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldPosition.y = EditorGUILayout.Slider(m_Target.options.starfieldPosition.y, 0.0f, 360.0f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Position Z", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldPosition.z = EditorGUILayout.Slider(m_Target.options.starfieldPosition.z, 0.0f, 360.0f);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();
                GUILayout.Label("Starfield Color Balance:");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color R", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldColor.x = EditorGUILayout.Slider(m_Target.options.starfieldColor.x, 1.0f, 2.0f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color G", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldColor.y = EditorGUILayout.Slider(m_Target.options.starfieldColor.y, 1.0f, 2.0f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Color B", GUILayout.Width(m_labelWidth-4));
                m_Target.options.starfieldColor.z = EditorGUILayout.Slider(m_Target.options.starfieldColor.z, 1.0f, 2.0f);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                //Reflection Probe Mode
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Refl. Probe", GUILayout.Width(m_labelWidth - 4));
                m_Target.options.useReflectionProbe = EditorGUILayout.Popup(m_Target.options.useReflectionProbe, m_useReflectionProbe);
                EditorGUILayout.EndHorizontal();
                if (m_Target.options.useReflectionProbe == 1)
                {
                    m_Target.reflectionProbe.gameObject.SetActive(true);

                    //Refresh Mode.
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("Refresh Mode", GUILayout.Width(m_labelWidth - 4));
                    m_Target.options.reflectionProbeRefreshMode = EditorGUILayout.Popup(m_Target.options.reflectionProbeRefreshMode, m_reflectionRefreshMode);
                    EditorGUILayout.EndHorizontal();
                    switch (m_Target.options.reflectionProbeRefreshMode)
                    {
                        case 0:
                            m_Target.reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
                            break;
                        case 1:
                            m_Target.reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame;
                            break;
                        case 2:
                            m_Target.reflectionProbe.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
                            //Refresh Mode
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Refresh Time", GUILayout.Width(m_labelWidth - 4));
                            m_Target.options.reflectionProbeUpdateTime = EditorGUILayout.FloatField(m_Target.options.reflectionProbeUpdateTime);
                            EditorGUILayout.EndHorizontal();
                            //Force Update at First Frame?
                            EditorGUILayout.BeginHorizontal();
                            GUILayout.Label("Update at Start?");
                            m_Target.options.reflectionProbeUpdateAtFirstFrame = EditorGUILayout.Toggle(m_Target.options.reflectionProbeUpdateAtFirstFrame, GUILayout.Width(15));
                            EditorGUILayout.EndHorizontal();
                            EditorGUILayout.Space();
                            break;
                    }
					EditorGUILayout.HelpBox("Unity's GI is very expensive, so performance dropping is normal when the Reflection Probe is on. If you do not need Global Illumination, please turn the Reflection Probe off.",MessageType.Warning);
                }
                else
	                {
	                    m_Target.reflectionProbe.gameObject.SetActive(false);
	                }
                EditorGUILayout.EndVertical();

				//Particles Mode.
				EditorGUILayout.BeginVertical("Box");
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Particles", GUILayout.Width(m_labelWidth - 4));
				m_Target.options.particlesMode = EditorGUILayout.Popup(m_Target.options.particlesMode, m_particleMode);
				m_Target.SetParticlesActive(m_Target.options.particlesMode);
				EditorGUILayout.EndHorizontal();
				if (m_Target.options.particlesMode == 0) {
					//Keep Wheater Update.
					EditorGUILayout.BeginHorizontal ();
					GUILayout.Label ("Keep Wheater Properties Updated");
					m_Target.options.keepWeatherUpdate = EditorGUILayout.Toggle (m_Target.options.keepWeatherUpdate, GUILayout.Width (15));
					EditorGUILayout.EndHorizontal ();
				}
				else
					{
						EditorGUILayout.HelpBox ("The use of particles can greatly reduce performance. Disable the particles if you do not need climate changes.", MessageType.Warning);
					}
				EditorGUILayout.EndVertical();
                //Clouds Mode.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Clouds", GUILayout.Width(m_labelWidth));
                m_Target.options.cloudMode = EditorGUILayout.Popup(m_Target.options.cloudMode, m_cloudMode);
				m_Target.ConfigureShaders ();
                EditorGUILayout.EndHorizontal();
                //Repeat Mode.
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Repeat Mode", GUILayout.Width(m_labelWidth));
                m_Target.options.repeatMode = EditorGUILayout.Popup(m_Target.options.repeatMode, m_repeatMode);
                EditorGUILayout.EndHorizontal();
				//Sunset Mode
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Sunset Mode", GUILayout.Width(m_labelWidth));
				m_Target.options.sunsetColorMode = EditorGUILayout.Popup(m_Target.options.sunsetColorMode, m_sunsetColor);
				EditorGUILayout.EndHorizontal();
				//Wavelength Mode
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wavelength", GUILayout.Width(m_labelWidth));
				m_Target.options.wavelengthMode = EditorGUILayout.Popup(m_Target.options.wavelengthMode, m_wavelengthMode);
				EditorGUILayout.EndHorizontal();
				//Shader Mode
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Shader Mode", GUILayout.Width(m_labelWidth));
				m_Target.options.shaderMode = EditorGUILayout.Popup(m_Target.options.shaderMode, m_shaderMode);
				EditorGUILayout.EndHorizontal();
				if (m_Target.options.shaderMode == 0)
				{
					EditorGUILayout.HelpBox("Pixel Shader requires more processing, consider using Vertex Shader for a better performance.",MessageType.Info);
				}
				else
					{
						EditorGUILayout.HelpBox("Note: Vertex Shader uses a skydome and the sky lighting will not affect the ambient lighting. Switch to Pixel Shader if you want Envorinment Lighting as Skybox.",MessageType.Info);
					}

                //Standard Profiles List.
                m_reorderableStandardProfileList.DoLayoutList();
            }
            EditorGUILayout.Space();



            //Output Tab.
            //-------------------------------------------------------------------------------------------------------
            m_bgRect = EditorGUILayout.GetControlRect();
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, 0).y - 21, m_bgRect.width, 2), m_tabTex);
            GUI.color = m_col2;//Set transparent color.
            if (GUI.Button(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "")) m_Target.editorSettings.showOutputTab = !m_Target.editorSettings.showOutputTab;
            GUI.color = m_col1;//Return to standard color.
            m_Target.editorSettings.showOutputTab = EditorGUI.Foldout(new Rect(m_bgRect.width + 15, m_bgRect.y, m_bgRect.width, 15), m_Target.editorSettings.showOutputTab, "");
            GUI.Label(new Rect(m_bgRect.x, m_bgRect.y, m_bgRect.width, 15), "OUTPUTS", EditorStyles.boldLabel);
            GUI.Label(new Rect(m_bgRect.width - 40, m_bgRect.y, m_bgRect.width, 15), m_showHideOutputs);
            GUI.DrawTexture(new Rect(GUILayoutUtility.GetRect(m_bgRect.width, 1).x, GUILayoutUtility.GetRect(m_bgRect.width, -4).y - 5, m_bgRect.width, 2), m_tabTex);
            if (m_Target.editorSettings.showOutputTab)
            {
                EditorGUILayout.Space();
                //Curve Output List.
                m_reorderableCurveOutputList.DoLayoutList();

                EditorGUILayout.Space();
                //Gradient Output List.
                m_reorderableGradientOutputList.DoLayoutList();
            }
            EditorGUILayout.Space();


            //base.OnInspectorGUI();
            //Refresh the Inspector.
            //-------------------------------------------------------------------------------------------------------
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}