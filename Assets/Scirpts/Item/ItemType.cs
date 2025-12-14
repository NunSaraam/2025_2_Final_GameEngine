using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType 
{
    // Blocks
    Dirt,
    Grass,
    Water,
    Cloud,

    // Tools
    Sword,
    Axe,

    // BUilding
    Furnace,
    Generator,

    // Resources
    Wood,
    Stone,
    Iron,
    Crystal
}

public enum ItemCategory
{
    Block,
    Tool,
    Structure,
    Resource
}

public class ItemTypeData
{
    public ItemType itemType;
    public ItemCategory category;
    public Sprite icon;
    public string displayName;
}