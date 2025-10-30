using UnityEngine;

public class NpcBot : MonoBehaviour
{
    [Header("Диалог")]
    public DialogueLine[] dialogueLines;

    [Header("Trigger & UI")]
    public GameObject talkButton;

    private bool playerInRange = false;

    private void Start()
    {
        if (talkButton != null) talkButton.SetActive(false);
        if (talkButton != null) talkButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartDialogue);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
            StartDialogue();
    }

    private void StartDialogue()
    {
        if (dialogueLines != null && dialogueLines.Length > 0)
            DialogueManager.Instance.StartDialogue(dialogueLines);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            playerInRange = true;
            if (talkButton != null) talkButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("hero"))
        {
            playerInRange = false;
            if (talkButton != null) talkButton.SetActive(false);
        }
    }
}