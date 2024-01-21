using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Two places for items, hotbar and inventory
// Hotbar is a max of 3 items

// TEMP DATABASE MAYBE???
[CreateAssetMenu(fileName = "New Data Table", menuName = "Inventory/Item DataTable")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemBase> items;
}

public enum Rarity
{
    None,
    Common,
    Rare,
    Legendary
}

public enum SoundLevel
{
    None,
    Faint,
    Audible,
    Loud
};

public enum CounterType
{
    None,
    Sight,
    Scent,
    Sound,
    Taste,
    Touch
}

// ITEM CLASS
public class ItemBase : ScriptableObject
{
    [Header("Item")]
    public string ItemID;
    public Rarity rarity;
    public string description;
    public int stackSize;
    public Sprite thumbnail;
    public GameObject prefab;
    [Tooltip("Which monster type to either attract or counter")]
    public CounterType type;
    public float attractionStrength;
    public SoundLevel soundLevel;
}

// ARTIFACT CLASS
[CreateAssetMenu(fileName = "New Artifact", menuName = "Inventory/Items/Artifact")]
public class Artifact : ItemBase
{
    [Header("Artifact")]
    [Tooltip("Artifact cooldown in seconds")]
    public float cooldown;
    [Tooltip("Artifact duration in seconds")]
    public float duration;
}

// CONSUMABLES CLASS
[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Items/Consumable")]
public class Consumable : ItemBase
{
    
}