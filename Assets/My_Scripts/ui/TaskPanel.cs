using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskPanelManager : MonoBehaviour
{
    [Header("Елементи інтерфейсу")]
    [SerializeField] private Button taskButton;      // Кнопка для відкриття панелі
    [SerializeField] private GameObject taskPanel;   // Панель
    [SerializeField] private TMP_Text taskText;      // Назва завдання
    [SerializeField] private TMP_Text statusText;    // Текст статусу завдання
    [SerializeField] private TMP_Text directionText; // Напрямок
    [SerializeField] private TMP_Text coordsText;    // Координати

    [Header("Посилання")]
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private Transform player;
    [SerializeField] private Transform taskTarget;

    private void Start()
    {
        taskPanel.SetActive(false);
        taskButton.onClick.AddListener(TogglePanel);
    }

    private void Update()
    {
        if (missionManager == null) return;

        UpdateStatus();
        UpdateDirection();
        UpdateCoordinates();
        UpdateTaskText();
    }

    private void TogglePanel()
    {
        bool isActive = taskPanel.activeSelf;
        taskPanel.SetActive(!isActive);

        if (!isActive)
        {
            UpdateTaskText();
        }
    }

    private void UpdateTaskText()
    {
        if (taskText == null) return;

        if (missionManager.isCompleted || missionManager.isFailed || !missionManager.isActive)
            taskText.text = "Місій для тебе поки що немає";
        else
            taskText.text = missionManager.currentTaskText; // 🔹 Беремо з MissionManager
    }

    private void UpdateStatus()
    {
        if (statusText == null) return;

        if (missionManager.isCompleted)
            statusText.text = "Завдання виконано";
        else if (missionManager.isFailed)
            statusText.text = "Завдання провалено";
        else if (missionManager.isActive)
            statusText.text = "Завдання в процесі";
        else
            statusText.text = "Завдання відсутнє";
    }

    private void UpdateDirection()
    {
        if (player == null || directionText == null)
            return;

        if (missionManager.isCompleted || missionManager.isFailed || !missionManager.isActive)
        {
            directionText.text = "";
            return;
        }

        if (taskTarget == null)
        {
            directionText.text = "";
            return;
        }

        Vector3 offset = taskTarget.position - player.position;

        string direction;
        if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            direction = offset.x > 0 ? "Іди вправо" : "Іди вліво";
        else
            direction = offset.y > 0 ? "Іди вгору" : "Іди вниз";

        directionText.text = direction;
    }

    private void UpdateCoordinates()
    {
        if (coordsText == null || player == null) return;

        Vector3 playerPos = player.position;
        coordsText.text = $"Гравець: X={playerPos.x:F1}, Y={playerPos.y:F1}";

        if (taskTarget != null && missionManager.isActive)
        {
            Vector3 targetPos = taskTarget.position;
            coordsText.text += $"\n Ціль: X={targetPos.x:F1}, Y={targetPos.y:F1}";
        }
    }

public void SetCustomTask(string newTask)
{
    if (taskText != null)
        taskText.text = newTask;
}
}