using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueHint : MonoBehaviour
{
    public static DialogueHint Instance;

    [SerializeField] private CanvasGroup hintPanel;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float visibleTime = 7f;

    private int hintCount = 0; // Счётчик показанных подсказок

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 🔹 Автоматический поиск, если поля не заданы
        if (hintPanel == null)
        {
            GameObject panelObj = GameObject.Find("HintPanel");
            if (panelObj != null)
                hintPanel = panelObj.GetComponent<CanvasGroup>();
        }

        if (hintText == null)
        {
            GameObject textObj = GameObject.Find("HintText");
            if (textObj != null)
                hintText = textObj.GetComponent<TMP_Text>();
        }

        // 🔹 Безопасная инициализация
        if (hintPanel != null)
        {
            hintPanel.alpha = 0;
            hintPanel.gameObject.SetActive(false);
        }
    }

    public void ShowHint(string text)
    {
        if (hintPanel == null || hintText == null)
        {
            Debug.LogWarning("⚠️ DialogueHint: HintPanel или HintText не найдены!");
            return;
        }

        StopAllCoroutines();
        StartCoroutine(ShowHintRoutine(text));
    }

    private IEnumerator ShowHintRoutine(string text)
    {
        hintCount++;
        hintPanel.gameObject.SetActive(true);
        hintText.text = text;

        // 🔹 Плавное появление
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            hintPanel.alpha = t;
            yield return null;
        }

        // 🔹 Если это вторая подсказка — подождать 7 секунд
        if (hintCount == 2)
        {
            yield return new WaitForSeconds(visibleTime);

            // 🔹 Плавное исчезновение
            while (t > 0)
            {
                t -= Time.deltaTime * fadeSpeed;
                hintPanel.alpha = t;
                yield return null;
            }

            hintPanel.gameObject.SetActive(false);
        }
    }
}
