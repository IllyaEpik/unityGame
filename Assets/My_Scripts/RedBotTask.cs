using UnityEngine;

public class RedBotTask : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private DialogueTrigger dialogueTrigger; 
    [SerializeField] private MissionManager missionManager;

    private bool missionGiven = false;

    private void Start()
    {
        if (dialogueTrigger == null)
            dialogueTrigger = GetComponent<DialogueTrigger>();

        // if (dialogueTrigger != null)
        //     dialogueTrigger.onDialogueEnd += OnDialogueEnd;
    }

    private void OnDestroy()
    {
        // if (dialogueTrigger != null)
        //     dialogueTrigger.onDialogueEnd -= OnDialogueEnd;
    }

    public void OnDialogueEnd()
    {
        if (missionGiven) return;

        if (missionManager != null)
        {
            // missionManager.ActivateMission();
            Debug.Log("Місія активована після діалогу з Червоним ботом.");
            missionGiven = true;
        }
    }
}