using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public IslandType targetIsland;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boat"))
        {
            Boat boat = other.GetComponent<Boat>();

            if (boat != null)
            {
                PlayerStats player = boat.PlayerStats;
                Inventory inventory = boat.PlayerInventory;

                SaveGameData(player, inventory);
                IslandTravelManager.Instance.TravelToIsland(targetIsland);
            }
        }
    }

    private void SaveGameData(PlayerStats player, Inventory inventory)
    {
        if (player != null)
        {
            GameDataManager.Instance.playerStatusData = player.ExportStatus();
            Debug.Log("[Save] PlayerStats saved!");
        }
        else
        {
            Debug.LogWarning("[Save] PlayerStats NOT found!");
        }

        if (inventory != null)
        {
            GameDataManager.Instance.inventoryData = new Dictionary<ItemType, int>(inventory.items);
            Debug.Log("[Save] Inventory saved!");
        }
        else
        {
            Debug.LogWarning("[Save] Inventory NOT found!");
        }
    }
}
