using FSM;
using UnityEngine;

public class PlayerState_Locomotion : ActionState
{
    public PlayerMovement m_Movement;
    
    public PlayerState_Locomotion() : base(false)
    {
        AddAction(nameof(FixedUpdate), FixedUpdate);
    }

    public override void OnLogic()
    {
        base.OnLogic();

        if (m_Movement.GetInputJump())
        {
            m_Movement.Jump();
        }
    }

    public void FixedUpdate()
    {
        Debug.Log("hello");
        MovePlayer();
    }

    private void MovePlayer()
    {
        float _moveSpeed = m_Movement.GetDefaultMoveSpeed();
        if (m_Movement.GetInputSprint()) _moveSpeed *= m_Movement.GetSprintSpeedMultiplier();

        m_Movement.SetPlaneVelocity(m_Movement.GetMoveDirection() * _moveSpeed * Time.fixedDeltaTime);
        
        Debug.Log(m_Movement.GetMoveDirection() * _moveSpeed);
    }
}