using System;

namespace ItemData
{
    [Serializable]
    public class ItemData
    {
        public string type;   // тип объекта (Plant, Chest, Key, NPC, EnemyBot...)
        public float x;
        public float y;
    }
}