using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotItemPrefab : MonoBehaviour
{

    public Image itemImage;
    public TextMeshProUGUI itemText;
    public ItemType blockType;
    //public CraftingPannel craftingpanel;


    private void Awake()
    {
        //if (!craftingpanel)
        //{
        //    craftingpanel = FindObjectOfType<CraftingPannel>(true);
        //}
    }

    public void ItemSetting(Sprite itemSprite, string txt, ItemType type)
    {
        itemImage.sprite = itemSprite;
        itemText.text = txt;
        blockType = type;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right) return;

        //if (!craftingpanel) return;

        //craftingpanel.AddPlanned(blockType, 1);
    }
}
