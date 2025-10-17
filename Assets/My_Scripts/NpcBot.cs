using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string npcText;             // Что говорит NPC
    public string[] playerResponses;   // Варианты ответов игрока
}

public class NpcBot : MonoBehaviour
{
    [Header("Диалоговые данные")]
    [SerializeField] public DialogueLine[] dialogueLines; // Реплики NPC с ответами

    [Header("UI элементы")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button talkButton;

    [Header("Кнопки выбора ответа")]
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;

    // Это просто для проверки во время разработки:
    void Reset()
    {
        // Reset сработает при добавлении компонента или на Reset в инспекторе.
        dialogueLines = new DialogueLine[2];

        dialogueLines[0] = new DialogueLine();
        dialogueLines[0].npcText = "Привет, я NPC. Что хочешь?";
        dialogueLines[0].playerResponses = new string[] { "Привет!", "Кто ты?" };

        dialogueLines[1] = new DialogueLine();
        dialogueLines[1].npcText = "Хорошо, спасибо за ответ.";
        dialogueLines[1].playerResponses = new string[] { "Пока", "Еще что-нибудь?" };
    }

    void Start()
    {
        if (talkButton != null) talkButton.gameObject.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        if (talkButton != null)
            talkButton.onClick.AddListener(OnTalkButtonPressed);

        if (choiceButtons != null)
        {
            foreach (var btn in choiceButtons)
                if (btn != null) btn.gameObject.SetActive(false);
        }
    }

    // Остальной код как у тебя...
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
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (dialogueText != null && dialogueLines.Length > 0) dialogueText.text = dialogueLines[currentLine].npcText;
        ShowChoices();
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            if (dialogueText != null) dialogueText.text = dialogueLines[currentLine].npcText;
            ShowChoices();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowChoices()
    {
        if (choiceButtons == null || choiceTexts == null) return;
        var choices = dialogueLines[currentLine].playerResponses;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceTexts[i].text = choices[i];

                int index = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnPlayerChoice(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerChoice(int choiceIndex)
    {
        Debug.Log("Игрок выбрал: " + dialogueLines[currentLine].playerResponses[choiceIndex]);
        NextLine();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (choiceButtons != null)
            foreach (var btn in choiceButtons)
                if (btn != null) btn.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            playerInRange = true;
            if (talkButton != null) talkButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            playerInRange = false;
            if (talkButton != null) talkButton.gameObject.SetActive(false);
            EndDialogue();
        }
    }

    void OnTalkButtonPressed()
    {
        if (!dialogueActive) StartDialogue();
        else NextLine();
    }
}