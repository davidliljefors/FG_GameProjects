using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Must;

public class PathCostText : MonoBehaviour
{
    [SerializeField] Color m_GoodColour = Color.yellow;
    [SerializeField] Color m_BadColour = Color.red;
    
    private TextMeshPro m_Text;
    private Camera m_Camera;



    private void Awake()
    {
        m_Camera = Camera.main;
        m_Text = GetComponent<TextMeshPro>();
        Assert.IsNotNull(m_Text, "PathCostText: text not found");
        m_Text.text = "";
    }

    public void SetText(Vector3 worldpos, in string text)
    {
		transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
		transform.position = worldpos;
        m_Text.text = text;
        m_Text.enabled = true;
    }

    public void SetCost(Vector3 worldpos, int cost, int movesLeft)
    {
        transform.rotation = Quaternion.LookRotation(transform.position - m_Camera.transform.position);
        transform.position = worldpos;
        m_Text.color = cost <= movesLeft ? m_GoodColour : m_BadColour;
        if(movesLeft == -1)
        {
            m_Text.color = m_GoodColour;
            m_Text.text = cost + "/" + '∞';
        }
        else
        {
            m_Text.text = cost + "/" + movesLeft;
        }
        m_Text.enabled = true;
    }

    public void Clear()
    {
        m_Text.text = string.Empty;
        m_Text.enabled = false;
    }
}
