using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;

public class InventorySystem : MonoBehaviour
{
    private UIInventory uiInventory;

    private InventorySystemData inventoryData;

    public int playerStorageSize = 20;


    private void Awake()
    {
        uiInventory = GetComponent<UIInventory>();       
    }

    private void Start()
    {
        inventoryData = new InventorySystemData(playerStorageSize, uiInventory.HotbarElementsCount);
        ItemData artificialItem = new ItemData(0,10, "8bb87095fbb5408c9ba6d33e9a4e4b3e", true,100);
        AddToStorage(artificialItem);
        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();

        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUIElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
        }
    }

    private void UseHotbarItemHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Using Hotbar ");
        if (isEmpty)
            return;
        //throw new NotImplementedException();
        
    }

    public void ToggleInventory()
    {
        if(uiInventory.IsInventoryVisible == false)
        {
            inventoryData.ResetSelectedItem();
            inventoryData.ClearInventoryUIElementList();
            PrepareUI();
            PutDataInUI();
        }
        uiInventory.ToggleUI();
    }

    private void PutDataInUI()
    {
        var uiElementList = uiInventory.GetUiElementsForInventory();
        var inventoryItemList = inventoryData.GetItemsDataForInventory();
        for (int i = 0; i < uiElementList.Count; i++)
        {
            var uiItemElement = uiElementList[i];
            var itemData = inventoryItemList[i];
            if(itemData.IsNull == false)
            {
                var itemName = ItemDataManager.instance.GetItemName(itemData.ID);
                var itemSprite = ItemDataManager.instance.GetItemSprite(itemData.ID);
                uiItemElement.SetInventoryUIElement(itemName, itemData.Count, itemSprite);
            }
            inventoryData.AddInventoryUIElement(uiItemElement.GetInstanceID());
        }      
    }

    private void PrepareUI()
    {
        uiInventory.PrepareInventoryItems(inventoryData.PlayerStorageLimit);
        AddEventHandlersToInventoryUiElement();
    }

    private void AddEventHandlersToInventoryUiElement()
    {
        foreach (var uiItemElement in uiInventory.GetUiElementsForInventory())
        {
            uiItemElement.OnClickEvent += UiElementSelectedHandler;
        }
    }

    private void UiElementSelectedHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Selecting inventory items");
        if (isEmpty)
            return;
        inventoryData.ResetSelectedItem();
        inventoryData.SetSelectedItemTo(ui_id);
        
    }

    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }
}
