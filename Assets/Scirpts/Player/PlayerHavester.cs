using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHavester : MonoBehaviour
{
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;                  //가능 한 레이어 전부 다(일단)
    public int toolDamage = 1;
    public float hitCooldown = .15f;

    public GameObject selectedBlock;

    private float _nextHitTime;
    private Camera _cam;
    [SerializeField] private Inventory inventory;
    InventoryUI inventoryUI;

    private void Awake()
    {
        _cam = Camera.main;

        if (inventory == null) inventory = gameObject.AddComponent<Inventory>();

        inventoryUI = FindObjectOfType<InventoryUI>();
    }

    private void Update()
    {
        if (inventoryUI.selectedIndex < 0)
        {
            HaversterMode(toolDamage);
        }
        else
        {
            switch (inventoryUI.GetInventorySlot())
            {
                case ItemType.Sword:
                    HaversterMode(10);
                    break;
                default:
                    BuildMode();
                    break;
            }



        }
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }

    public void BuildMode()
    {

        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));

            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                Vector3Int placePos = AdjacentCellOnHitFace(hit);

                ItemType selected = inventoryUI.GetInventorySlot();
                if (inventory.Consume(selected, 1))
                {
                    FindObjectOfType<MainIslandGenerater>().MainPlaceTile(placePos, selected);
                }
            }
        }

        Ray rayDebug = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        if (Physics.Raycast(rayDebug, out var hitDebug, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
        {
            Vector3Int placePos = AdjacentCellOnHitFace(hitDebug);
            selectedBlock.transform.localScale = Vector3.one;
            selectedBlock.transform.position = placePos;
            selectedBlock.transform.rotation = Quaternion.identity;
        }
        else
        {
            selectedBlock.transform.localScale = Vector3.zero;
        }
    }

    public void HaversterMode(int damage)
    {
        selectedBlock.transform.localScale = Vector3.zero;
        if (Input.GetMouseButtonDown(0) && Time.time >= _nextHitTime)
        {
            _nextHitTime = Time.time + hitCooldown;

            Ray ray = _cam.ViewportPointToRay(new Vector3(.5f, .5f, 0));

            if (Physics.Raycast(ray, out var hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore))
            {
                var block = hit.collider.GetComponent<Block>();

                if (block != null)
                {
                    block.Hit(damage, inventory);
                }
            }
        }
    }
}
