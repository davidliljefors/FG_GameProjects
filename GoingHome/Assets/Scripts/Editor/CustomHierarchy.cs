using UnityEditor;
using UnityEngine;

namespace EditorTools
{
	[InitializeOnLoad]
	public class CustomHierarchy : MonoBehaviour
	{
		public static GUIStyle guiStyle = new GUIStyle() { fontStyle = FontStyle.Bold };
		public static Color backgroundColor = Color.green;

		private static CustomHierarchy instance;

		static CustomHierarchy()
		{
			EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyItemOnGUI;
		}

		private static void HandleHierarchyItemOnGUI(int instanceID, Rect selectionRect)
		{
			GameObject instanceObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (!instanceObject || !instanceObject.CompareTag("EditorOnly"))
			{
				return;
			}

			EditorGUI.DrawRect(selectionRect, backgroundColor);
			EditorGUI.LabelField(selectionRect, instanceObject.name, guiStyle);
		}
	}
}