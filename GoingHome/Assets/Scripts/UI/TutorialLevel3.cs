using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevel3 : MonoBehaviour
{
    public GameObject tutorialUi;
    public TextMeshProUGUI tutorialText;
    private AudioSource audioSource;
    public DialogueManager dialogueManager;
    private Image m_windowImage;
    public GameObject traitsWindow;
    //public GameObject playerDialogueBubble;
    //public GameObject NpcDialogueBubble;
    //public GameObject goal;

    public Transform foodParent;
    public GameObject[] foodToPickup;
    private int m_pickedUpFood;
    

    [TextArea(8, 50)] public string firstText;
    public Sprite image1;
    [TextArea] public string secondText;
    public Sprite image2;
    [TextArea] public string thirdText;
    public Sprite image3;

    private bool m_firstPartActive;
    private bool m_secondPartActive;

    private Animator m_boxAnimator;
    private bool shouldOpenTraits;

    private void Start()
    {
        tutorialUi.SetActive(true);
        m_windowImage = tutorialUi.transform.Find("Content/Image").GetComponent<Image>();
        tutorialUi.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        m_boxAnimator = tutorialUi.GetComponent<Animator>();
    }

    private IEnumerator startTutorial(string text, Sprite image)
    {
        yield return new WaitForSeconds(0.8f);

        audioSource.Play();
        tutorialUi.SetActive(true);
        tutorialText.text = text;
        
        if(image != null)
        {
            m_windowImage.sprite = image;
            m_windowImage.gameObject.SetActive(true);
            Debug.Log("image != null");
        }
        else
        {
            m_windowImage.gameObject.SetActive(false);
            Debug.Log("image == null");
        }
        
    }

    public IEnumerator FadeOutBox()
    {
        m_boxAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.3f);

        if(shouldOpenTraits)
        {
            traitsWindow.SetActive(true);
            shouldOpenTraits = false;
        }
        else
        {
            tutorialUi.SetActive(false);
        }
    }
    public void CloseTraitsWindow()
    {
        StartCoroutine(FadeOutTraits());
    }
    public IEnumerator FadeOutTraits()
    {
        traitsWindow.GetComponent<Animator>().SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.4f);
        traitsWindow.SetActive(false);
        tutorialUi.SetActive(false);

    }

    //public void OpenTraitsWindow()
    //{
    //    traitsWindow.SetActive(true);
    //}

    private void Update()
    {
        if(dialogueManager.dialogueOver == true && !m_firstPartActive)
        {
            m_firstPartActive = true;
            StartCoroutine(startTutorial(firstText, image1));
            shouldOpenTraits = true;
        }
        m_pickedUpFood = 0;
        for (int i = 0; i < foodToPickup.Length; i++)
        {
            if(foodToPickup[i] == null)
            {
                m_pickedUpFood++;
            }
        }
        
        if (m_pickedUpFood == foodToPickup.Length && !m_secondPartActive)
        {
            m_secondPartActive = true;
            StartCoroutine(FadeOutBox());
            StartCoroutine(startTutorial(secondText, image2));
        }
    }

    public void CloseTutorWindow()
    {
        StartCoroutine(FadeOutBox());
        
    }
}
