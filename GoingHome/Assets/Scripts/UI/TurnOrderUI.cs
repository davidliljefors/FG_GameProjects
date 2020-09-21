using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Collections;

public class TurnOrderUI : MonoBehaviour
{
    [SerializeField] private Image[] m_Borders = null;
    [SerializeField] private Image[] m_Portraits = null;

    [SerializeField] private Sprite m_PlayerBorder = null;
    [SerializeField] private Sprite m_EnemyBorder = null;
  

    TurnManager m_TurnManager = null;

    [SerializeField] private float m_Scale = 1f;
    [SerializeField] private float m_TransitionTime = 1f;

    Vector2 originalImageSize = default;
    Vector2 originalPortraitSize = default; 


    void Awake()
    {
        Assert.IsNotNull(m_Borders, "Assign borders to TurnOrderUI");
        Assert.IsNotNull(m_Portraits, "Assign Portraits to TurnOrderUI");
        Assert.IsNotNull(m_PlayerBorder, "Assign border to TurnOrderUI");
        Assert.IsNotNull(m_EnemyBorder, "Assign border to TurnOrderUI");


        m_TurnManager = FindObjectOfType<TurnManager>();
        Assert.IsNotNull(m_TurnManager, "Can't Find Turn Manager");
        m_TurnManager.OnTurnDataChanged += RedrawUI;

        originalImageSize = m_Borders[0].rectTransform.sizeDelta;
        originalPortraitSize = m_Borders[0].rectTransform.sizeDelta;


        foreach (Image i in m_Borders)
        {
            i.gameObject.SetActive(false);
        }
        foreach (Image i in m_Portraits)
        {
            i.gameObject.SetActive(false);
        }
    }


    private void RedrawUI()
    {
        var objs = m_TurnManager.AliveCharacters;

        foreach (Image i in m_Borders)
        {
            i.rectTransform.sizeDelta = originalImageSize;
            i.color = Color.white;
            i.gameObject.SetActive(false);
        }
        foreach (Image i in m_Portraits)
        {
            i.rectTransform.sizeDelta = originalPortraitSize * 1.4f;
            i.gameObject.SetActive(false);
        }

        for (int i = 0; i < objs.Count; i++)
        {
            m_Portraits[i].sprite = objs[i].GetComponent<ITurnPawn>().Portrait;
            m_Borders[i].sprite = objs[i].CompareTag("Player") ? m_PlayerBorder : m_EnemyBorder;

            if (m_Portraits[i].TryGetComponent(out HoverOverUI hoverOverUI))
            {
                hoverOverUI.AliveChar = objs[i];
            }

            m_Portraits[i].gameObject.SetActive(true);
            m_Borders[i].gameObject.SetActive(true);
        }

        if (m_TurnManager.CurrentTurn >= 0)
        {
        //    m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = new Vector2(scale, scale);
        //    m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = new Vector2(scale, scale);
            
            StartCoroutine(LerpToScale(m_TransitionTime));
            
            //m_Borders[m_TurnManager.CurrentTurn].color = Color.yellow;
        }

        //m_SelectedBorder.transform.position = m_Borders[m_TurnManager.CurrentTurn].transform.position;
    }
    IEnumerator LerpToScale(float transitionTime)
    {
        float elapsedTime = 0f;
        Vector2 targetBorders = m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta * m_Scale;
        Vector2 originBorders = m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta;
        Vector2 targetPortraits = m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta * m_Scale;
        Vector2 originalPortraits = m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta ;

        Vector2 smallerBorders = m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta * (m_Scale * 0.8f);
        Vector2 smallerPortraits = m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta * m_Scale;

        while (true)
        {
            if(elapsedTime > transitionTime * 0.83f)
            {
                m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = targetPortraits;
                m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = targetBorders;
                if (m_TurnManager.CurrentTurn > 0)
                {
                    m_Portraits[m_TurnManager.CurrentTurn - 1].rectTransform.sizeDelta = originalPortraits;
                    m_Borders[m_TurnManager.CurrentTurn - 1].rectTransform.sizeDelta = originBorders;
                }
                break;
            }
            Vector2 smoothBordersScale = Vector3.Lerp(originBorders, targetBorders, elapsedTime / (transitionTime * 0.83f));
            Vector2 smoothPortraitsScale = Vector3.Lerp(originalPortraits, targetPortraits, elapsedTime / (transitionTime * 0.83f));
            
            m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smoothPortraitsScale;
            m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smoothBordersScale;

            if(m_TurnManager.CurrentTurn > 0)
            {
                m_Portraits[m_TurnManager.CurrentTurn - 1].rectTransform.sizeDelta = Vector3.Lerp(smallerPortraits, originalPortraits, elapsedTime / (transitionTime * 0.83f));
                m_Borders[m_TurnManager.CurrentTurn - 1].rectTransform.sizeDelta = Vector3.Lerp(smallerBorders, originBorders, elapsedTime / (transitionTime * 0.83f));
            }

            for (int i = 1; i < m_Borders.Length; i++)
            {
                if (i != 0)
                {
                    Vector2 imagePos = new Vector2(m_Borders[i - 1].rectTransform.anchoredPosition.x + m_Borders[i - 1].rectTransform.sizeDelta.x, m_Borders[i].rectTransform.anchoredPosition.y);
                    m_Borders[i].rectTransform.anchoredPosition = imagePos;
                }
            }


            elapsedTime += Time.deltaTime;
            yield return null;
        }
        


        elapsedTime = 0f;
        while (true)
        {
            if(elapsedTime > transitionTime * 0.17f)
            {
                m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smallerPortraits;
                m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smallerBorders;
                break;
            }
            LerpBackToScale(transitionTime * 0.17f, targetBorders, smallerBorders, smallerPortraits, targetPortraits, ref elapsedTime);
            yield return null;
        }
        
    }

    void LerpBackToScale(float transitionTime, Vector2 targetBorders, Vector2 smallerBorders, Vector2 smallerPortraits, Vector2 targetPortraits, ref float elapsedTime)
    {
        Vector2 smoothBordersScale = Vector3.Lerp(targetBorders, smallerBorders, elapsedTime / transitionTime);
        Vector2 smoothPortraitsScale = Vector3.Lerp(targetPortraits, smallerPortraits, elapsedTime / transitionTime);

        m_Portraits[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smoothPortraitsScale; //new Vector2(scale, scale);
        m_Borders[m_TurnManager.CurrentTurn].rectTransform.sizeDelta = smoothBordersScale; //new Vector2(scale, scale);

        for (int i = 1; i < m_Borders.Length; i++)
        {
            if (i != 0)
            {
                Vector2 imagePos = new Vector2(m_Borders[i - 1].rectTransform.anchoredPosition.x + m_Borders[i - 1].rectTransform.sizeDelta.x, m_Borders[i].rectTransform.anchoredPosition.y);
                m_Borders[i].rectTransform.anchoredPosition = imagePos;
            }
        }

        elapsedTime += Time.deltaTime;
    }
}
