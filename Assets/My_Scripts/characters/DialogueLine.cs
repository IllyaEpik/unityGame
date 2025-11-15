using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string npcText;
    public PlayerResponse[] responses;
}
