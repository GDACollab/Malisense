using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapShadows))]
public class TilemapShadowsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Regenerate"))
        {
            foreach(var target in targets)
            {
                if(target is TilemapShadows shadows)
                {
                    shadows.Regenerate();
                }
            }
        }

        if(GUILayout.Button("Clear"))
        {
            foreach(var target in targets)
            {
                if(target is TilemapShadows shadows)
                {
                    shadows.Clear();
                }
            }
        }
    }
}
