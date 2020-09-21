using UnityEngine;
using UnityEngine.Assertions;

public class MenuCursorChanger : MonoBehaviour
{
    [SerializeField] private Texture2D m_DefaultCursor = null;
    [SerializeField] private Texture2D m_DefaultCursorClicked = null;

    private void Start()
    {
        Assert.IsNotNull(m_DefaultCursor, "No cursor assigned");
        Assert.IsNotNull(m_DefaultCursorClicked, "No cursor assigned");
        Cursor.SetCursor(m_DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            Cursor.SetCursor(m_DefaultCursorClicked, new Vector2(20,20), CursorMode.ForceSoftware);
        }
        else
        {
            Cursor.SetCursor(m_DefaultCursor, Vector2.zero, CursorMode.ForceSoftware);
        }
    }
}
