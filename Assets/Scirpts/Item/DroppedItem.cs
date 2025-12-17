using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public ItemType itemType;
    public int amount = 1;
    public float lifeTime = 300f;

    public MeshRenderer meshRenderer;

    void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponentInChildren<MeshRenderer>();
        }

        Destroy(gameObject, lifeTime);
        UpdateVisual();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.Add(itemType, amount);
                Destroy(gameObject);
            }
        }
    }
    public void Setup(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }


    void UpdateVisual()
    {
        var dataList = FindObjectOfType<Inventory>()?.itemDataList;
        if (dataList == null) return;

        var data = dataList.Find(x => x.itemType == itemType);
        if (data != null && data.icon != null)
        {
            Texture2D iconTex = data.icon.texture;
            Material mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            mat.mainTexture = iconTex;

            if (meshRenderer != null)
            {
                meshRenderer.material = mat;
            }
        }
    }
}
