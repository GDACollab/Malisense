using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolPath))]
public class PatrolPathEditor : Editor
{
    private void OnSceneGUI()
    {
        var addText = new GUIContent("Add");
        var areas = FindObjectsOfType<SBProtoPatrolArea>();
        var targets = this.targets.OfType<PatrolPath>();

        var areaProperty = serializedObject.FindProperty(nameof(PatrolPath.areas));

        // Draw a rectangle around each area
        Handles.color = Color.red;
        foreach(var area in areas)
        {
            if (SceneVisibilityManager.instance.IsHidden(area.gameObject))
                continue;

            Handles.matrix = area.transform.localToWorldMatrix;
            Handles.DrawWireCube(Vector3.zero, area.size);
        }

        // Draw lines between areas in the path
        Handles.matrix = Matrix4x4.identity;
        foreach(var target in targets)
        {
            if (target.areas.Length == 0) continue;

            for (int i = 0; i < target.areas.Length; i++)
            {
                if (target.areas[i] == null || target.areas[(i + 1) % target.areas.Length] == null) continue;

                Vector2 start = target.areas[i].transform.position;
                Vector2 end = target.areas[(i + 1) % target.areas.Length].transform.position;
                Vector2 dir = (end - start).normalized;
                end -= dir * 1f;
                Handles.DrawLine(start, end);
                Handles.DrawLine(end, end - Rotate(dir * 0.5f, 45f));
                Handles.DrawLine(end, end - Rotate(dir * 0.5f, -45f));
            }
        }

        // Put an "Add" button over each area to add it to the path
        Handles.matrix = Matrix4x4.identity;
        Handles.BeginGUI();
        foreach (var area in areas)
        {
            if (targets.All(target => target.areas.Length > 0 && target.areas[^1] == area)
                || SceneVisibilityManager.instance.IsHidden(area.gameObject))
            {
                continue;
            }

            var rect = HandleUtility.WorldPointToSizedRect(area.transform.position, addText, GUIStyle.none);
            rect.size += new Vector2(12f, 6f);
            rect.position -= rect.size / 2f;

            if(GUI.Button(rect, addText))
            {
                Undo.RecordObjects(this.targets, "Add patrol area");

                foreach (var target in targets)
                {
                    if (target.areas.Length == 0 || target.areas[^1] != area)
                    {
                        areaProperty.InsertArrayElementAtIndex(areaProperty.arraySize);
                        areaProperty.GetArrayElementAtIndex(areaProperty.arraySize - 1).objectReferenceValue = area;
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
        }

        GUILayout.BeginArea(new Rect(10f, Screen.height - 80f, 100f, 30f));
        if(GUILayout.Button("Clear Path"))
        {
            areaProperty.ClearArray();
            serializedObject.ApplyModifiedProperties();
        }
        GUILayout.EndArea();

        Handles.EndGUI();
    }

    private static Vector2 Rotate(Vector2 vec, float deg)
    {
        float c = Mathf.Cos(Mathf.Deg2Rad * deg);
        float s = Mathf.Sin(Mathf.Deg2Rad * deg);
        return new Vector2(vec.x * c - vec.y * s, vec.y * c + vec.x * s);
    }
}
