using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml.Serialization;
using System;

[System.Serializable]
public class PlayerResponse
{
    [TextArea(1, 3)]
    public string responseText; // Текст варианта игрока
    public int nextLineIndex;   // К какому элементу diálogo перейти (-1 = конец)
    public bool oneTime = false; // Если true — можно выбрать только один раз за игру (сессия)

}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string npcText; // Реплика NPC
    public PlayerResponse[] responses; // Варианты ответов игрока
}

public class NpcBot : MonoBehaviour
{
    
    
    [Header("Диалоговые данные")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("UI елементи")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button talkButton;

    [Header("Кнопки вибору відповіді")]
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;

    // Служебное: ключи использованных одноразовых ответов
    private HashSet<string> usedOneTimeResponses = new HashSet<string>();

    private int currentLine = 0;
    private bool playerInRange = false;
    private bool dialogueActive = false;
    public System.Action<int> endAction;
    void Start()
    {
        if (talkButton != null) talkButton.gameObject.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (talkButton != null) talkButton.onClick.AddListener(OnTalkButtonPressed);

        if (choiceButtons != null)
        {
            foreach (var b in choiceButtons)
                if (b != null) b.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueActive) StartDialogue();
        }
    }

    void StartDialogue()
    {
        if (dialogueLines == null || dialogueLines.Length == 0) return;

        dialogueActive = true;
        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        ShowLine(currentLine);
    }

    void ShowLine(int lineIndex)
    {
        if (dialogueLines == null) return;

        if (lineIndex < 0 || lineIndex >= dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        currentLine = lineIndex;
        Debug.Log(dialogueLines[currentLine].npcText);
        if (dialogueText != null) dialogueText.text = dialogueLines[currentLine].npcText;

        ShowChoices();
    }

    void ShowChoices()
    {
        if (choiceButtons == null || choiceTexts == null) return;

        var responses = dialogueLines[currentLine].responses ?? new PlayerResponse[0];

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < responses.Length)
            {
                var resp = responses[i];
                var btn = choiceButtons[i];
                var txt = choiceTexts[i];

                btn.gameObject.SetActive(true);
                txt.text = resp.responseText;

                // уникальный ключ для одноразового ответа: "lineIndex_responseIndex"
                string key = currentLine + "_" + i;

                // если одноразовый и уже использован -> делаем неинтерактивным
                if (resp.oneTime && usedOneTimeResponses.Contains(key))
                {
                    btn.interactable = false;
                    txt.text = resp.responseText + " (використано)";
                    btn.onClick.RemoveAllListeners();
                }
                else
                {
                    btn.interactable = true;
                    btn.onClick.RemoveAllListeners();
                    int choiceIndex = i; // локальная копия для замыкания
                    btn.onClick.AddListener(() => OnPlayerChoice(choiceIndex));
                }
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerChoice(int choiceIndex)
    {
        var responses = dialogueLines[currentLine].responses;
        if (responses == null || choiceIndex < 0 || choiceIndex >= responses.Length) return;

        var chosen = responses[choiceIndex];

        // если одноразовый — отметим как использованный
        if (chosen.oneTime)
        {
            string key = currentLine + "_" + choiceIndex;
            usedOneTimeResponses.Add(key);
        }

        // Переходим к следующей линии. Если nextLineIndex == -1 -> конец диалога.
        if (chosen.nextLineIndex >= 0)
            ShowLine(chosen.nextLineIndex);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        if (choiceButtons != null)
            foreach (var b in choiceButtons)
                if (b != null) b.gameObject.SetActive(false);
        endAction(currentLine);
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
    }
}