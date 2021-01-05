using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void EnterState(AgentController controller){
        base.EnterState(controller);
        controllerReference.movement.StopMovement();
        controllerReference.agentAnimations.OnFinishAttacking += TransitionBack;
        controllerReference.agentAnimations.TriggerAttackAnimation();
        controllerReference.detectionSystem.OnAttackSuccessful += PerformHit;

        controllerReference.playerStatsManager.Stamina -= 20;
    }
    
    public void TransitionBack(){
        controllerReference.agentAnimations.OnFinishAttacking -= TransitionBack; 
        controllerReference.detectionSystem.OnAttackSuccessful -= PerformHit;
        controllerReference.TransitionToState(controllerReference.movementState);
    }

    public void PerformHit(Collider hitObject, Vector3 hitPosition){
        var hittable = hitObject.GetComponent<IHittable>();
        if(hittable != null){
            var equippedItem = ItemDataManager.instance.GetItemData(controllerReference.inventorySystem.EquippedWeaponID);
            hittable.GetHit((WeaponItemSO)equippedItem, hitPosition);
        }
    }
}
