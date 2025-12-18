using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class MainIslandSaveData
{
    public List<SavedBlockData> blocks = new();
}

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public Dictionary<ItemType, int> inventory = new();
}

[System.Serializable]
public class SavedBlockData
{
    public Vector3 position;
    public Quaternion rotation;
    public string blockType;  // 문자열로 저장 (ex. "Dirt", "Grass")
}



[System.Serializable]
public class ItemTypePrefabPair
{
    public ItemType type;
    public GameObject prefab;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    public List<ItemTypePrefabPair> blockPrefabs;
    private Dictionary<ItemType, GameObject> prefabMap;

    public bool IsLoading { get; private set; }

    string mainIslandPath => Path.Combine(Application.persistentDataPath, "mainIsland.json");
    string savePath => Path.Combine(Application.persistentDataPath, "save.json");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitPrefabMap();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitPrefabMap()
    {
        prefabMap = new Dictionary<ItemType, GameObject>();
        foreach (var pair in blockPrefabs)
        {
            if (!prefabMap.ContainsKey(pair.type))
                prefabMap[pair.type] = pair.prefab;
        }
    }

    public GameObject GetBlockPrefab(ItemType type)
    {
        prefabMap.TryGetValue(type, out var prefab);
        return prefab;
    }

    public void SaveMainIsland()
    {
        var data = new MainIslandSaveData();
        foreach (var block in FindObjectsOfType<Block>())
        {
            data.blocks.Add(new SavedBlockData
            {
                position = block.transform.position,
                rotation = block.transform.rotation,
                blockType = block.type.ToString()
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(mainIslandPath, json);
        string path = Application.persistentDataPath + "/mainIsland.json";
        Debug.Log("Saving MainIsland to: " + path);
        Debug.Log("Main Island Saved");
    }

    public void LoadMainIsland()
    {
        if (!File.Exists(mainIslandPath)) return;

        IsLoading = true;

        if (!File.Exists(mainIslandPath))
        {
            Debug.Log("No Main Island save file found.");
            return;
        }

        string json = File.ReadAllText(mainIslandPath);
        var data = JsonUtility.FromJson<MainIslandSaveData>(json);

        foreach (var b in FindObjectsOfType<Block>())
            Destroy(b.gameObject);

        foreach (var blockData in data.blocks)
        {
            if (!System.Enum.TryParse(blockData.blockType, out ItemType type)) continue;

            GameObject prefab = GetBlockPrefab(type);
            if (prefab == null) continue;

            var block = Instantiate(prefab, blockData.position, blockData.rotation);
            block.GetComponent<Block>().type = type;
        }

        IsLoading = false;
        Debug.Log("Main Island Loaded");
    }

    public bool HasMainIslandSave() => File.Exists(mainIslandPath);

    public void Save()
    {
        var data = new SaveData();

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            data.playerPosition = player.transform.position;

        var inven = FindObjectOfType<Inventory>();
        if (inven != null)
            data.inventory = new Dictionary<ItemType, int>(inven.items);

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved");
    }

    public void Load()
    {
        if (!File.Exists(mainIslandPath)) return;

        IsLoading = true;

        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        var data = JsonUtility.FromJson<SaveData>(json);

        var player = GameObject.FindWithTag("Player");
        if (player != null)
            player.transform.position = data.playerPosition;

        var inven = FindObjectOfType<Inventory>();
        if (inven != null)
        {
            inven.items = new Dictionary<ItemType, int>(data.inventory);
            FindObjectOfType<InventoryUI>()?.UpdateInventory(inven);
        }

        IsLoading = false;
        Debug.Log("Game Loaded");
    }

    public bool HasSaveData() => File.Exists(savePath);
}