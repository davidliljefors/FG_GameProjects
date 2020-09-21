using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class WorldText : MonoBehaviour
    {
        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 10, Color color = default, TextAnchor textAnchor = default, TextAlignment textAlignment = default)
        {

            GameObject obj = new GameObject("World_Text", typeof(TextMesh));

            obj.transform.SetParent(parent, false);
            obj.transform.position = localPosition;
            TextMesh tm = obj.GetComponent<TextMesh>();
            tm.anchor = textAnchor;
            tm.fontSize = fontSize;
            tm.alignment = textAlignment;
            tm.text = text;
            tm.fontSize = fontSize;
            tm.color = color;

            return tm;
        }

        
    }


}
