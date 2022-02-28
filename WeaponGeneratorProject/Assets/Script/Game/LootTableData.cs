using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LootData
{
    public ItemType ItemType;
    public int amount;
}

[CreateAssetMenu(fileName = "New LootData", menuName = "Game/Data/LootData")]
public class LootTableData : ScriptableObject
{
    public LootData[] loot;
}
