using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerResponse
{
    [TextArea(1, 3)]
    public string responseText; // Текст варианта игрока
    public int nextLineIndex;   // К какому элементу диалога перейти (-1 = конец)
    public bool oneTime = false; // Если true — можно выбрать только один раз за игру
    public UnityEvent onChosen;  // 🔹 Дія, яка виконується при виборі
}