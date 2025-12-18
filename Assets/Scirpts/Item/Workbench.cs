using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    public CraftingPanel craftingPanel;

    private void Start()
    {
        if (craftingPanel == null)
        {
            craftingPanel = FindObjectOfType<CraftingPanel>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryOpenWorkbench();
        }
    }

    void TryOpenWorkbench()
    {
        Camera cam = Camera.main;
        if (cam == null || craftingPanel == null) return;

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                craftingPanel.TogglePanel(CraftingType.WorkbenchOnly);
            }
        }
    }
}
