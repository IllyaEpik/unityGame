using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueTeleport : MonoBehaviour
{
    [Header("Посилання")]
    [SerializeField] private CanvasGroup fadePanel; // Чорний екран (CanvasGroup на чорному Image)
    [SerializeField] private TMP_Text centerText;   // Текст по центру
    [SerializeField] private Transform player;      // Гравець, якого треба телепортувати
    [SerializeField] private Vector3 teleportPosition; // Координати, куди телепортувати
    [SerializeField] private GameObject botObject;  // Бот, який зникає (червоний)
    
    [Header("Префаб для спавну")]
    [SerializeField] private GameObject blueBotPrefab;  // Префаб синього бота
    [SerializeField] private Vector3 blueBotSpawnPos;   // Координати спавну синього бота

    [Header("Налаштування")]
    [SerializeField] private float fadeSpeed = 1.5f; // Швидкість затемнення
    [SerializeField] private float textDelay = 7f;   // Час, поки текст видно на екрані

    public void OnDialogueEnd()
    {
        StartCoroutine(FadeAndTeleport());
    }

    private IEnumerator FadeAndTeleport()
    {
        // Затемнення екрана
        fadePanel.gameObject.SetActive(true);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = t;
            yield return null;
        }

        // Показати текст
        centerText.text = "Слідкування за B-17.";
        centerText.gameObject.SetActive(true);

        //  2.1 — Прибрати бота після затемнення (в момент чорного екрану)
        if (botObject != null)
        {
            botObject.SetActive(false);
            Debug.Log("Червоний бот зник після діалогу.");
        }

        // Чекаємо 7 секунд, поки гравець читає текст
        yield return new WaitForSeconds(textDelay);

        // Телепортація гравця
        player.position = teleportPosition;

        // 3.1 — Спавн синього бота
        if (blueBotPrefab != null)
        {
            Instantiate(blueBotPrefab, blueBotSpawnPos, Quaternion.identity);
            Debug.Log("Синій бот зʼявився після телепортації.");
        }

        // Зникнення чорного екрана
        yield return new WaitForSeconds(1f);
        while (t > 0)
        {
            t -= Time.deltaTime * fadeSpeed;
            fadePanel.alpha = t;
            yield return null;
        }

        fadePanel.gameObject.SetActive(false);
        centerText.gameObject.SetActive(false);
    }
}