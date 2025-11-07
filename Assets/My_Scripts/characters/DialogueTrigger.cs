using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Данные диалога")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("Настройки")]
    public bool autoStartOnTrigger = true;
    public bool startOnlyOnce = false;
   [SerializeField] private Button talkButton;

    private bool triggered = false;
    internal Action onDialogueEnd;

    void Start()
    {
        talkButton.gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (triggered) return;

        if (other.CompareTag("hero"))
        {
            if (talkButton == null)
            {
                TriggerDialogue();
                if (startOnlyOnce) triggered = true;
                return;
            }
            talkButton.gameObject.SetActive(true);
            
        }
    }

    // можешь вызвать вручную из другого скрипта:
    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
    }

    void OnDialogueEnd()
    {
        // сюда можно добавить действия после окончания диалога
        Debug.Log($"Диалог завершён на объекте {gameObject.name}");
    }
}