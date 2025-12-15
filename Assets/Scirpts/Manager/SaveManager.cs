using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<SavedBlockData> blocks = new();
    public Vector3 playerPosition;
    public Dictionary<ItemType, int> inventory = new();
}

[System.Serializable]
public class SavedBlockData
{
    public Vector3 position;
    public ItemType type;
    public Quaternion rotation;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    string savePath => Application.persistentDataPath + "/save.json";

    public void Save()
    {
        var data = new SaveData();

        // 저장 대상 수집
        foreach (var block in FindObjectsOfType<Block>())
        {
            data.blocks.Add(new SavedBlockData
            {
                position = block.transform.position,
                rotation = block.transform.rotation,
                type = block.type
            });
        }

        data.playerPosition = GameObject.FindWithTag("Player").transform.position;

        var inven = FindObjectOfType<Inventory>();
        data.inventory = new Dictionary<ItemType, int>(inven.items);

        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
        Debug.Log("Game Saved!");
    }

    public void Load()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<SaveData>(json);

        // 기존 블록 제거
        foreach (var b in FindObjectsOfType<Block>())
            Destroy(b.gameObject);

        // 재배치
        foreach (var blockData in data.blocks)
        {
            // Instantiate block prefab by type
        }

        GameObject.FindWithTag("Player").transform.position = data.playerPosition;

        var inven = FindObjectOfType<Inventory>();
        inven.items = new Dictionary<ItemType, int>(data.inventory);

        FindObjectOfType<InventoryUI>().UpdateInventory(inven);
    }
}
