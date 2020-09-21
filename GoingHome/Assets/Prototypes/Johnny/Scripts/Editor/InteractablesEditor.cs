using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Interactable))]
public class InteractablesEditor : Editor
{
    SerializedProperty s_actions, s_distancePosition, s_spriteCursor, s_lookOnly;

    private void OnEnable()
    {
        s_actions = serializedObject.FindProperty("actions");
        s_distancePosition = serializedObject.FindProperty("distancePosition");
        s_spriteCursor = serializedObject.FindProperty("spriteCursor");
        s_lookOnly = serializedObject.FindProperty("lookOnly");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.BeginVertical("box");

        s_spriteCursor.objectReferenceValue = EditorGUILayout.ObjectField("Sprite Cursor", s_spriteCursor.objectReferenceValue, typeof(Sprite), false, GUILayout.Height(75f));

        EditorGUILayout.PropertyField(s_lookOnly, new GUIContent("Look Only"));

        EditorGUILayout.PropertyField(s_distancePosition, new GUIContent("Distance Position: "));

        EditorExtensions.DrawActionsArray(s_actions, "Actions: ");

        GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
