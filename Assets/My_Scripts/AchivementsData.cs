using System;

[System.Serializable]
public class AchievementData
{
    public string id;           // уникальный идентификатор
    public string title;        // заголовок ачивки
    public string description;  // описание ачивки
    public bool achieved;       // выполнено или нет
}