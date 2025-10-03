using UnityEngine;
using UnityEngine.UI; 
using System.Collections; 

public class death : MonoBehaviour
{
    
    public Hero playerHero;

   
    public CanvasGroup gameOverPanelGroup; 
    public CanvasGroup gameOverPanelGroup1; 
    
    
    public float fadeDuration = 3f; 
    private bool isFading = false; 

    void Start()
    {
       
        if (gameOverPanelGroup != null || gameOverPanelGroup1 != null)
        {
            
            if (gameOverPanelGroup != null)
            {
                gameOverPanelGroup.alpha = 0f;
                gameOverPanelGroup.blocksRaycasts = false; 
            }
            if (gameOverPanelGroup1 != null)
            {
                gameOverPanelGroup1.alpha = 0f;
                gameOverPanelGroup1.blocksRaycasts = false; 
            }
        }
    }

    void Update()
    {
        // Добавлена проверка playerHero на null
        if (playerHero == null)
        {
            return;
        }
        if (playerHero.health > 0)
        {
            if (gameOverPanelGroup != null)
            {
                gameOverPanelGroup.alpha = 0f;
                gameOverPanelGroup.blocksRaycasts = false; 
            }
            if (gameOverPanelGroup1 != null)
            {
                gameOverPanelGroup1.alpha = 0f;
                gameOverPanelGroup1.blocksRaycasts = false; 
            }
        }

        // 5. Проверяем условие смерти и убеждаемся, что анимация еще не запущена
        if (playerHero.health <= 0 && !isFading)
        {
            Debug.Log("Герой умер! Запускаем экраны Game Over.");
            
            isFading = true;
            
            // 💡 Включаем блокировку ввода на главном экране ТОЛЬКО перед началом появления
            if (gameOverPanelGroup != null)
            {
                gameOverPanelGroup.blocksRaycasts = true; 
            }
            
            StartFadeIn(0f, 1f, fadeDuration);
        }
    }

    // Вспомогательный метод для запуска корутины
    public void StartFadeIn(float startAlpha, float endAlpha, float duration)
    {
        // Запускаем корутину для первого элемента, если он есть
        if (gameOverPanelGroup != null)
        {
            StartCoroutine(FadeCanvasGroup(gameOverPanelGroup, startAlpha, endAlpha, duration));
        }
        
        // Запускаем корутину для второго элемента, если он есть
        if (gameOverPanelGroup1 != null)
        {
            StartCoroutine(FadeCanvasGroup(gameOverPanelGroup1, startAlpha, endAlpha, duration));
        }
    }

    // --- Корутина для плавного изменения прозрачности Canvas Group ---
    
    IEnumerator FadeCanvasGroup(CanvasGroup groupToFade, float startAlpha, float endAlpha, float duration)
    {
        // Дополнительная проверка на null внутри корутины
        if (groupToFade == null)
        {
            yield break; // Выход, если компонент не установлен
        }

        float startTime = Time.time;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed = Time.time - startTime;
            
            float t = Mathf.Clamp01(elapsed / duration); 
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, t);

            // Меняем прозрачность
            groupToFade.alpha = newAlpha;

            yield return null; 
        }

        // Убеждаемся, что альфа установлена в конечное значение
        groupToFade.alpha = endAlpha;
        
        // По завершении: отключаем блокировку ввода, если экран снова становится прозрачным
        groupToFade.blocksRaycasts = (endAlpha == 1f); 
    }
}