using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string npcText; // Реплика NPC
    public PlayerResponse[] responses; // Варианты ответов игрока
}
