using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Дані діалогу")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("Підказки після певних рядків")]
    [SerializeField] private bool enableHints = true;

    [Header("Телепорт після певного рядка")]
    [SerializeField] private DialogueTeleport teleportTarget;
    [SerializeField] private int teleportAfterLine = -1;

    [Header("Спавн об'єкта після певного рядка")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int spawnAfterLine = -1;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private bool destroySelfAfterSpawn = false;

    [Header("Настройки")]
    [SerializeField] private bool autoStartOnTrigger = true;
    [SerializeField] private bool startOnlyOnce = false;
    [SerializeField] private Button talkButton;

    private bool triggered = false;
    private bool hintShown0 = false;
    private bool hintShown1 = false;
    private bool teleportTriggered = false;
    private bool spawnTriggered = false;

    void Start()
    {
        if (talkButton != null)
            talkButton.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("hero"))
        {
            if (autoStartOnTrigger && talkButton == null)
            {
                TriggerDialogue();
                if (startOnlyOnce) triggered = true;
                return;
            }

            if (talkButton != null)
            {
                talkButton.gameObject.SetActive(true);
                talkButton.onClick.RemoveAllListeners();
                talkButton.onClick.AddListener(() =>
                {
                    TriggerDialogue();
                    talkButton.gameObject.SetActive(false);
                });
            }
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueLines, OnDialogueEnd);
    }

    // 🔹 Викликається коли діалог завершується
    void OnDialogueEnd(int lastLine)
    {
        Debug.Log($"Діалог завершено ({gameObject.name}), останній рядок: {lastLine}");

        // Телепорт
        if (teleportTarget != null && lastLine == teleportAfterLine)
        {
            Debug.Log("Телепорт після рядка " + lastLine);
            teleportTarget.OnDialogueEnd();
        }

        // Спавн об'єкта
        if (!spawnTriggered && objectToSpawn != null && lastLine == spawnAfterLine)
        {
            spawnTriggered = true;
            Vector3 spawnPos = spawnPosition != null ? spawnPosition.position : transform.position;
            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            Debug.Log($"Spawned {objectToSpawn.name} at {spawnPos}");

            if (destroySelfAfterSpawn)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (DialogueManager.Instance == null) return;

        int current = DialogueManager.Instance.GetCurrentLineIndex();

        // Підказки
        if (enableHints)
        {
            if (!hintShown0 && current == 1)
            {
                DialogueHint.Instance.ShowHint("Memory Integrity: 0/5 — розпочати реконструкцію памʼяті");
                hintShown0 = true;
            }
            else if (!hintShown1 && current == 4)
            {
                DialogueHint.Instance.ShowHint("Memory Integrity: 2/5");
                hintShown1 = true;
                enableHints = false;
            }
        }

        // Телепорт (у процесі, якщо треба)
        if (!teleportTriggered && teleportTarget != null && teleportAfterLine >= 0 && current == teleportAfterLine)
        {
            teleportTriggered = true;
            teleportTarget.OnDialogueEnd();
        }

        // Спавн (у процесі, якщо треба)
        if (!spawnTriggered && objectToSpawn != null && spawnAfterLine >= 0 && current == spawnAfterLine)
        {
            spawnTriggered = true;
            Vector3 spawnPos = spawnPosition != null ? spawnPosition.position : transform.position;
            Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
            Debug.Log($"Spawned {objectToSpawn.name} at {spawnPos}");

            if (destroySelfAfterSpawn)
            {
                Destroy(gameObject);
            }
        }
    }
}
