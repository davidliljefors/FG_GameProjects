using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevel1 : MonoBehaviour
{
    public GameObject tutorialUi;
    private TextMeshProUGUI tutorialText;
    private AudioSource audioSource;
    private Transform m_player;
    public GameObject dialogueManager;
    private DialogueManager m_dialogueScript;
    public Transform goal;
    public GameObject exit;
    private Image m_windowImage;

    public Transform spawnPoint;

    public GameObject enemyPrefab;
    public Transform enemyParent;

    private Animator m_boxAnimator;

    public GameObject objectToPickUp;
    [TextArea] public string firstText;
    public Sprite image1;
    [TextArea] public string secondText;
    public Sprite image2;
    [TextArea] public string thirdText;
    public Sprite image3;

    private bool m_firstPartActive;
    private bool m_secondPartActive;
    private bool m_thirdPartActive;

    private void Start()
    {
        m_windowImage = tutorialUi.transform.Find("Content/Image").GetComponent<Image>();
        tutorialUi.SetActive(false);
        tutorialText = tutorialUi.GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_dialogueScript = dialogueManager.GetComponent<DialogueManager>();
        m_boxAnimator = tutorialUi.GetComponent<Animator>();
        exit.SetActive(false);
        
    }

    private void Update()
    {
        if(m_dialogueScript.dialogueOver == true && !m_firstPartActive)
        {
            m_firstPartActive = true;
            StartCoroutine(startTutorial(firstText, image1));
        }


        if(objectToPickUp == null && !m_secondPartActive && m_firstPartActive)
        {
            StartCoroutine(FadeOutBox());
            m_secondPartActive = true;
            //tutorialUi.SetActive(false);
            //m_boxAnimator.SetTrigger("FadeOut");
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation, enemyParent);
            StartCoroutine(startTutorial(secondText, image2));
            exit.SetActive(true);



        }

        if (m_player != null && !m_thirdPartActive && m_secondPartActive)
        {
            if (Vector3.Distance(m_player.position, goal.position) < 7 && !m_thirdPartActive)
            {
                m_thirdPartActive = true;
                StartCoroutine(FadeOutBox());

                StartCoroutine(startTutorial(thirdText, image3));
            }
        }

    }

    public IEnumerator FadeOutBox()
    {
        Debug.Log("Closing tutorial Window");
        m_boxAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.3f);
        tutorialUi.SetActive(false);
    }

    private IEnumerator startTutorial(string text, Sprite image)
    {
        yield return new WaitForSeconds(0.8f);

        audioSource.Play();
        tutorialUi.SetActive(true);
        tutorialText.text = text;
        
        

        if (image != null)
        {
            m_windowImage.sprite = image;
            m_windowImage.gameObject.SetActive(true);
            m_windowImage.preserveAspect = true;
            Debug.Log("image != null");
        }
        else
        {
            m_windowImage.gameObject.SetActive(false);
            Debug.Log("image == null");
        }
    }

    public void CloseTutorWindow()
    {
        StartCoroutine(FadeOutBox());
    }
}
