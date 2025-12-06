using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
// using NUnit.Framework.Interfaces;
// using ItemDataModel.ItemData;
public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance;

    [Header("Prefabs для загрузки объектов")]
    public GameObject plantPrefab;
    public GameObject enemyBotPrefab;
    public GameObject npcPrefab;
    public GameObject chestPrefab;
    public GameObject keyPrefab;
    public GameObject alienPrefab;

    [Header("Ссылка на игрока")]
    public Hero hero;

    // Массив тегов для удаления объектов на сцене
    private readonly string[] tagsToDelete = { "Plant", "enemy", "NPC", "Chest", "Key", "alien" };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private string GetPath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"save_slot_{slot}.json");
    }

    // ================== СОХРАНЕНИЕ ==================
    public void Save(int slot)
    {
        SaveData data = new SaveData();

        // Игрок
        data.playerX = hero.transform.position.x;
        data.playerY = hero.transform.position.y;
        data.health = hero.health;
        data.battery = hero.battery;

        data.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        data.saveTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

        // Сохраняем объекты на сцене
        SaveSceneObjects(data);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(slot), json);

        Debug.Log($"Игра сохранена в слот {slot}");
    }

    private void SaveSceneObjects(SaveData data)
    {
        SaveType(data, "Plant", plantPrefab);
        SaveType(data, "enemy", enemyBotPrefab);
        SaveType(data, "NPC", npcPrefab);
        SaveType(data, "Chest", chestPrefab);
        SaveType(data, "Key", keyPrefab);
        SaveType(data, "alien", alienPrefab);
    }

    private void SaveType(SaveData data, string tag, GameObject prefab)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            ItemData.ItemData id = new ItemData.ItemData();
            id.type = tag; // сохраняем тег
            id.x = obj.transform.position.x;
            id.y = obj.transform.position.y;
            data.objects.Add(id);
        }
    }

    // ================== ЗАГРУЗКА ==================
    public void Load(int slot)
    {
        string path = GetPath(slot);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Слот {slot} пуст!");
            return;
        }

        string json = File.ReadAllText(path);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Игрок
        hero.transform.position = new Vector2(data.playerX, data.playerY);
        hero.health = data.health;
        hero.battery = data.battery;

        // Удаляем старые объекты
        DeleteAllObjects();

        // Спавним объекты заново
        foreach (ItemData.ItemData id in data.objects)
        {
            SpawnObject(id);
        }

        Debug.Log($"Слот {slot} загружен!");
    }

    private void DeleteAllObjects()
    {
        foreach (string tag in tagsToDelete)
        {
            DeleteByTag(tag);
        }
    }

    private void DeleteByTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objs)
            Destroy(obj);
    }

    private void SpawnObject(ItemData.ItemData id)
    {
        GameObject prefab = null;
        switch (id.type)
        {
            case "Plant": prefab = plantPrefab; break;
            case "enemy": prefab = enemyBotPrefab; break;
            case "NPC": prefab = npcPrefab; break;
            case "Chest": prefab = chestPrefab; break;
            case "Key": prefab = keyPrefab; break;
            case "alien": prefab = alienPrefab; break;
        }

        if (prefab != null)
            Instantiate(prefab, new Vector2(id.x, id.y), Quaternion.identity);
    }

    // ================== СЛОТЫ ==================
    public bool IsSlotOccupied(int slot)
    {
        return File.Exists(GetPath(slot));
    }

    public void Delete(int slot)
    {
        string path = GetPath(slot);
        if (File.Exists(path))
            File.Delete(path);
    }

    public SaveData Peek(int slot)
    {
        string path = GetPath(slot);
        if (!File.Exists(path)) return null;
        return JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
    }
}