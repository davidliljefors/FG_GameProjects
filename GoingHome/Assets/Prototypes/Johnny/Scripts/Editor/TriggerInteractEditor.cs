using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TriggerInteract))]
public class TriggerInteractEditor : Editor
{
    SerializedProperty s_enterActions, s_exitActions, s_selectedTag;

    private void OnEnable()
    {
        s_enterActions = serializedObject.FindProperty("enterActions");
        s_exitActions = serializedObject.FindProperty("exitActions");
        s_selectedTag = serializedObject.FindProperty("selectedTag");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        s_selectedTag.stringValue = EditorGUILayout.TagField("Trigger Tag: ", s_selectedTag.stringValue);

        EditorExtensions.DrawActionsArray(s_enterActions, "TriggerEnter Actions:");
        EditorExtensions.DrawActionsArray(s_exitActions, "TriggerExit Actions:");

        serializedObject.ApplyModifiedProperties();
    }

}
