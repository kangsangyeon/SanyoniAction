using FSM;

public class PlayerState_Dodge : ActionState
{
    public PlayerInputContext m_Input;
    public PlayerSkill_Dodge m_Dodge;

    public PlayerState_Dodge() : base(true)
    {
    }

    public override void OnEnter()
    {
        m_Dodge.StartDodge();
    }

    public override void OnExit()
    {
        m_Dodge.EndDodge();
    }

    public override void OnLogic()
    {
        if (m_Dodge.GetPlayingDodge() == false)
            fsm.StateCanExit();
    }
}