using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStorageButtonsHelper : MonoBehaviour
{
    public Action OnUseBtnClick,OnDropBtnClick;
    public Button useBtn, dropBtn; 
    // Start is called before the first frame update
    void Start()
    {
        useBtn.onClick.AddListener(() => OnUseBtnClick?.Invoke());
        dropBtn.onClick.AddListener(() => OnDropBtnClick?.Invoke());
    }
    
    public void HideAllButtons()
    {
        ToggleGroupButton(false);
        ToggleUseButton(false);
    }

    public void ToggleUseButton(bool val)
    {
        useBtn.interactable = val;
    }

    public void ToggleGroupButton(bool val)
    {
        dropBtn.interactable = val;
    }
}
