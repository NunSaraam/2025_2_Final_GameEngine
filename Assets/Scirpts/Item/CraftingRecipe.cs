using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraftingType
{
    PlayerOnly,
    WorkbenchOnly
}

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Serializable]
    public struct Ingredient
    {
        public ItemType type;
        public int count;
    }

    [Serializable]
    public struct Product
    {
        public ItemType type;
        public int count;
    }

    public string displayName;
    public CraftingType craftingType; //조합 방식
    public List<Ingredient> inputs = new();
    public List<Product> outputs = new();
}
