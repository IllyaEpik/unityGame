using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public event Action<int> OnDialogueEndedPublic;

    [Header("UI елементи")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;

    private DialogueLine[] currentDialogue;
    private HashSet<string> usedOneTimeResponses = new HashSet<string>();
    private int currentLine = 0;
    private bool dialogueActive = false;
    private Action<int> onDialogueEnd;

    [Header("Оновлення завдання (Task Update)")]
    [SerializeField] private bool allowTaskUpdate = false;          
    [SerializeField] private int updateTaskAfterLine = 2;          
    [SerializeField] private string newTaskText = "Йди до центру прийняття рішень";
    [SerializeField] private Transform newTaskTarget;               
    [SerializeField] private TaskPanelManager taskPanelManager;     

    void Awake()
    {
        Instance = this;
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        foreach (var b in choiceButtons)
            b.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] dialogue, Action<int> endAction = null)
    {
        if (dialogue == null || dialogue.Length == 0)
            return;

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

        // Перевірка чи потрібно оновити завдання
        if (allowTaskUpdate && lineIndex == updateTaskAfterLine)
        {
            UpdateTask();
            allowTaskUpdate = false;
        }
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

        foreach (var b in choiceButtons)
            b.gameObject.SetActive(false);

        onDialogueEnd?.Invoke(currentLine);

        OnDialogueEndedPublic?.Invoke(currentLine);
    }

    public int GetCurrentLineIndex()
    {
        return currentLine;
    }

    private void UpdateTask()
    {
        if (taskPanelManager != null)
        {
            taskPanelManager.UpdateTask(newTaskText, newTaskTarget);
            Debug.Log("Завдання оновлено через TaskPanelManager: " + newTaskText);
        }
        else
        {
            Debug.LogWarning("TaskPanelManager не призначено у DialogueManager!");
        }
    }
}
