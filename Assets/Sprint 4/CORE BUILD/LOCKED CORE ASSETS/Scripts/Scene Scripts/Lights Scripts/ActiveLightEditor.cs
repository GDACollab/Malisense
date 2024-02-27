using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(ActiveLight))]
public class ActiveLightEditor : Editor
{
    SerializedProperty lightStyle;
    SerializedProperty flickerPercentRange;
    SerializedProperty flickerTimeRange;
    SerializedProperty glowTimeRange;
    SerializedProperty dimPercentRange;
    
    
    private void OnEnable()
    {
        lightStyle = serializedObject.FindProperty("lightStyle");
        flickerPercentRange = serializedObject.FindProperty("flickerPercentRange");
        flickerTimeRange = serializedObject.FindProperty("flickerTimeRange");
        glowTimeRange = serializedObject.FindProperty("glowTimeRange");
        dimPercentRange = serializedObject.FindProperty("dimPercentRange");
    }
    
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(lightStyle);
        
        if((ActiveLight.LIGHT_STYLE)lightStyle.enumValueIndex == ActiveLight.LIGHT_STYLE.flicker){
            EditorGUILayout.PropertyField(flickerPercentRange);
            EditorGUILayout.PropertyField(flickerTimeRange);
        }
        else if((ActiveLight.LIGHT_STYLE)lightStyle.enumValueIndex == ActiveLight.LIGHT_STYLE.glow){
            EditorGUILayout.PropertyField(glowTimeRange);
        }
        EditorGUILayout.PropertyField(dimPercentRange);
        
        serializedObject.ApplyModifiedProperties();
    }
}
#endif