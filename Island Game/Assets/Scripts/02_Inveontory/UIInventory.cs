using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public GameObject inventoryGeneralPanel;

    public bool IsInventoryVisible { get => inventoryGeneralPanel.activeSelf; }
    public int HotbarElementsCount { get=>hotbarUIItems.Count;}

    public Dictionary<int, ItemPanelHelper> inventoryUIItems = new Dictionary<int, ItemPanelHelper>();
    public Dictionary<int, ItemPanelHelper> hotbarUIItems = new Dictionary<int, ItemPanelHelper>();

    private List<int> listOfHotbarElementID = new List<int>();

    public List<ItemPanelHelper> GetUiElementsForHotbar()
    {
        return hotbarUIItems.Values.ToList();
    }

    public GameObject hotbarPanel,storagePanel;

    public GameObject storagePrefab;

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
}
