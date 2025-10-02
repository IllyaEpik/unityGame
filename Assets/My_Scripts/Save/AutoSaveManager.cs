
using UnityEngine;
using System.Collections;

public class AutoSaveManager : MonoBehaviour
{
    // Герой 
    public Hero player;
    public SaveSystem saveSystem;
    
    // Как часто сохранять игру (в секундах). 
    public float intervalSeconds = 60f;

    private Coroutine autoSaveCoroutine;

    private void OnEnable()
    {
        // Запускаем автосохранение
        autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
    }

    private void OnDisable()
    {
        // Останавливаем автосохранение если вдруг выключили объект
        if (autoSaveCoroutine != null)
            StopCoroutine(autoSaveCoroutine);
    }

    IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalSeconds);
            if (player != null && saveSystem != null)
            {
                // Сохраняем игру чтобы не потерять прогресс после очередного босса
                saveSystem.SaveGame(player); 
                Debug.Log("AutoSave completed. (Да-да, мы всё записали)");
            }
        }
    }

    private void OnApplicationQuit()
    {
        // На всякий случай сохраняем игру при выходе. Вдруг свет выключат
        if (player != null && saveSystem != null)
            saveSystem.SaveGame(player);
    }

    private void OnApplicationPause(bool pause)
    {
        // Сохраняем игру, если игрок решил сделать перерыв на чай тип пауза
        if (pause && player != null && saveSystem != null)
            saveSystem.SaveGame(player);
    }
}