using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NpcBot : MonoBehaviour
{
    [TextArea(2, 5)]
    [SerializeField] private string[] dialogueLines; // Тексты реплик NPC

    [Header("UI элементы")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button talkButton;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    void Start()
    {
        talkButton.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);

        // Подключаем функцию при нажатии
        talkButton.onClick.AddListener(OnTalkButtonPressed);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueActive)
                StartDialogue();
            else
                NextLine();
        }
    }

    void StartDialogue()
    {
        dialogueActive = true;
        currentLine = 0;
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            talkButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            talkButton.gameObject.SetActive(false);
            EndDialogue();
        }
    }

    void OnTalkButtonPressed()
    {
        if (!dialogueActive)
            StartDialogue();
        else
            NextLine();
    }
}
