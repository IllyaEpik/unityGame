using UnityEngine;

public class AutoSaveManager : MonoBehaviour
{
    [Header("Автосейв")]
    [SerializeField] private float saveInterval = 30f; // Интервал автосейва в секундах
    [SerializeField] private int autoSaveSlot = 0; // Слот для автосейва (0, 1 или 2)

    private float timer = 0f;

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= saveInterval)
        {
            timer = 0f;
            PerformAutoSave();
        }
    }

    private void PerformAutoSave()
    {
        if (SaveSystem.Instance != null)
        {
            SaveSystem.Instance.Save(autoSaveSlot);
            Debug.Log($"Автосейв выполнен в слот {autoSaveSlot}");
        }
        else
        {
            Debug.LogWarning("SaveSystem.Instance не найден! Автосейв не выполнен.");
        }
    }
}