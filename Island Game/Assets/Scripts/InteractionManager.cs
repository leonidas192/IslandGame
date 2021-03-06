﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public AgentController agentController;
    public bool UseItem(ItemSO itemData)
    {
        var itemType = itemData.GetItemType();
        switch (itemType)
        {
            case ItemType.None:
                throw new System.Exception("Item can't have itemtype of NONE");
            case ItemType.Food:
                FoodItemSO foodData = (FoodItemSO)itemData;
                Debug.Log("Boosting player stats with food");
                agentController.playerStatsManager.Health += foodData.hungerBonus;
                agentController.playerStatsManager.Stamina += foodData.energyBonus;
                return true;
            case ItemType.Weapon:
                WeaponItemSO weapon = (WeaponItemSO)itemData;
                Debug.Log("Equiping Weapon");
                return false;
            default:
                Debug.Log("Cant use an item of type " + itemType.ToString());
                break;
        }
        return false;
    }
    internal bool EquipItem(ItemSO itemData)
    {
        var itemType = itemData.GetItemType();
        switch (itemType)
        {
            case ItemType.None:
                throw new System.Exception("Item can't be none");
            case ItemType.Weapon:
                return true;
            default:
                break;
        }
        return false;
    }
}
