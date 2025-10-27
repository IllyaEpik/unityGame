
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

public class SaveSystem : MonoBehaviour
{
    // Путь к файлу сохранения.
    private string savePath;
    public surfaceGenerator surfaceGenerator;
    private void Awake()
    {
        // Формируем путь к сейву. 
        savePath = Path.Combine(Application.persistentDataPath, "save.json");
        Debug.Log("Save path: " + savePath);
    }
    // Сохраняем игрока и ВСЕХ врагов типа EnemyPlant на сцене.
    private SaveData SaveMob<T>(SaveData data)  where T : MonoBehaviour
    {
        T[] currentEnemies = FindObjectsOfType<T>();
        foreach (T e in currentEnemies)
        {
            // Проверяем, что ссылка не "мертвая" (на всякий случай а то баги)
            if (e == null) continue;

            ItemData ed = new ItemData();
            ed.posX = e.transform.position.x;
            ed.posY = e.transform.position.y;
            // ed.type = T;
            // ed.isAlive = e.isAlive;
            data.enemies.Add(ed);
        }
        return data;
    }
    public void SaveGame(Hero player) 
    {
        SaveData data = new SaveData();

        // Сохраняем координаты, здоровье и батарейку игрока
        data.playerX = player.transform.position.x;
        data.playerY = player.transform.position.y;
        data.health = player.health;
        data.battery = player.battery;
        data.timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        data.seed = surfaceGenerator.seed;
        // Враги
        // Находим ВСЕХ активных врагов типа EnemyPlant на сцене прямо сейчас 
        SaveMob<EnemyPlant>(data);
        // SaveMob<enemyBot>(data);
        // SaveMob<NpcBot>(data);
        // SaveMob<Chest>(data);
        // SaveMob<Key>(data);
        
        // Превращаем всё в JSON и записываем на диск.
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved вроде!");
    }

    // Загружаем игрока и врагов из prefab.
    public bool LoadGame(Hero player, GameObject enemyPrefab)
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("сейва нет"); // Не нашли сейв — не беда, можно начать сначала!
            return false;
        }
        
        // Читаем JSON и воскрешаем всех из цифрового небытия
        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);
        surfaceGenerator.LoadSeed(data.seed);
        // Восстанавливаем игрока. 
        player.transform.position = new Vector2(data.playerX, data.playerY);
        player.health = data.health;
        player.battery = data.battery;

        // Удаляем старых врагов типа EnemyPlant со сцены. елси не удалить все полетит 
        foreach (EnemyPlant old in FindObjectsOfType<EnemyPlant>()) 
        {
            Destroy(old.gameObject);
        }
        // Создаём врагов из prefab. 
        foreach (ItemData ed in data.enemies)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, new Vector2(ed.posX, ed.posY), Quaternion.identity);
            EnemyPlant e = enemyObj.GetComponent<EnemyPlant>(); 
            
            // восстанавливаем состояние isAlive
            // e.isAlive = ed.isAlive; 
            
            // если в враг здох то деактив 
            // if (!ed.isAlive)
            //     enemyObj.SetActive(false);
        }

        Debug.Log("Game loaded плз донт краш!");
        return true;
    }
}