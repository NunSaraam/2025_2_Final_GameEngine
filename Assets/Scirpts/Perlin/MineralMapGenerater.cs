using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineralMapGenerater : MonoBehaviour
{
    public GameObject stonePrefab;
    public GameObject coalPrefab;
    public GameObject ironPrefab;
    public GameObject diamondPrefab;

    public int width = 10;
    public int height = 5;
    public int depth = 10;

    void Start()
    {
        GenerateMineralIsland();
    }

    void GenerateMineralIsland()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    GameObject prefab = GetRandomMineralPrefab();
                    GameObject block = Instantiate(prefab, pos, Quaternion.identity, transform);

                    Block b = block.GetComponent<Block>() ?? block.AddComponent<Block>();
                    b.mineable = true;
                    b.maxHP = 3;
                    b.hp = b.maxHP;
                }
            }
        }
    }

    GameObject GetRandomMineralPrefab()
    {
        float rand = Random.value;

        if (rand < 0.05f) return diamondPrefab;       // 5%
        else if (rand < 0.15f) return ironPrefab;     // 10%
        else if (rand < 0.30f) return coalPrefab;     // 15%
        else return stonePrefab;                      // 70%
    }
}
