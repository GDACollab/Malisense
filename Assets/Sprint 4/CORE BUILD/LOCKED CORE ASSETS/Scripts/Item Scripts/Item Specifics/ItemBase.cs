using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Two places for items, hotbar and inventory
// Hotbar is a max of 3 items
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
public abstract class ItemBase : ScriptableObject
{
    [Header("Item")]
    public string ItemID;
    public Rarity rarity;
    [Tooltip("Cost of item in shop")]
    public long cost;
    [TextArea(15, 20)]
    public string description;
    public int stackSize;
    public Sprite thumbnail;
    public GameObject prefab;
    [Tooltip("Which monster type to either attract or counter")]
    public CounterType type;
    public float attractionStrength;
    public SoundLevel soundLevel;
}