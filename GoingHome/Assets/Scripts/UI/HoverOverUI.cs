using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [System.NonSerialized] public GameObject AliveChar = null;
    [System.NonSerialized] public TurnManager TurnMngr = null;

    public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
    {
		if (AliveChar.TryGetComponent<Highlightable>(out var highlightable))
		{
			highlightable.Highlight(true);
		}

	}

    public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
    {
		if (AliveChar.TryGetComponent<Highlightable>(out var highlightable))
		{
			highlightable.Highlight(false);
		}
    }
}

