using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    protected CharacterController characterController;
    protected HumanoidAnimations agentAnimations;
    public float movementSpeed;
    public float gravity;
    public float rotationSpeed;
    public float jumpSpeed;

    public int angleRotationThreshold;

    protected Vector3 moveDirection = Vector3.zero;

    protected float desiredRotationAngler = 0;

    int inputVerticalDirection = 0;

    bool isJumping = false;
    bool finishedJumping = true;

    private bool temporaryMovementTrigger = false;
    private Quaternion endRotationY;
    private float temporaryDisiredRotationAngle;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        agentAnimations = GetComponent<HumanoidAnimations>();
    }

    public bool IsGround()
    {
        return characterController.isGrounded;
    }

    public void HandleMovement(Vector2 input)
    {
        if (characterController.isGrounded)
        {
            if (input.y != 0)
            {
                temporaryMovementTrigger = false;
                if (input.y > 0)
                {
                    inputVerticalDirection = Mathf.CeilToInt(input.y);
                }
                else
                {
                    inputVerticalDirection = Mathf.FloorToInt(input.y);
                }
                moveDirection = input.y * transform.forward * movementSpeed;
            }
            else
            {
                if(input.x != 0)
                {
                    if(temporaryMovementTrigger == false)
                    {
                        temporaryMovementTrigger = true;

                        int directionParameter = input.x > 0 ? 1 : -1;
                        if(directionParameter > 0)
                        {
                            temporaryDisiredRotationAngle = 90;
                        }
                        else
                        {
                            temporaryDisiredRotationAngle = -90;
                        }
                        endRotationY = Quaternion.Euler(transform.localEulerAngles) * Quaternion.Euler(Vector3.up*temporaryDisiredRotationAngle);
                    }
                    inputVerticalDirection = 1;
                    moveDirection = input.y * transform.forward * movementSpeed;

                }
                else
                {
                    temporaryMovementTrigger = false;
                    agentAnimations.SetMovementFloat(0);
                    moveDirection = Vector3.zero;
                }
                
            }
        }

        
    }

    public void HandleMovementDirection(Vector3 input)
    {
        if (temporaryMovementTrigger)
        {
            return;
        }
        desiredRotationAngler = Vector3.Angle(transform.forward, input);
        var crossProduct = Vector3.Cross(transform.forward, input).y;
        if(crossProduct < 0)
        {
            desiredRotationAngler *= -1;
        }
    }

    public void HandleJump()
    {
        if (characterController.isGrounded)
        {
            isJumping = true;
        }
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            if (moveDirection.magnitude > 0 && finishedJumping)
            {
                var animationSpeedMultiplier = agentAnimations.SetCorrectAnimation(desiredRotationAngler, angleRotationThreshold, inputVerticalDirection);
                if (temporaryMovementTrigger == false)
                {
                    RotateAgent();
                }
                else
                {
                    RotateTemp();
                }
                moveDirection *= animationSpeedMultiplier;
            }
        }
        moveDirection.y -= gravity;
        if (isJumping)
        {
            isJumping = false;
            finishedJumping = false;
            moveDirection.y = jumpSpeed;
            agentAnimations.SetMovementFloat(0);
            agentAnimations.TriggerJumpAnimation();
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void RotateTemp()
    {
        desiredRotationAngler = Quaternion.Angle(transform.rotation,endRotationY);
        if(desiredRotationAngler > angleRotationThreshold || desiredRotationAngler < -angleRotationThreshold)
        {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, endRotationY, Time.deltaTime * rotationSpeed * 100);
        }
    }

    private void RotateAgent()
    {
        if(desiredRotationAngler > angleRotationThreshold || desiredRotationAngler < -angleRotationThreshold)
        {
            transform.Rotate(Vector3.up * desiredRotationAngler * rotationSpeed * Time.deltaTime);
        }
    }

    public void StopMovementImmediatelly()
    {
        moveDirection = Vector3.zero;
    }

    public bool HasFinishedJumping()
    {
        return finishedJumping;
    }

    public void SetFinishedJumping(bool value)
    {
        finishedJumping = true;
    }

    public void SetFinishedJumpingTrue()
    {
        finishedJumping = true;
    }

    public void SetFinishedJumpingFalse()
    {
        finishedJumping = false;
    }
}
