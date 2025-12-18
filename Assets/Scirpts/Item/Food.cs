using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFood", menuName = "Item/Food")]
public class Food : ItemTypeData
{
    public int hungerRestore; // 배고픔 회복량
    public int thirstRestore; // 목마름 회복량

    public void Use(PlayerStats stat, Inventory inventory)
    {
        stat.AddHunger(hungerRestore);
        stat.AddThirst(thirstRestore);

        inventory.Consume(itemType, 1); // 인벤토리에서 1개 제거
    }
}
