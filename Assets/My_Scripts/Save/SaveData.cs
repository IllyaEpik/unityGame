using System;
using System.Collections.Generic;

[Serializable]
public class EnemyData
{
    // Координаты врага. 
    public float posX;
    public float posY;
    // Жив ли враг? Или уже прикидывается трупиком?
    public bool isAlive;
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
    public List<EnemyData> enemies = new List<EnemyData>();
}
