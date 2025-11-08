using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI елементи")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;

    private DialogueLine[] currentDialogue;
    private HashSet<string> usedOneTimeResponses = new HashSet<string>();
    private int currentLine = 0;
    private bool dialogueActive = false;

    // 🔹 Подія, яка викликається після закінчення діалогу, з номером останнього рядка
    private Action<int> onDialogueEnd;

    void Awake()
    {
        Instance = this;
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        foreach (var b in choiceButtons) b.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] dialogue, Action<int> endAction = null)
    {
        if (dialogue == null || dialogue.Length == 0) return;

        currentDialogue = dialogue;
        currentLine = 0;
        dialogueActive = true;
        onDialogueEnd = endAction;

        dialoguePanel.SetActive(true);
        ShowLine(currentLine);
    }

    void ShowLine(int lineIndex)
    {
        if (lineIndex < 0 || lineIndex >= currentDialogue.Length)
        {
            EndDialogue();
            return;
        }

        currentLine = lineIndex;
        dialogueText.text = currentDialogue[lineIndex].npcText;
        ShowChoices(currentDialogue[lineIndex].responses);
    }

    void ShowChoices(PlayerResponse[] responses)
    {
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < responses.Length)
            {
                var resp = responses[i];
                var btn = choiceButtons[i];
                var txt = choiceTexts[i];

                btn.gameObject.SetActive(true);
                txt.text = resp.responseText;

                string key = currentLine + "_" + i;
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
                    int choiceIndex = i;
                    btn.onClick.AddListener(() => OnPlayerChoice(responses[choiceIndex], choiceIndex));
                }
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerChoice(PlayerResponse chosen, int choiceIndex)
    {
        if (chosen.oneTime)
        {
            string key = currentLine + "_" + choiceIndex;
            usedOneTimeResponses.Add(key);
        }

        chosen.onChosen?.Invoke();

        if (chosen.nextLineIndex >= 0)
            ShowLine(chosen.nextLineIndex);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        dialogueActive = false;
        dialoguePanel.SetActive(false);
        foreach (var b in choiceButtons) b.gameObject.SetActive(false);

        // 🔹 Викликаємо подію з параметром (яка лінія була останньою)
        onDialogueEnd?.Invoke(currentLine);
    }

public int GetCurrentLineIndex()
{
    return currentLine;
}
}