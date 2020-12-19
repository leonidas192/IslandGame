﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : BaseState
{
    float fallingDelay = 0;
    float defaultFallingDelay = 0.2f;
    public override void EnterState(AgentController controller)
    {
        base.EnterState(controller);
        fallingDelay = defaultFallingDelay;
    }

    public override void HandleMovement(Vector2 input)
    {
        base.HandleMovement(input);
        controllerReference.movement.HandleMovement(input);
        
    }

    public override void HandleCameraDirection(Vector3 input)
    {
        base.HandleCameraDirection(input);
        controllerReference.movement.HandleMovementDirection(input);
    }

    public override void HandleJumpInput()
    {
        controllerReference.TransitionToState(controllerReference.jumpState);
    }

    public override void HandleInventoryInput()
    {
        base.HandleInventoryInput();
        controllerReference.TransitionToState(controllerReference.inventoryState);
    }

    public override void Update()
    {
        base.Update();
        HandleMovement(controllerReference.input.MovementInputVector);
        HandleCameraDirection(controllerReference.input.MovementDirectionVector);
        if(controllerReference.movement.IsGround() == false)
        {
            if(fallingDelay > 0)
            {
                fallingDelay -= Time.deltaTime;
                return;
            }
            controllerReference.TransitionToState(controllerReference.fallingState);
        }
        else
        {
            fallingDelay = defaultFallingDelay;
        }
    }
}
