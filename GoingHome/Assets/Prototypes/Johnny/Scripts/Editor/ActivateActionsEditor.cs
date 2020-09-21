using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActivateActions))]
public class ActivateActionsEditor : Editor
{
    SerializedProperty customGOList;

    private void OnEnable()
    {
        customGOList = serializedObject.FindProperty("customGameObjects");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Add Entry"))
        {
            customGOList.InsertArrayElementAtIndex(customGOList.arraySize);
        }

        DrawCustomObjectFields(customGOList);

        serializedObject.ApplyModifiedProperties();
    }

    void DrawCustomObjectFields(SerializedProperty customList)
    {
        for (int i = 0; i < customList.arraySize; i++)
        {
            GUILayout.BeginHorizontal("box");

            EditorGUILayout.PropertyField(customList.GetArrayElementAtIndex(i).FindPropertyRelative("gO"), new GUIContent("GameObject:"));

            GUILayout.BeginVertical(GUILayout.Width(25f));
            EditorGUILayout.PropertyField(customList.GetArrayElementAtIndex(i).FindPropertyRelative("activeStatus"), GUIContent.none, GUILayout.Width(25f));

            if (GUILayout.Button("X", GUILayout.Width(20f)))
            {
                customList.DeleteArrayElementAtIndex(i);
            }
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }

}
