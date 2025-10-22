using System;
using System.Collections.Generic;
using UnityEditor;
public interface IItemUser
{
    void OnItemUsed(ItemData item);
    // You can define other required methods here
}
[Serializable]
public class ItemData
{
    // Координаты врага. 
    public float posX;
    public float posY;
    // Жив ли враг? Или уже прикидывается трупиком?
    // public bool isAlive = true;
    public IItemUser type;
}

[Serializable]
public class SaveData
{
    // Игрок — координаты, здоровье и батарейка
    public float playerX;
    public float playerY;
    public float health;
    public int battery;

    // Время сохранения
    public long timestamp;
    
    // враги 
    public List<ItemData> enemies = new List<ItemData>();
}
