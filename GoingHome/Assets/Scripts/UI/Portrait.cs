using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private enum Character
    {
        Mickey,
        Ticky,
        Stitchy
    }

    [SerializeField] private Sprite[] damagedPortraits;
    private int currentPortraitIndex;
    private float m_Health;
    private float m_MaxHealth;
    public Health characterPrefab;

    [SerializeField] private Character characterType;
    [SerializeField] private Animator hudAnim;

    [HideInInspector] public bool b_StateOne;
    [HideInInspector] public bool b_StateTwo;
    [HideInInspector] public bool b_StateThree;

    private void Start()
    {
        characterPrefab.OnHealthChanged += ChangeMicePortrait;
    }


    public void ChangeMicePortrait()
    {
        m_Health = (float)characterPrefab.GetHeatlh();
        m_MaxHealth = (float)characterPrefab.GetMaxHeatlh();
        float normalizedHealth = m_Health / m_MaxHealth;
        Debug.Log(normalizedHealth);
        
        if(normalizedHealth >= 1f)
            ChangePortrait(3);
        else if (normalizedHealth >= 0.50f)
            ChangePortrait(2);
        else if (normalizedHealth >= 0.25f)
            ChangePortrait(1);
        else if (normalizedHealth <= 0f)
            ChangePortrait(0);
    }

    public void ChangePortrait(int state)
    {
            if (characterType == Character.Mickey)
            { hudAnim.SetTrigger("MickeyDmg"); }  
            else if (characterType == Character.Ticky)
            { hudAnim.SetTrigger("TickyDmg"); }
            else if (characterType == Character.Stitchy)
            { hudAnim.SetTrigger("StitchyDmg"); }
           
            gameObject.GetComponent<Image>().sprite = damagedPortraits[state];
    }
}
