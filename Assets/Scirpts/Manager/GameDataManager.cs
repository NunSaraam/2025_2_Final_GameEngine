using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    public Dictionary<ItemType, int> inventoryData = new();
    public PlayerStatusData playerStatusData = new();

    void Awake()
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
}

[System.Serializable]
public class PlayerStatusData
{
    public float hunger = 100f;
    public float thirst = 100f;
    public float health = 100f;
    public float stamina = 100f;
}
