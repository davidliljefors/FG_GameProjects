using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimateAction))]

public class AnimateActionEditor : Editor
{
    SerializedProperty anims, actions;

    private void OnEnable()
    {
        anims = serializedObject.FindProperty("anims");
        actions = serializedObject.FindProperty("actions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Add Animation Parameter"))
        {
            anims.InsertArrayElementAtIndex(anims.arraySize);
        }

        //draw the anims inspector
        for (int i = 0; i < anims.arraySize; i++)
        {
            DrawAnimsInspector(anims.GetArrayElementAtIndex(i), i);
        }

        EditorExtensions.DrawActionsArray(actions, "Actions Chaining:");

        serializedObject.ApplyModifiedProperties();
    }

    void DrawAnimsInspector(SerializedProperty entry, int id)
    {
        GUILayout.BeginVertical("box");

        GUILayout.BeginHorizontal();

        EditorGUILayout.PropertyField(entry.FindPropertyRelative("triggerName"), new GUIContent("Trigger Name:"));

        if (GUILayout.Button("X", GUILayout.Width(20f)))
        {
            anims.DeleteArrayElementAtIndex(id);
            return;
        }

        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(entry.FindPropertyRelative("invokeDelay"), new GUIContent("Delay (in sec):"));
        GUILayout.EndVertical();
    }
}
