using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
	[SerializeField] private GameObject[] m_ObjectsToHighlight;
	private int m_PreviousLayer;
	private int m_HighlightLayer;
	private void Awake()
	{
		m_PreviousLayer = gameObject.layer;
		m_HighlightLayer = LayerMask.NameToLayer("Highlight");
	}

	public void Highlight(bool active)
	{
		if (active)
		{
			foreach (var Go in m_ObjectsToHighlight)
			{
				if (Go != null)
				{
					Go.layer = m_HighlightLayer;
				}
			}

		}
		else
		{
			foreach (var Go in m_ObjectsToHighlight)
			{
				if(Go != null)
				{
					Go.layer = m_PreviousLayer;
				}
			}
		}
	}
}
