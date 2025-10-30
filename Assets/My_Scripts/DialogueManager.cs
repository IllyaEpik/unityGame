using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private TMP_Text[] choiceTexts;

    private DialogueLine[] currentLines;
    private int currentLineIndex = 0;
    private Action<int> endCallback;
    private HashSet<string> usedOneTimeResponses = new HashSet<string>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        foreach (var b in choiceButtons)
            if (b != null) b.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueLine[] lines, Action<int> endAction = null)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentLineIndex = 0;
        endCallback = endAction;

        dialoguePanel.SetActive(true);
        ShowLine(currentLineIndex);
    }

    private void ShowLine(int index)
    {
        if (currentLines == null || index < 0 || index >= currentLines.Length)
        {
            EndDialogue();
            return;
        }

        currentLineIndex = index;
        dialogueText.text = currentLines[index].npcText;
        ShowChoices();
    }

    private void ShowChoices()
    {
        var responses = currentLines[currentLineIndex].responses ?? new PlayerResponse[0];

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < responses.Length)
            {
                var btn = choiceButtons[i];
                var txt = choiceTexts[i];
                var resp = responses[i];

                btn.gameObject.SetActive(true);
                txt.text = resp.responseText;

                string key = currentLineIndex + "_" + i;
                btn.interactable = !(resp.oneTime && usedOneTimeResponses.Contains(key));

                btn.onClick.RemoveAllListeners();
                int choiceIndex = i;
                btn.onClick.AddListener(() => OnChoiceSelected(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnChoiceSelected(int choiceIndex)
    {
        var resp = currentLines[currentLineIndex].responses[choiceIndex];

        if (resp.oneTime)
            usedOneTimeResponses.Add(currentLineIndex + "_" + choiceIndex);

        if (resp.nextLineIndex >= 0)
            ShowLine(resp.nextLineIndex);
        else
            EndDialogue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        foreach (var b in choiceButtons)
            b.gameObject.SetActive(false);

        endCallback?.Invoke(currentLineIndex);
    }
}