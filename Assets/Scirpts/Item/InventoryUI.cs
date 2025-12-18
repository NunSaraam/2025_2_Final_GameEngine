using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class ItemDatabase
{
    static Dictionary<ItemType, ItemTypeData> itemMap = new();

    public static void Register(ItemType type, ItemTypeData data)
    {
        itemMap[type] = data;
    }

    public static ItemTypeData Get(ItemType type)
    {
        itemMap.TryGetValue(type, out var data);
        return data;
    }
}

public class InventoryUI : MonoBehaviour
{
    public Sprite dirtSprite;
    public Sprite grassSprite;
    public Sprite woodSprite;
    public Sprite leafSprite;
    public Sprite plankSprite;

    public Sprite stoneSprite;
    public Sprite coalSprite;
    public Sprite ironSprite;
    public Sprite diamondSprite;

    public Sprite stickSprite;
    public Sprite ironAxeSprite;
    public Sprite ironPickaxeSprite;
    public Sprite diamondAxeSprite;
    public Sprite diamondPickaxeSprite;

    public Sprite workBenchSprite;

    public Sprite appleSprite;

    public Sprite simpleWaterPurifierSprite;

    public List<Transform> slot = new List<Transform>();
    public GameObject slotItem;
    List<GameObject> items = new List<GameObject>();


    float rightClickHoldTime = 1f;
    float rightClickTimer = 0f;

    public int selectedIndex = -1;

    [SerializeField] private PlayerStats playerStats;
    private Inventory inventory;

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }

        HandleRightClickUse();
    }


    public void SetSelectedIndex(int index)
    {
        Resetselection();
        if (selectedIndex == index)
        {
            selectedIndex = -1;
        }
        else
        {
            if (index >= items.Count)
            {
                selectedIndex = -1;
            }
            else
            {
                SetSelection(index);
                selectedIndex = index;
            }
        }
    }
    public void Resetselection()
    {
        foreach (var slot in slot)
        {
            slot.GetComponent<Image>().color = Color.white;
        }
    }

    void SetSelection(int index)
    {
        slot[index].GetComponent<Image>().color = Color.yellow;
    }

    public ItemType GetInventorySlot()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count)
            return ItemType.None; // 혹은 적절한 기본값

        return items[selectedIndex].GetComponent<SlotItemPrefab>().blockType;
    }

    void HandleRightClickUse()
    {
        if (selectedIndex < 0 || selectedIndex >= items.Count) return;

        if (Input.GetMouseButtonDown(1)) // 우클릭 누름
        {
            rightClickTimer += Time.deltaTime;

            if (rightClickTimer >= rightClickHoldTime)
            {
                ItemType type = GetInventorySlot();
                var data = ItemDatabase.Get(type);
                if (data is Food food)
                {
                    food.Use(playerStats, inventory);
                    UpdateInventory(inventory);
                }

                rightClickTimer = 0f;
            }
        }
        else
        {
            rightClickTimer = 0f;
        }
    }

    public void UpdateInventory(Inventory myInven)
    {
        foreach (var slotItems in items)
        {
            Destroy(slotItems);
        }

        items.Clear();

        int index = 0;

        foreach (var item in myInven.items)
        {
            var go = Instantiate(slotItem, slot[index].transform);
            go.transform.localScale = Vector3.one;
            SlotItemPrefab sItem = go.GetComponent<SlotItemPrefab>();
            items.Add(go);

            switch (item.Key)
            {
                case ItemType.Dirt:
                    sItem.ItemSetting(dirtSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Grass:
                    sItem.ItemSetting(grassSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Wood:
                    sItem.ItemSetting(woodSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Plank:
                    sItem.ItemSetting(plankSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.WorkBench:
                    sItem.ItemSetting(workBenchSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Leaf:
                    sItem.ItemSetting(leafSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Stone:
                    sItem.ItemSetting(stoneSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Coal:
                    sItem.ItemSetting(coalSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Iron:
                    sItem.ItemSetting(ironSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Diamond:
                    sItem.ItemSetting(diamondSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Stick:
                    sItem.ItemSetting(stickSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.IronPickAxe:
                    sItem.ItemSetting(ironPickaxeSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.IronAxe:
                    sItem.ItemSetting(ironAxeSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.DiamondPickAxe:
                    sItem.ItemSetting(diamondPickaxeSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.DiamondAxe:
                    sItem.ItemSetting(diamondAxeSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.apple:
                    sItem.ItemSetting(appleSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.SimpleWaterPurifier:
                    sItem.ItemSetting(simpleWaterPurifierSprite, "x" + item.Value.ToString(), item.Key);
                    break;
            }

            ItemTypeData data = myInven.itemDataList.Find(x => x.itemType == item.Key);

            if (data != null)
            {
                sItem.ItemSetting(data.icon, "x" + item.Value.ToString(), data.itemType);
            }

            index++;
        }
    }
}
