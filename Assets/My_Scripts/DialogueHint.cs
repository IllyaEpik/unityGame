using UnityEngine;
using TMPro;

public class DialogueHint : MonoBehaviour
{
    public static DialogueHint Instance;

    [SerializeField] private CanvasGroup hintPanel;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private float visibleTime = 7f;

    private float timer = 0f;
    private bool isVisible = false;

    [Header("Memory Integrity")]
    private int memoryCollected = 0;
    private int maxMemory = 5;

    private void Awake()
    {
        Instance = this;

        if (hintPanel == null)
            hintPanel = GameObject.Find("HintPanel")?.GetComponent<CanvasGroup>();

        if (hintText == null)
            hintText = GameObject.Find("HintText")?.GetComponent<TMP_Text>();

        HideInstant();
    }

    private void Update()
    {
        if (!isVisible) return;

        timer += Time.deltaTime;

        if (timer >= visibleTime)
            HideInstant();
    }

    private void HideInstant()
    {
        hintPanel.alpha = 0;
        hintPanel.blocksRaycasts = false;
        hintPanel.interactable = false;
        hintText.text = "";
        isVisible = false;
        timer = 0f;
    }

    public void ShowHint(string text)
    {
        hintText.text = text;

        hintPanel.alpha = 1;
        hintPanel.blocksRaycasts = true;
        hintPanel.interactable = true;

        isVisible = true;
        timer = 0f;
    }


    public void CollectMemoryShard()
    {
        memoryCollected++;
        int value = Mathf.Clamp(memoryCollected, 0, maxMemory);

        ShowHint($"Memory Integrity частково відновлена {value}/{maxMemory}");
    }

    public int GetMemoryCollected() => memoryCollected;
}
