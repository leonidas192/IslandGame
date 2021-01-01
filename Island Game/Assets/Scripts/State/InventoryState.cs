using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryState : BaseState
{

    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        Debug.Log("Open inventory window");
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.craftingSystem.ToggleCraftingUI();
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        Debug.Log("Close Inventory");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        controllerReference.inventorySystem.ToggleInventory();
        controllerReference.craftingSystem.ToggleCraftingUI();
        controllerReference.TransitionToState(controllerReference.movementState);

    }

    public override void HandleEscapeInput(){
        HandleInventoryInput();
        //base.HandleEscapeInput();
        //controllerReference.inventorySystem.ToggleInventory();
        //controllerReference.craftingSystem.ToggleCraftingUI(true);
    }


}
