﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
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
}
