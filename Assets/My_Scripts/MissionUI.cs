using TMPro;
using UnityEngine;

public class MissionScrollUI : MonoBehaviour
{
    public TextMeshProUGUI taskText;

    void Start()
    {
        UpdateMissionText();
    }

    void Update()
    {
        //Обновляем, если текст миссии изменился
        if (MissionManager.Instance != null)
        {
            taskText.text = MissionManager.Instance.currentTaskText;
        }
    }

    public void UpdateMissionText()
    {
        if (MissionManager.Instance != null)
            taskText.text = MissionManager.Instance.currentTaskText;
        else
            taskText.text = "Немає активних завдань";
    }
}