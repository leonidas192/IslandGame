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
                if(countLeft == 0)
                {
                    updateHotbarCallback.Invoke();
                    return countLeft;
                }
            }
            countLeft = storagePlayer.AddItem(item.ID,countLeft,item.IsStackable,item.StackLimit);
            if(countLeft > 0)
            {
                countLeft = storageHotbar.AddItem(item.ID, countLeft, item.IsStackable, item.StackLimit);
                if (countLeft == 0)
                {
                    updateHotbarCallback.Invoke();
                    return countLeft;
                }
            }
            return countLeft;
        }

        public List<ItemData> GetItemsDataForInventory()
        {
            return storagePlayer.GetItemsData();
        }
    }
}
