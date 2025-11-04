using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarknessBlock : MonoBehaviour
{
    private Light2D globalLight;
    private float normalIntensity;
    private bool isDark = false; // Текущее состояние
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float darkMultiplier = 0.33f;

    private bool playerInside = false; // чтобы не срабатывало многократно
    private bool playerWasInside = false;

    private void Start()
    {
        globalLight = FindFirstObjectByType<Light2D>();

        if (globalLight == null)
        {
            Debug.LogError("Global Light 2D не найден");
            enabled = false;
            return;
        }

        normalIntensity = globalLight.intensity;
    }

    private void Update()
    {
        // Проверяем — если игрок вышел после того, как был внутри
        if (playerWasInside && !playerInside)
        {
            // Проход завершён
            ToggleLight();
            playerWasInside = false;
        }

        playerInside = false; // сбрасываем, чтобы отметить что игрок уже не внутри
    }

    private void FixedUpdate()
    {
        // можно плавно менять яркость
        float target = isDark ? normalIntensity * darkMultiplier : normalIntensity;
        globalLight.intensity = Mathf.Lerp(globalLight.intensity, target, Time.deltaTime * fadeSpeed);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("hero"))
        {
            playerInside = true;
            playerWasInside = true;
        }
    }

    private void ToggleLight()
    {
        isDark = !isDark; // Переключаем состояние
        Debug.Log("Смена света: " + (isDark ? "Тьма" : "Свет"));
    }
}