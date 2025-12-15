using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<ItemType, int> items = new();
    public List<ItemTypeData> itemDataList;

    public InventoryUI inventoryUI;

    public int maxStack = 64;

    private void Start()
    {
        LoadFromGameData();

        inventoryUI = FindObjectOfType<InventoryUI>();
        inventoryUI.UpdateInventory(this);
    }

    void LoadFromGameData()
    {
        if (GameDataManager.Instance == null) return;

        if (GameDataManager.Instance.inventoryData != null)
        {
            items = new Dictionary<ItemType, int>(GameDataManager.Instance.inventoryData);
            Debug.Log("[Load] Inventory loaded!");
        }
    }

    public int GetCount(ItemType type)
    {
        items.TryGetValue(type, out var count);
        return count;
    }

    public void Add(ItemType type, int count = 1)
    {
        if (!items.ContainsKey(type)) items[type] = 0;
        items[type] += count;
        Debug.Log($"[inventory] +{count} {type} ( {items[type]}");
        inventoryUI.UpdateInventory(this);
    }

    public bool Consume(ItemType type, int count = 1)
    {
        if (!items.TryGetValue(type, out var have) || have < count) return false;

        items[type] = have - count;
        Debug.Log($"[inventory] -{count} {type} ( {items[type]}");
        if (items[type] == 0)
        {
            items.Remove(type);
            inventoryUI.selectedIndex = -1;
            inventoryUI.Resetselection();
        }

        inventoryUI.UpdateInventory(this);
        return true;
    }
}
