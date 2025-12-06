using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ComicIntroManager : MonoBehaviour
{
    [Header("Комикс")]
    public Sprite[] comicFrames;           // массив спрайтов комикса
    public Image backgroundImage;          // Image панели для отображения кадра
    public CanvasGroup comicPanelGroup;    // CanvasGroup для fade-in

    [Header("Настройки")]
    public float fadeDuration = 0.5f;      // время плавного появления
    public string nextSceneName = "test"; // сцена уровня после комикса

    private int currentFrame = 0;
    private bool isFading = false;

    void Start()
    {
        if (comicFrames.Length == 0) return;

        // показать первый кадр
        currentFrame = 0;
        ShowFrame(currentFrame);
    }

    void Update()
    {
        // Пробел = следующий кадр
        if (Input.GetKeyDown(KeyCode.Space) && !isFading)
        {
            currentFrame++;

            if (currentFrame < comicFrames.Length)
            {
                ShowFrame(currentFrame);
            }
            else
            {
                // конец комикса = загрузка уровня
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }

    void ShowFrame(int frameIndex)
    {
        backgroundImage.sprite = comicFrames[frameIndex];
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        isFading = true;
        float t = 0;
        comicPanelGroup.alpha = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            comicPanelGroup.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        comicPanelGroup.alpha = 1;
        isFading = false;
    }
}