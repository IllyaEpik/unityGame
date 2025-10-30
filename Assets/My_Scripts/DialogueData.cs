using UnityEngine;

[System.Serializable]
public class PlayerResponse
{
    [TextArea(1, 3)]
    public string responseText;   // Текст варианта игрока
    public int nextLineIndex = -1; // К какому элементу диалога перейти (-1 = конец)
    public bool oneTime = false;  // Одноразовый выбор
}

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string npcText;           // Реплика NPC
    public PlayerResponse[] responses; // Варианты игрока
}