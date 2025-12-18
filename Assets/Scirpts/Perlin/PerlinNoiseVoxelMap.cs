using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IslandType
{
    Main,
    Resource,
    Mineral,
}

public class PerlinNoiseVoxelMap : MonoBehaviour
{
    public class TileInfos
    {
        public int tileX;
        public int tileY;
        public int tileZ;
        public string tileType;
    }

    public IslandType islandType = IslandType.Resource;
    [Header("자원")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    //나무
    public GameObject woodPrefab;
    public GameObject leafPrefab;
    [Range(0f, 1f)] public float treeSpawnChance = 0.05f; // 나무 밀도

    [Header("광석")]
    public GameObject stonePrefab;
    public GameObject coalPrefab;
    public GameObject ironPrefab;
    public GameObject diamondPrefab;


    [Header("세팅")]
    public int width = 20;

    public int depth = 20;

    public int maxHeight = 16;

    private bool[,,] hasBlock;

    [SerializeField] float noiseScale = 20f;

    List<TileInfos> tileInfos = new List<TileInfos>();

    private void Start()
    {
        GenerateIsland();
    }
    public void GenerateIsland()
    {
        switch (islandType)
        {
            case IslandType.Resource:
                GenerateResourceIsland();
                break;
            case IslandType.Mineral:
                GenerateMineralIsland();
                break;
        }
    }

    void GenerateResourceIsland()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        hasBlock = new bool[width, maxHeight, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.Max(1, Mathf.FloorToInt(noise * maxHeight));

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        PlaceGrass(x, y, z);
                        TrySpawnTree(x, y + 1, z);
                    }
                    else
                    {
                        Place(x, y, z);
                    }
                }
            }
        }
    }

    void GenerateMineralIsland()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        hasBlock = new bool[width, maxHeight, depth];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;
                float noise = Mathf.PerlinNoise(nx, nz);
                int h = Mathf.Max(1, Mathf.FloorToInt(noise * maxHeight));

                for (int y = 0; y <= h; y++)
                {
                    // 깊이에 따른 보정값 (0 ~ 1)
                    float depth01 = (float)y / h;

                    // 기본은 돌
                    ItemType spawnType = ItemType.Stone;

                    // 깊을수록 광석 확률 증가
                    float oreChance = Mathf.Lerp(0.15f, 0.6f, depth01);
                    float rand = Random.value;

                    if (rand < oreChance)
                    {
                        float oreRand = Random.value;

                        if (oreRand < 0.03f * depth01)
                            spawnType = ItemType.Diamond;
                        else if (oreRand < 0.10f)
                            spawnType = ItemType.Iron;
                        else if (oreRand < 0.37f)
                            spawnType = ItemType.Coal;
                        else
                            spawnType = ItemType.Stone;
                    }

                    PlaceMineral(x, y, z, spawnType);
                }
            }
        }
    }

    void PlaceMineral(int x, int y, int z, ItemType type)
    {
        GameObject prefab = type switch
        {
            ItemType.Stone => stonePrefab,
            ItemType.Coal => coalPrefab,
            ItemType.Iron => ironPrefab,
            ItemType.Diamond => diamondPrefab,
            _ => null
        };

        if (prefab == null) return;

        var go = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"{type}_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos { tileX = x, tileY = y, tileZ = z, tileType = type.ToString() });

        var block = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        block.type = type;

        // 기본 설정
        block.maxHP = type switch
        {
            ItemType.Stone => 3,
            ItemType.Coal => 4,
            ItemType.Iron => 5,
            ItemType.Diamond => 6,
            _ => 3
        };

        block.dropCount = 1;
        block.mineable = true;
    }


    void Place(int x, int y, int z)
    {
        var go = Instantiate(dirtPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos()
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Dirt"
        });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Dirt;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    void PlaceGrass(int x, int y, int z)
    {
        var go = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";
        hasBlock[x, y, z] = true;

        tileInfos.Add(new TileInfos()
        {
            tileX = x,
            tileY = y,
            tileZ = z,
            tileType = "Grass"
        });

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Grass;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;

        TrySpawnTree(x, y + 1, z);
    }

    void TrySpawnTree(int x, int y, int z)
    {
        if (islandType != IslandType.Resource) return;
        if (Random.value > treeSpawnChance) return;

        int treeHeight = Random.Range(3, 6); //줄기 높이

        //나무 줄기 생성
        for (int i = 0; i < treeHeight; i++)
        {
            Vector3Int pos = new Vector3Int(x, y + i, z);
            PlaceTreeBlock(woodPrefab, pos, ItemType.Wood, 5);
        }

        //나뭇잎 생성 (단순한 + 모양)
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dz = -2; dz <= 2; dz++)
            {
                for (int dy = 0; dy <= 2; dy++)
                {
                    if (Mathf.Abs(dx) + Mathf.Abs(dz) + dy <= 3) //둥근 느낌 유지
                    {
                        Vector3Int leafPos = new Vector3Int(x + dx, y + treeHeight + dy - 1, z + dz);
                        PlaceTreeBlock(leafPrefab, leafPos, ItemType.Leaf, 1, leaf: true);
                    }
                }
            }
        }
    }

    void PlaceTreeBlock(GameObject prefab, Vector3Int pos, ItemType type, int hp, bool leaf = false)
    {
        if (prefab == null || !IsInBounds(pos)) return;

        var go = Instantiate(prefab, pos, Quaternion.identity, transform);
        go.name = $"{type}_{pos.x}_{pos.y}_{pos.z}";

        var block = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        block.type = type;
        block.maxHP = hp;
        block.dropCount = 1;
        block.mineable = true;

        if (leaf)
        {
            // 항상 Leaf를 기본으로 드롭
            block.type = ItemType.Leaf;
            block.dropCount = 1;

            // 그리고 20% 확률로 사과도 하나 더 드롭
            if (Random.value < 0.2f)
            {
                // 인벤토리에 사과도 추가되도록 별도 드롭 로직 작성
                var appleDrop = new GameObject("AppleDrop");
                var dropComp = appleDrop.AddComponent<DroppedItem>();
                dropComp.itemType = ItemType.apple;
                dropComp.amount = 1;
                appleDrop.transform.position = pos + Vector3.up * 0.5f;
            }
        }

        bool IsInBounds(Vector3Int pos)
        {
            return pos.x >= 0 && pos.x < width &&
                   pos.y >= 0 && pos.y < maxHeight &&
                   pos.z >= 0 && pos.z < depth;
        }
    }
}