using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemType",menuName = "Item/ItemTypeData")]
public class ItemTypeData : ScriptableObject
{
    public ItemType itemType;
    public ItemCategory category;
    public Sprite icon;
    public string displayName;
}
