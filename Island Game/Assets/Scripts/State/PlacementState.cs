using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : MovementState
{
    PlacementHelper placementHelper;

    public override void EnterState(AgentController controller)
    {
        Debug.Log("Entering Placement State");
        base.EnterState(controller);
        CreateStructureToPlace();
    }

    private void CreateStructureToPlace()
    {
        placementHelper = ItemSpawnManager.instance.CreateStructure(controllerReference.inventorySystem.selectedStructureData);
        placementHelper.PrepareForMovement();
        Debug.Log("Creating a structure to place");
    }

    public override void HandleEscapeInput()
    {
        Debug.Log("Exiting placementState");
        if (placementHelper.isActiveAndEnabled)
        {
            DestroyPlaceObject();
        }
        controllerReference.TransitionToState(controllerReference.movementState);
    }

    public override void HandleJumpInput()
    {
        
    }

    public override void HandleInventoryInput()
    {
        
    }

    public override void HandlePrimaryAction()
    {
        if (placementHelper.CorrectLocation)
        {
            var structureComponent = placementHelper.PrepareForPlacement();
            structureComponent.SetData(controllerReference.inventorySystem.selectedStructureData);
            placementHelper.enabled = false;
            controllerReference.inventorySystem.RemoveSelectedStructureFromInventory();
            controllerReference.buildingPlacementStorage.SaveStructureReference(structureComponent);
            HandleEscapeInput();
        }
    }

    public override void HandleSecondaryAction()
    {
        
    }

    public override void HandleHotbarInput(int hotbarKey)
    {
       
    }

    public override void Update()
    {
        HandleMovement(controllerReference.input.MovementInputVector);
        HandleCameraDirection(controllerReference.input.MovementDirectionVector);
        HandleFallingDown();
    }

    protected new void HandleFallingDown()
    {
        if (controllerReference.movement.IsGround() == false)
        {
            if (fallingDelay > 0)
            {
                fallingDelay -= Time.deltaTime;
                return;
            }
            DestroyPlaceObject();
            controllerReference.TransitionToState(controllerReference.fallingState);
        }
        else
        {
            fallingDelay = defaultFallingDelay;
        }
    }

    private void DestroyPlaceObject()
    {
        Debug.Log("Destroying Placed Object");
        placementHelper.DestroyStructure();
    }
}
