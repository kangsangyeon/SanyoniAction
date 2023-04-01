using FSM;
using UnityEngine;

public class PlayerState_MeleeAttack : ActionState
{
    public PlayerInputContext m_Input;
    public PlayerMeleeAttack m_PlayerAttack;

    public PlayerState_MeleeAttack() : base(false)
    {
    }

    public override void OnLogic()
    {
        base.OnLogic();

        if (m_Input.GetInputMeleeAttack()
            && GameManager.Instance.GetFocusMode() == GameFocusMode.InGame)
        {
            m_PlayerAttack.AttemptAttack();
        }
    }
}