using FSM;
using UnityEngine;

public class PlayerState_SprintMeleeAttack : ActionState
{
    public PlayerInputContext m_Input;
    public PlayerSkill_SprintMeleeAttack m_SprintMeleeAttack;
    
    public PlayerState_SprintMeleeAttack() : base(true)
    {
    }

    public override void OnEnter()
    {
        m_SprintMeleeAttack.StartSprintMeleeAttack();
    }
    
    public override void OnExit()
    {
        m_SprintMeleeAttack.EndSprintMeleeAttack();
    }

    public override void OnLogic()
    {
        if (m_SprintMeleeAttack.GetPlaying() == false)
            fsm.StateCanExit();
    }
}