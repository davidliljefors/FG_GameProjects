using System.Data;
using UnityEditor;
using UnityEngine;

public class HighlightSelection : MonoBehaviour
{
	[SerializeField] private LayerMask m_MouseOverLayerMask;
	[SerializeField] private LayerMask m_GroundLayer;

	private GameObject m_LastHighlight = null;
	private Camera m_Camera;
	private int m_HighlightLayer = -1;
	private int m_LastLayer = -1;
	private Ray m_CameraToMouseRay;

	private Vector3 lastMouseWorldPos = default;

	private void Awake()
	{
		m_Camera = Camera.main;
		m_HighlightLayer = LayerMask.NameToLayer("Highlight");
	}

	private void Update()
	{
		if (m_LastHighlight != null)
		{
			m_LastHighlight.layer = m_LastLayer;
		}


		m_CameraToMouseRay = m_Camera.ScreenPointToRay(Input.mousePosition);

		{
			if (Physics.Raycast(m_CameraToMouseRay, out RaycastHit outHit, 1000f, m_MouseOverLayerMask))
			{
				m_LastHighlight = outHit.collider.gameObject;
				m_LastLayer = m_LastHighlight.layer;
				m_LastHighlight.layer = m_HighlightLayer;
			}
		}

		{
			if (Physics.Raycast(m_CameraToMouseRay, out RaycastHit outHit, 1000f, m_GroundLayer))
			{

				lastMouseWorldPos = outHit.point;
			}
		}
	}
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Vector3 squarepos = lastMouseWorldPos.Round();
		Handles.color = Color.yellow;
		Handles.DrawAAPolyLine(squarepos, squarepos + Vector3.forward, squarepos + Vector3.forward + Vector3.right, squarepos + Vector3.right, squarepos);
	}
#endif
}
