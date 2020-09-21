using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialLevel2 : MonoBehaviour
{
    public GameObject tutorialUi;
    private TextMeshProUGUI tutorialText;
    private AudioSource audioSource;
    public GameObject dialogueManager;
    public GameObject enemyCharacters;
    public GameObject enemyPrefab;
    public Transform enemySpawnPoint;
    public GameObject goal;
    private DialogueManager m_dialogueScript;
    private Image m_windowImage;

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

    private Animator m_boxAnimator;

    private void Start()
    {
        m_windowImage = tutorialUi.transform.Find("Content/Image").GetComponent<Image>();
        tutorialUi.SetActive(false);
        tutorialText = tutorialUi.GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
        m_dialogueScript = dialogueManager.GetComponent<DialogueManager>();
        m_boxAnimator = tutorialUi.GetComponent<Animator>();
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

    public IEnumerator FadeOutBox()
    {
        m_boxAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.3f);
        tutorialUi.SetActive(false);
    }
    private void Update()
    {
        if(m_dialogueScript.dialogueOver == true && !m_firstPartActive)
        {
            m_firstPartActive = true;
            StartCoroutine(startTutorial(firstText, image1));
        }

        if(objectToPickUp == null && !m_secondPartActive)
        {
            m_secondPartActive = true;
            StartCoroutine(FadeOutBox());
            //tutorialUi.SetActive(false);
            StartCoroutine(startTutorial(secondText, image2));
            Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity, enemyCharacters.transform);
            enemyCharacters.GetComponent<EnemySpawn>().shouldSpawn = true;
            enemyCharacters.GetComponent<EnemySpawn>().SpawnEnemies();

        }

        if (enemyCharacters.GetComponent<EnemySpawn>().shouldSpawn && enemyCharacters.transform.childCount < 1 && !m_thirdPartActive) 
        {
            m_thirdPartActive = true;
            //tutorialUi.SetActive(false);
            StartCoroutine(FadeOutBox());
            StartCoroutine(startTutorial(thirdText, image3));
            enemyCharacters.GetComponent<EnemySpawn>().shouldSpawn = false;
            goal.SetActive(true);
        }

    }
    public void CloseTutorWindow()
    {
        StartCoroutine(FadeOutBox());
    }

}
