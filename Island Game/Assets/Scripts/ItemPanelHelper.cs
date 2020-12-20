﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelHelper : MonoBehaviour
{
    public Action<int, bool> OnClickEvent;

    public Image itemImage;
    [SerializeField]
    private Text nameText, countText;
    public String itemName, itemCount;
    public bool isEmpty = true;
    public Outline outline;
    public bool isHotbarItem = false;

    public Sprite backgroundSprite;

    private void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        if(itemImage.sprite == backgroundSprite)
        {
            ClearImage();
        }
    }

    public void SetInventoryUIElement(string name,int count,Sprite image)
    {
        itemName = name;
        itemCount = count + "";
        if (!isHotbarItem)
            nameText.text = itemName;
        countText.text = itemCount;
        isEmpty = false;
        SetImageSprite(image);

    }

    public void SwapWithData(string name, int count, Sprite image, bool isEmpty)
    {
        SetInventoryUIElement(name, count, image);
        this.isEmpty = isEmpty;
    }

    private void SetImageSprite(Sprite image)
    {
        itemImage.sprite = image;
    }

    private void ClearImage()
    {
        itemName = "";
        itemCount = "";
        countText.text = itemCount;
        if (!isHotbarItem)
            nameText.text = itemName;
        ResetImage();
        isEmpty = true;
        ToggleHighlight(false);
    }

    private void ToggleHighlight(bool val)
    {
        outline.enabled = val;
    }

    private void ResetImage()
    {
        itemImage.sprite = backgroundSprite;
    }
}