using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    //reference AgentController to access input,movement,to other script
    protected AgentController controllerReference;
    //preparing anyting before start doing some activities in the state
    public virtual void EnterState(AgentController controller)
    {
        this.controllerReference = controller;
    }
    //method that responsible for handling input
    public virtual void HandleMovement(Vector2 input) { }

    public virtual void HandleCameraDirection(Vector3 input) { }

    public virtual void HandleJumpInput() { }

    public virtual void Update() { }
}
