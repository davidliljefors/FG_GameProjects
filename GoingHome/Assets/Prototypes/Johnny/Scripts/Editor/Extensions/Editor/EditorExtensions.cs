using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class EditorExtensions
{
    public static void DrawActionsArray(SerializedProperty array, string label)
    {
        GUILayout.BeginVertical("box");
        EditorGUILayout.LabelField(label);

        if (array.arraySize == 0)
        {
            if (GUILayout.Button("Add Actions"))
            {
                array.InsertArrayElementAtIndex(0);
            }
        }

        for (int i = 0; i < array.arraySize; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i), GUIContent.none);

            if (GUILayout.Button("x", GUILayout.Width(20f)))
            {
                array.DeleteArrayElementAtIndex(i);
            }

            if (i == array.arraySize - 1)
            {
                if (GUILayout.Button("+", GUILayout.Width(20f)))
                {
                    array.InsertArrayElementAtIndex(array.arraySize);
                }
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }
}
