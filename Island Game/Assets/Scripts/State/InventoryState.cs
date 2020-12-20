﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : BaseState
{

    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        Debug.Log("Open Inventory Window");
        controllerReference.inventorySystem.ToggleInventory();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        Debug.Log("Close Inventory");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.TransitionToState(controllerReference.movementState);
    }



}