using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingPanel : MonoBehaviour
{
    public Inventory inventory;
    public List<CraftingRecipe> recipeList;
    public GameObject craftingpanel;
    public TMP_Text plannedText;
    public Button craftBUtton;
    public Button clearButton;
    public TMP_Text hintText;

    public CraftingType allowedCraftingType = CraftingType.PlayerOnly;

    readonly Dictionary<ItemType, int> planned = new();
    bool isOpen;

    private void Start()
    {
        SetOpen(false);
        craftBUtton.onClick.AddListener(DoCraft);
        clearButton.onClick.AddListener(ClearPlanned);
        RefreshPlannedUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePanel(CraftingType.PlayerOnly);
        }
    }

    public void TogglePanel(CraftingType type)
    {
        allowedCraftingType = type;
        SetOpen(!isOpen);
    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        if (craftingpanel)
        {
            craftingpanel.SetActive(open);
            Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = open;
        }

        if (!open) ClearPlanned();
    }

    public void AddPlanned(ItemType type, int count = 1)
    {
        if (!planned.ContainsKey(type)) planned[type] = 0;
        planned[type] += count;

        RefreshPlannedUI();
        SetHint($"{type} x{count} 추가 완료");
    }

    public void ClearPlanned()
    {
        planned.Clear();
        RefreshPlannedUI();
        SetHint("초기화 완료");
    }

    private void RefreshPlannedUI()
    {
        if (!plannedText) return;

        if (planned.Count == 0)
        {
            plannedText.text = "우클릭으로 재료를 추가하세요.";
            return;
        }

        var sb = new StringBuilder();
        foreach (var item in planned)
        {
            sb.AppendLine($"{item.Key} x{item.Value}");
        }

        plannedText.text = sb.ToString();
    }

    private void SetHint(string msg)
    {
        if (hintText) hintText.text = msg;
    }

    private void DoCraft()
    {
        if (planned.Count == 0)
        {
            SetHint("재료가 부족합니다.");
            return;
        }

        foreach (var item in planned)
        {
            if (inventory.GetCount(item.Key) < item.Value)
            {
                SetHint($"{item.Key} 가 부족합니다.");
                return;
            }
        }

        var matchedProduct = FindMatch(planned);

        if (matchedProduct == null)
        {
            SetHint("알맞은 레시피가 없습니다.");
            return;
        }

        foreach (var item in planned)
        {
            inventory.Consume(item.Key, item.Value);
        }

        foreach (var product in matchedProduct.outputs)
        {
            inventory.Add(product.type, product.count);
        }

        ClearPlanned();
        SetHint($"조합완료 : {matchedProduct.displayName}");
    }

    private CraftingRecipe FindMatch(Dictionary<ItemType, int> planned)
    {
        foreach (var recipe in recipeList)
        {
            if (recipe.craftingType != allowedCraftingType) continue;

            bool ok = true;
            foreach (var ing in recipe.inputs)
            {
                if (!planned.TryGetValue(ing.type, out int have) || have != ing.count)
                {
                    ok = false;
                    break;
                }
            }

            if (ok) return recipe;
        }

        return null;
    }
}
