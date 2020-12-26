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

        internal int TakeFromItem(string ID, int count)
        {
            int tempCount = 0;
            tempCount += TakeFromStorage(storageHotbar, ID, count);
            if (tempCount == count)
            {
                return count;
            }
            else
            {
                tempCount += TakeFromStorage(storagePlayer, ID, count - tempCount);
            }
            return tempCount;
        }

        private int TakeFromStorage(Storage storage, string ID, int count)
        {
            var tempQuantity = storage.GetItemCount(ID);
            if (tempQuantity > 0)
            {
                if (tempQuantity >= count)
                {
                    storage.TakeItemFromStorageIfContaintEnough(ID, count);
                    return count;
                }
                else
                {
                    storage.TakeItemFromStorageIfContaintEnough(ID, tempQuantity);
                    return tempQuantity;
                }
            }
            return 0;
        }

        internal bool CheckItemInStorage(string id, int count)
        {
            int quantity = storagePlayer.GetItemCount(id);
            quantity += storageHotbar.GetItemCount(id);
            if(quantity >= count)
            {
                return true;
            }
            return false;
        }

        internal bool CheckIfStorageIsFull()
        {
            return storageHotbar.CheckIfStorageIsFull() && storagePlayer.CheckIfStorageIsFull();
        }

        internal bool CheckIfSelectedItemIsEmpty(int ui_id)
        {
            if (CheckedItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.CheckIfItemIsEmpty(hotbarUIElementIdList.IndexOf(ui_id));
            }
            else if (CheckedItemForUiStorageNotEmpty(ui_id))
            {
                return storagePlayer.CheckIfItemIsEmpty(inventoryUIElementIdList.IndexOf(ui_id));
            }
            else
            {
                return true;
            }
        }

        internal void TakeOneFromItem(int ui_id)
        {
            if (CheckedItemForHotbarStorageNotEmpty(ui_id))
            {
                storageHotbar.TakeFromItemWith(hotbarUIElementIdList.IndexOf(ui_id),1);
            }
            else if (CheckedItemForUiStorageNotEmpty(ui_id))
            {
                storagePlayer.TakeFromItemWith(inventoryUIElementIdList.IndexOf(ui_id),1);
            }
            else
            {
                throw new Exception("No item with ui id " + ui_id);
            }
        }

        internal int GetItemCountFor(int ui_id)
        {
            if (CheckedItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetCountOfItemWithIndex(hotbarUIElementIdList.IndexOf(ui_id));
            }
            else if (CheckedItemForUiStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetCountOfItemWithIndex(inventoryUIElementIdList.IndexOf(ui_id));
            }
            else
            {
                return -1;
            }
        }

        internal void RemoveItemFromInventory(int ui_id)
        {
            if (hotbarUIElementIdList.Contains(ui_id))
            {
                storageHotbar.RemoveItemOfIndex(hotbarUIElementIdList.IndexOf(ui_id));
            }
            else if (inventoryUIElementIdList.Contains(ui_id))
            {
                storagePlayer.RemoveItemOfIndex(inventoryUIElementIdList.IndexOf(ui_id));
            }
            else
            {
                throw new Exception("No item with id " + ui_id);
            }
            ResetSelectedItem();
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
            return hotbarUIElementIdList.Contains(ui_id) && storageHotbar.CheckIfItemIsEmpty(hotbarUIElementIdList.IndexOf(ui_id)) == false;
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

        internal string GetItemIdFor(int ui_id)
        {
            if (CheckedItemForUiStorageNotEmpty(ui_id))
            {
                return storagePlayer.GetIdOfItemWithIndex(inventoryUIElementIdList.IndexOf(ui_id));
            }
            else if (CheckedItemForHotbarStorageNotEmpty(ui_id))
            {
                return storageHotbar.GetIdOfItemWithIndex(hotbarUIElementIdList.IndexOf(ui_id));
            }
            else
            {
                return null;
            }
        }

        public SavedItemSystemData GetDataToSave(){
            return new SavedItemSystemData{
                playerStorageItems = storagePlayer.GetDataToSave(),
                hotbarStorageItems = storageHotbar.GetDataToSave(),
                playerStorageSize = storagePlayer.StorageLimit
            };
        }

        public void LoadData(SavedItemSystemData dataToLoad){
            storagePlayer = new Storage (dataToLoad.playerStorageSize);
            storageHotbar.ClearStorage();
            foreach (var item in dataToLoad.playerStorageItems)
            {
                if(item.IsNull==false){
                    storagePlayer.SwapItemWithIndexFor(item.StorageIndex,item);
                }
            }
            foreach (var item in dataToLoad.hotbarStorageItems)
            {
                if(item.IsNull==false){
                    storageHotbar.SwapItemWithIndexFor(item.StorageIndex,item);
                }
            }
            updateHotbarCallback.Invoke();
        }
    }
    [Serializable]
    public struct InventorySaveData{
        public List <ItemData> playerStorageItems, gotbarStorageItems;
        public int playerStorageSize;
    }
    [Serializable]
    public struct SavedItemSystemData{
        public List<ItemData> playerStorageItems;
        public List<ItemData> hotbarStorageItems;
        public int playerStorageSize;
    }
}
