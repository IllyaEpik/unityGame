using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Данные диалога")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("Настройки")]
    public bool autoStartOnTrigger = true;
    public bool startOnlyOnce = false;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!autoStartOnTrigger || triggered) return;

        if (other.CompareTag("hero"))
        {
            DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
            if (startOnlyOnce) triggered = true;
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