using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MainIslandGenerater : MonoBehaviour
{
    public GameObject dirtPrefab;
    public GameObject grassPrefab;

    public int width = 5;
    public int height = 3;
    public int depth = 5;

    void Start()
    {
        string path = Application.persistentDataPath + "/mainIsland.json";
        Debug.Log("파일 존재 체크: " + path);

        if (File.Exists(path))
        {
            SaveManager.Instance.LoadMainIsland();
            Debug.Log("데이터 로드 성공");
        }
        else
        {
            GenerateStartingIsland();
            SaveManager.Instance.SaveMainIsland();
            Debug.Log("데이터 없음 → 새로 생성 & 저장 시도");
        }
    }

    void GenerateStartingIsland()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject block;

                    if (y == height - 1)
                        block = Instantiate(grassPrefab, pos, Quaternion.identity, transform);
                    else
                        block = Instantiate(dirtPrefab, pos, Quaternion.identity, transform);

                    var b = block.GetComponent<Block>() ?? block.AddComponent<Block>();
                    b.type = y == height - 1 ? ItemType.Grass : ItemType.Dirt;
                    b.maxHP = 3;
                    b.dropCount = 1;
                    b.mineable = true;
                }
            }
        }
    }

    public void MainPlaceTile(Vector3Int pos, ItemType type)
    {
        GameObject prefab = type switch
        {
            ItemType.Dirt => dirtPrefab,
            ItemType.Grass => grassPrefab,
            _ => null
        };

        if (prefab == null) return;

        var block = Instantiate(prefab, pos, Quaternion.identity, transform);
        var b = block.GetComponent<Block>() ?? block.AddComponent<Block>();
        b.type = type;

        //if (!SaveManager.Instance.IsLoading)
        //{
        //    SaveManager.Instance.SaveMainIsland();
        //}
    }
}
