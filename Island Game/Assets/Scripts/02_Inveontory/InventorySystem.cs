using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using System;
using SVS.InventorySystem;
using UnityEngine.EventSystems;

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
        inventoryData.updateHotbarCallback += UpdateHotbarHandler;
        ItemData artificialItem = new ItemData(0,20, "8bb87095fbb5408c9ba6d33e9a4e4b3e", true,100);
        ItemData artificialItem1 = new ItemData(0,90, "8bb87095fbb5408c9ba6d33e9a4e4b3e", true,100);
        AddToStorage(artificialItem);
        AddToStorage(artificialItem1);
        var hotbarUiElementsList = uiInventory.GetUiElementsForHotbar();

        for (int i = 0; i < hotbarUiElementsList.Count; i++)
        {
            inventoryData.AddHotbarUIElement(hotbarUiElementsList[i].GetInstanceID());
            hotbarUiElementsList[i].OnClickEvent += UseHotbarItemHandler;
            hotbarUiElementsList[i].DragContinueCallback +=DraggingHandler;
            hotbarUiElementsList[i].DragStartCallback += DragStartHandler;
            hotbarUiElementsList[i].DragStopCallback += DragStopHandler;
            hotbarUiElementsList[i].DropCallback += DropHandler;
        }
    }
    private void UpdateHotbarHandler(){
        Debug.Log("updating hotbar");
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
            uiItemElement.DragContinueCallback += DraggingHandler;
            uiItemElement.DragStartCallback += DragStartHandler;
            uiItemElement.DragStopCallback += DragStopHandler;
            uiItemElement.DropCallback += DropHandler;
        }
    }
    private void DropHandler(PointerEventData eventData,int droppedItemID){
        if(uiInventory.Draggableitem != null){
            var draggedItemID = uiInventory.DraggableItemPanel.GetInstanceID();
            if(draggedItemID == droppedItemID)
            {
                return;
            }
            DeselectCurrentItem();
            if(uiInventory.CheckItemInInventory(draggedItemID)){
                if(uiInventory.CheckItemInInventory(droppedItemID)){
                    DroppingItemsInventoryToInventory(droppedItemID,draggedItemID);
                }
                else
                {
                    DroppingItemsInventoryToHotbar(droppedItemID,draggedItemID);
                }
            }
            else
            {
                if(uiInventory.CheckItemInInventory(droppedItemID)){
                    DroppingItemsHotbarToInventory(droppedItemID,draggedItemID);
                }
                else
                {
                    DroppingItemsHotbarToHotbar(droppedItemID,draggedItemID);
                }
                
            }
        }
    }
    private void DroppingItemsHotbarToHotbar(int droppedItemID,int draggedItemID){
        uiInventory.SwapUiItemHotbarToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideHotbar(droppedItemID, draggedItemID);
    }
    private void DroppingItemsHotbarToInventory(int droppedItemID,int draggedItemID){
        uiInventory.SwapUiItemHotbarToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageHotbarToInventory(droppedItemID, draggedItemID);
    }
    private void  DroppingItemsInventoryToHotbar(int droppedItemID,int draggedItemID){
       uiInventory.SwapUiItemInventoryToHotbar(droppedItemID, draggedItemID);
        inventoryData.SwapStorageInventoryToHotbar(droppedItemID, draggedItemID);
    }
    private void DroppingItemsInventoryToInventory(int droppedItemID,int draggedItemID){
        uiInventory.SwapUiItemInventoryToInventory(droppedItemID, draggedItemID);
        inventoryData.SwapStorageItemsInsideInventory(droppedItemID, draggedItemID);
    }
    private void DeselectCurrentItem(){
        inventoryData.ResetSelectedItem();
    }
    private void DragStopHandler(PointerEventData eventData){
        uiInventory.DestroyDraggedObject();
    }
    private void DragStartHandler(PointerEventData eventData,int ui_id){
        uiInventory.DestroyDraggedObject();
        uiInventory.CreateDraggableItem(ui_id);
    }
    private void DraggingHandler(PointerEventData eventData){
        uiInventory.MoveDraggableItem(eventData);
    }
    

    private void UiElementSelectedHandler(int ui_id, bool isEmpty)
    {
        Debug.Log("Selecting inventory items");
        if (isEmpty)
            return;
        DeselectCurrentItem();
        inventoryData.SetSelectedItemTo(ui_id);
        
    }

    public int AddToStorage(IInventoryItem item)
    {
        int val = inventoryData.AddToStorage(item);
        return val;
    }
}
