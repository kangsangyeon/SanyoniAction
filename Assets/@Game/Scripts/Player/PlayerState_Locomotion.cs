using FSM;
using UnityEngine;

public class PlayerState_Locomotion : ActionState
{
    public PlayerInputContext m_Input;
    public PlayerMovement m_Movement;

    public PlayerState_Locomotion() : base(false)
    {
        AddAction(nameof(FixedUpdate), FixedUpdate);
    }

    public override void OnLogic()
    {
        if (m_Input.GetInputJump())
        {
            m_Movement.Jump();
        }
    }

    public void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float _moveSpeed = m_Movement.GetDefaultMoveSpeed();
        if (m_Input.GetInputSprint()) _moveSpeed *= m_Movement.GetSprintSpeedMultiplier();

        m_Movement.SetPlaneVelocity(m_Movement.GetMoveDirection() * _moveSpeed * Time.fixedDeltaTime);
    }
}