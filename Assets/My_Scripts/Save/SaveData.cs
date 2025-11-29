using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public float playerX;
    public float playerY;
    public float health;
    public int battery;

    public long timestamp;
    public string saveTime;  // строка вида "12.01.2025 14:22"

    public List<ItemData> objects = new List<ItemData>();
}