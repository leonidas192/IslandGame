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

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get=>hotbarUIItems.Count;}
    public RectTransform Draggableitem{get => draggableitem;}
    public ItemPanelHelper DraggableItemPanel{get => draggableItemPanel;}

    public Dictionary<int, ItemPanelHelper> inventoryUIItems = new Dictionary<int, ItemPanelHelper>();
    public Dictionary<int, ItemPanelHelper> hotbarUIItems = new Dictionary<int, ItemPanelHelper>();

    private List<int> listOfHotbarElementID = new List<int>();

    public List<ItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUIItems.Values.ToList();
    }

    public GameObject hotbarPanel,storagePanel;

    public GameObject storagePrefab;

    private RectTransform draggableitem;
    private ItemPanelHelper draggableItemPanel;

    public Canvas canvas;

    private void Awake()
    {
        inventoryGeneralPanel.SetActive(false);
        foreach (Transform child in hotbarPanel.transform)
        {
            ItemPanelHelper helper = child.GetComponent<ItemPanelHelper>();
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
        if(inventoryGeneralPanel.activeSelf == false)
        {
            inventoryGeneralPanel.SetActive(true);
        }
        else
        {
            inventoryGeneralPanel.SetActive(false);
            DestroyDraggedObject();
        }
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
            var itemHelper = element.GetComponent<ItemPanelHelper>();
            inventoryUIItems.Add(itemHelper.GetInstanceID(), itemHelper);
        }
    }

    public List<ItemPanelHelper> GetUiElementsForInventory()
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
}
