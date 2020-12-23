using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SVS.InventorySystem;
using System;

namespace Inventory { 

    public class InventorySystemData 
    {
        public Action updateHotbarCallback;
        private Storage storagePlayer, storageHotbar;
        List<int> inventoryUIElementIdList = new List<int>();
        List<int> hotbarUIElementIdList = new List<int>();

        public int selectedItemUIID = -1;

        public InventorySystemData(int playerStorageSize,int hotbarStorageSize)
        {
            storagePlayer = new Storage(playerStorageSize);
            storageHotbar = new Storage(hotbarStorageSize);
        }

        public int PlayerStorageLimit { get => storagePlayer.StorageLimit; }

        public void SetSelectedItemTo(int ui_id)
        {
            selectedItemUIID = ui_id;
        }

        public void ResetSelectedItem()
        {
            selectedItemUIID = -1;
        }

        public void AddHotbarUIElement(int ui_id)
        {
            hotbarUIElementIdList.Add(ui_id);
        }

        public void AddInventoryUIElement(int ui_id)
        {
            inventoryUIElementIdList.Add(ui_id);
        }

        public void ClearInventoryUIElementList()
        {
            inventoryUIElementIdList.Clear();
        }

        public int AddToStorage(IInventoryItem item)
        {
            int countLeft = item.Count;
            if (storageHotbar.CheckIfStorageContains(item.ID))
            {
                countLeft = storageHotbar.AddItem(item);
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            countLeft = storagePlayer.AddItem(item.ID,countLeft,item.IsStackable,item.StackLimit);
            if(countLeft > 0)
            {
                countLeft = storageHotbar.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
                updateHotbarCallback.Invoke();
                if (countLeft == 0)
                {
                    return countLeft;
                }
            }
            return countLeft;
        }

        public List<ItemData> GetItemsDataForHotbar()
        {
            return storageHotbar.GetItemsData();
        }

        public List<ItemData> GetItemsDataForInventory()
        {
            return storagePlayer.GetItemsData();
        }
        internal void SwapStorageItemsInsideInventory(int droppedItemID, int draggedItemID){
            var storage_IdDraggedItem = inventoryUIElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUIElementIdList.IndexOf(droppedItemID);

            if(CheckedItemForUiStorageNotEmpty(droppedItemID)){
                
                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else{
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }
        private bool CheckedItemForUiStorageNotEmpty(int ui_id){
            return inventoryUIElementIdList.Contains(ui_id) && storagePlayer.CheckIfItemIsEmpty
            (inventoryUIElementIdList.IndexOf(ui_id)) == false;
        }
        internal void SwapStorageItemsInsideHotbar(int droppedItemID, int draggedItemID){
            var storage_IdDraggedItem = hotbarUIElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUIElementIdList.IndexOf(droppedItemID);

            if(CheckedItemForHotbarStorageNotEmpty(droppedItemID)){
                
                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else{
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }
        private bool CheckedItemForHotbarStorageNotEmpty(int ui_id){
            return storageHotbar.CheckIfItemIsEmpty(hotbarUIElementIdList.IndexOf(ui_id)) == false;
        }
        internal void SwapStorageHotbarToInventory(int droppedItemID, int draggedItemID){
            var storage_IdDraggedItem = hotbarUIElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storageHotbar.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = inventoryUIElementIdList.IndexOf(droppedItemID);

            if(CheckedItemForUiStorageNotEmpty(droppedItemID)){
                
                var storagedata_IdDroppedItem = storagePlayer.GetItemData(storage_IdDroppedItem);

                storageHotbar.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else{
                storagePlayer.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storageHotbar.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }
        internal void SwapStorageInventoryToHotbar(int droppedItemID, int draggedItemID){
            var storage_IdDraggedItem = inventoryUIElementIdList.IndexOf(draggedItemID);
            var storagedata_IdDraggedItem = storagePlayer.GetItemData(storage_IdDraggedItem);
            var storage_IdDroppedItem = hotbarUIElementIdList.IndexOf(droppedItemID);

            if(CheckedItemForHotbarStorageNotEmpty(droppedItemID)){
                
                var storagedata_IdDroppedItem = storageHotbar.GetItemData(storage_IdDroppedItem);

                storagePlayer.SwapItemWithIndexFor(storage_IdDraggedItem, storagedata_IdDroppedItem);
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
            }
            else{
                storageHotbar.SwapItemWithIndexFor(storage_IdDroppedItem, storagedata_IdDraggedItem);
                storagePlayer.RemoveItemOfIndex(storage_IdDraggedItem);
            }
        }
    }
}
