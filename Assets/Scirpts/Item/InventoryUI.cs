using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{

    public Sprite dirtSprite;
    public Sprite diamondSprite;
    public Sprite grassSprite;
    public Sprite waterSprite;
    public Sprite cloudSprite;
    public Sprite swordSprite;

    public List<Transform> slot = new List<Transform>();
    public GameObject slotItem;
    List<GameObject> items = new List<GameObject>();

    public int selectedIndex = -1;

    private void Update()
    {
        for (int i = 0; i < Mathf.Min(9, slot.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SetSelectedIndex(i);
            }
        }
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
        return items[selectedIndex].GetComponent<SlotItemPrefab>().blockType;
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
                case ItemType.Water:
                    sItem.ItemSetting(waterSprite, "x" + item.Value.ToString(), item.Key);
                    break;
                case ItemType.Sword:
                    sItem.ItemSetting(swordSprite, "x" + item.Value.ToString(), item.Key);
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
