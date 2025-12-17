using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Sprite itemicon;
    public int maxStack = 64;

    public ItemType type = ItemType.Dirt;
    public ItemTypeData itemData;

    public int maxHP = 3;

    [HideInInspector] public int hp;

    public int dropCount = 1;
    public bool mineable = true;

    [SerializeField] private GameObject dropPrefab;

    private void Awake()
    {
        hp = maxHP;
        if (GetComponent<Collider>() == null) gameObject.AddComponent<BoxCollider>();

        if (string.IsNullOrEmpty(gameObject.tag) || gameObject.tag == "Untaged")
        {
            gameObject.tag = "Block";
        }
    }

    public void Hit(int damage, Inventory inven)
    {
        if (!mineable) return;

        hp -= damage;

        if (hp <= 0)
        {
            DropItem(); // 인벤토리에 바로 안넣고, 드롭 
            Destroy(gameObject);

            //SaveManager.Instance.SaveMainIsland();

            //if (!SaveManager.Instance.IsLoading)
            //{
            //    SaveManager.Instance.SaveMainIsland();
            //}
        }
    }


    void DropItem()
    {
        if (dropPrefab == null) return;

        GameObject drop = Instantiate(dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);

        // 아이템 정보 설정
        DroppedItem di = drop.GetComponent<DroppedItem>();
        if (di != null)
        {
            di.itemType = type;
            di.amount = dropCount;
        }

        // 메시와 머티리얼 복사
        MeshFilter sourceFilter = GetComponentInChildren<MeshFilter>();
        MeshRenderer sourceRenderer = GetComponentInChildren<MeshRenderer>();

        MeshFilter dropFilter = drop.GetComponent<MeshFilter>();
        MeshRenderer dropRenderer = drop.GetComponent<MeshRenderer>();

        if (sourceFilter != null && dropFilter != null)
        {
            dropFilter.sharedMesh = sourceFilter.sharedMesh;
        }

        if (sourceRenderer != null && dropRenderer != null)
        {
            dropRenderer.sharedMaterials = sourceRenderer.sharedMaterials;
        }

        if (drop.GetComponent<Rigidbody>() == null)
        {
            drop.AddComponent<Rigidbody>().mass = 0.5f;
        }
    }
}
