using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private UIInventory uiInventory;

    private void Awake()
    {
        uiInventory = GetComponent<UIInventory>();       
    }

    public void ToggleInventory()
    {
        if(uiInventory.IsInventoryVisible == false)
        {
            //populate inventory

        }
        uiInventory.ToggleUI();
    }
}
