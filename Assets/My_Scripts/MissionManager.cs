using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public GameObject enemy;
    public Hero hero;

    public bool isActive = true;
    public bool isCompleted = false;
    public bool isFailed = false;

    // Нове поле — текст поточного завдання
    [HideInInspector] public string currentTaskText = "Вбий ворога";

    void Update()
    {
        if (!isActive || isCompleted || isFailed)
            return;

        if (hero != null && hero.health <= 0)
        {
            isFailed = true;
            isActive = false;
            Debug.Log("Місію провалено — герой загинув");
            return;
        }

        if (enemy == null)
        {
            isCompleted = true;
            isActive = false;
            Debug.Log("Місію виконано — ворога знищено");
        }
    }

    // Метод для оновлення тексту завдання
    public void SetTaskText(string newText)
    {
        currentTaskText = newText;
        Debug.Log($"Завдання оновлено: {newText}");
    }
}