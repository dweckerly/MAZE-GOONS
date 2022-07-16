using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine _stateMachine) : base(_stateMachine) {}

    public float gravity = 20.0f;
    Vector3 movement = Vector3.zero;

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick(float deltaTime)
    {
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.animator.SetFloat("speedPercent", 0, 0.1f, deltaTime);
            return;
        }
        movement = new Vector3(stateMachine.InputReader.MovementValue.x, movement.y, stateMachine.InputReader.MovementValue.y);
        if (!stateMachine.Controller.isGrounded)
        {
            movement.y -= gravity * Time.deltaTime;
        }
        stateMachine.Controller.Move(movement * stateMachine.freeLookSpeed * deltaTime);
        Vector3 lookRotation = new Vector3(movement.x, 0, movement.z);
        stateMachine.transform.rotation = Quaternion.LookRotation(lookRotation);
        float currentSpeed = new Vector2(stateMachine.Controller.velocity.x, stateMachine.Controller.velocity.z).magnitude;
        stateMachine.animator.SetFloat("speedPercent", currentSpeed / stateMachine.freeLookSpeed, 0.1f, deltaTime);
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    
}
