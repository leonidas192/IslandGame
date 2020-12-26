using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIInventory : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;

    public UIStorageButtonsHelper uiStorageButtonsHelper;

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get => hotbarUIItems.Count; }
    public RectTransform Draggableitem { get => draggableitem; }
    public InventoryItemPanelHelper DraggableItemPanel { get => draggableItemPanel; }

    public Dictionary<int, InventoryItemPanelHelper> inventoryUIItems = new Dictionary<int, InventoryItemPanelHelper>();
    public Dictionary<int, InventoryItemPanelHelper> hotbarUIItems = new Dictionary<int, InventoryItemPanelHelper>();

    private List<int> listOfHotbarElementID = new List<int>();

    public List<InventoryItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUIItems.Values.ToList();
    }

    public GameObject hotbarPanel, storagePanel;

    public GameObject storagePrefab;

    private RectTransform draggableitem;
    private InventoryItemPanelHelper draggableItemPanel;

    public Canvas canvas;

    private void Awake()
    {
        inventoryGeneralPanel.SetActive(false);
        foreach (Transform child in hotbarPanel.transform)
        {
            InventoryItemPanelHelper helper = child.GetComponent<InventoryItemPanelHelper>();
            if (helper != null)
            {
                hotbarUIItems.Add(helper.GetInstanceID(), helper);
                helper.isHotbarItem = true;
            }
        }
        listOfHotbarElementID = hotbarUIItems.Keys.ToList();
    }

    public void ToggleUI()
    {
        if (inventoryGeneralPanel.activeSelf == false)
        {
            inventoryGeneralPanel.SetActive(true);
        }
        else
        {
            inventoryGeneralPanel.SetActive(false);
            DestroyDraggedObject();
        }
        uiStorageButtonsHelper.HideAllButtons();
    }

    internal int GetHotbarElementUiIDWithIndex(int ui_index)
    {
        if(listOfHotbarElementID.Count  <= ui_index)
        {
            return -1;
        }
        return listOfHotbarElementID[ui_index];
    }

    internal void ClearItemElement(int ui_id)
    {
        GetItemFromCorrectDictionary(ui_id).ClearImage();
    }

    private InventoryItemPanelHelper GetItemFromCorrectDictionary(int ui_id)
    {
        if (inventoryUIItems.ContainsKey(ui_id))
        {
            return inventoryUIItems[ui_id];
        }
        else if (hotbarUIItems.ContainsKey(ui_id))
        {
            return hotbarUIItems[ui_id];
        }
        return null;
    }

    public void AssignUseButtonHandler(Action handler)
    {
        uiStorageButtonsHelper.OnUseBtnClick += handler;
    }

    internal void updateItemInfo(int ui_id, int count)
    {
        GetItemFromCorrectDictionary(ui_id).UpdateCount(count);
    }

    public void AssignDropButtonHandler(Action handler)
    {
        uiStorageButtonsHelper.OnDropBtnClick += handler;
    }

    public void ToggleItemButtons(bool useBtn, bool dropButton)
    {
        uiStorageButtonsHelper.ToggleGroupButton(dropButton);
        uiStorageButtonsHelper.ToggleUseButton(useBtn);
    }

    internal void PrepareInventoryItems(int playerStorageLimit)
    {
        for (int i = 0; i < playerStorageLimit; i++)
        {
            foreach (Transform child in storagePanel.transform)
            {
                Destroy(child.gameObject);
            }
        }
        inventoryUIItems.Clear();
        for (int i = 0; i < playerStorageLimit; i++)
        {
            var element = Instantiate(storagePrefab, Vector3.zero, Quaternion.identity, storagePanel.transform);
            var itemHelper = element.GetComponent<InventoryItemPanelHelper>();
            inventoryUIItems.Add(itemHelper.GetInstanceID(), itemHelper);
        }
    }

    public List<InventoryItemPanelHelper> GetUiElementsForInventory()
    {
        return inventoryUIItems.Values.ToList();
    }
    public void DestroyDraggedObject(){
        if(draggableitem != null){
            Destroy(draggableitem.gameObject);
            draggableItemPanel = null;
            draggableitem = null;
        }
    }
    public void CreateDraggableItem(int ui_id){
        if(CheckItemInInventory(ui_id)){
            draggableItemPanel = inventoryUIItems[ui_id];
        }
        else
        {
            draggableItemPanel = hotbarUIItems[ui_id];
        }

        Image itemImage = draggableItemPanel.itemImage;
        var imageObject = Instantiate(itemImage,itemImage.transform.position,
        Quaternion.identity,canvas.transform);
        imageObject.raycastTarget = false;
        imageObject.sprite = itemImage.sprite;

        draggableitem = imageObject.GetComponent<RectTransform>();
        draggableitem.sizeDelta = new Vector2(100,100);
    }
    public bool CheckItemInInventory(int ui_id){
        return inventoryUIItems.ContainsKey(ui_id);
    }
    internal void MoveDraggableItem(PointerEventData eventData){
        var valueToAdd = eventData.delta / canvas.scaleFactor;
        draggableitem.anchoredPosition += valueToAdd;
    }
    internal void SwapUiItemInventoryToInventory(int droppedItemID, int draggedItemID){
        var tempName = inventoryUIItems[draggedItemID].itemName;
        var tempCount = inventoryUIItems[draggedItemID].itemCount;
        var tempSprite = inventoryUIItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUIItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUIItems[droppedItemID];
        inventoryUIItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount,
        droppedItemData.itemImage.sprite, droppedItemData.isEmpty);
    
        inventoryUIItems[droppedItemID].SwapWithData(tempName,tempCount,tempSprite,tempisEmpty);
        DestroyDraggedObject();
    }
    internal void SwapUiItemHotbarToHotbar(int droppedItemID, int draggedItemID){
        var tempName = hotbarUIItems[draggedItemID].itemName;
        var tempCount = hotbarUIItems[draggedItemID].itemCount;
        var tempSprite = hotbarUIItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUIItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUIItems[droppedItemID];
        hotbarUIItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount,
        droppedItemData.itemImage.sprite, droppedItemData.isEmpty);
    
        hotbarUIItems[droppedItemID].SwapWithData(tempName,tempCount,tempSprite,tempisEmpty);
        DestroyDraggedObject();
    }
    internal void SwapUiItemHotbarToInventory(int droppedItemID, int draggedItemID){
        var tempName = hotbarUIItems[draggedItemID].itemName;
        var tempCount = hotbarUIItems[draggedItemID].itemCount;
        var tempSprite = hotbarUIItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = hotbarUIItems[draggedItemID].isEmpty;

        var droppedItemData = inventoryUIItems[droppedItemID];
        hotbarUIItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount,
        droppedItemData.itemImage.sprite, droppedItemData.isEmpty);
    
        inventoryUIItems[droppedItemID].SwapWithData(tempName,tempCount,tempSprite,tempisEmpty);
        DestroyDraggedObject();
    }
    internal void SwapUiItemInventoryToHotbar(int droppedItemID, int draggedItemID){
        var tempName = inventoryUIItems[draggedItemID].itemName;
        var tempCount = inventoryUIItems[draggedItemID].itemCount;
        var tempSprite = inventoryUIItems[draggedItemID].itemImage.sprite;
        var tempisEmpty = inventoryUIItems[draggedItemID].isEmpty;

        var droppedItemData = hotbarUIItems[droppedItemID];
        inventoryUIItems[draggedItemID].SwapWithData(droppedItemData.itemName, droppedItemData.itemCount,
        droppedItemData.itemImage.sprite, droppedItemData.isEmpty);
    
        hotbarUIItems[droppedItemID].SwapWithData(tempName,tempCount,tempSprite,tempisEmpty);
        DestroyDraggedObject();
    }

    public void HighlightSelectedItem(int ui_id)
    {
        if (hotbarUIItems.ContainsKey(ui_id))
        {
            return;
        }
        inventoryUIItems[ui_id].ToggleHoghlight(true);
    }

    public void DisableHighlightForSelectedItem(int ui_id)
    {
        if (hotbarUIItems.ContainsKey(ui_id))
        {
            return;
        }
        inventoryUIItems[ui_id].ToggleHoghlight(false);
    }
}
