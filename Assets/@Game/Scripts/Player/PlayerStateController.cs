using System;
using UnityEngine;
using FSM;
using UnityEngine.Serialization;

public class PlayerStateController : MonoBehaviour
{
    // 이동, 회피, 공격
    public enum PlayerState
    {
        Locomotion,
        Dodge,
        MeleeAttack
    }

    [SerializeField] private PlayerInputContext m_PlayerInput;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private PlayerMeleeAttack m_PlayerMeleeAttack;

    private StateMachine m_FSM;

    void Start()
    {
        m_FSM = new StateMachine();

        var _playerStateLocomotion = new PlayerState_Locomotion()
            { m_Input = m_PlayerInput, m_Movement = m_PlayerMovement };
        var _playerMeleeAttack = new PlayerState_MeleeAttack()
            { m_Input = m_PlayerInput, m_PlayerAttack = m_PlayerMeleeAttack };

        m_FSM.AddState(PlayerState.Locomotion.ToString(), _playerStateLocomotion);
        m_FSM.AddState(PlayerState.MeleeAttack.ToString(), _playerMeleeAttack);

        m_FSM.AddTransition(
            PlayerState.Locomotion.ToString(),
            PlayerState.MeleeAttack.ToString(),
            t => m_PlayerInput.GetInputMeleeAttack());

        m_FSM.AddTransition(
            PlayerState.MeleeAttack.ToString(),
            PlayerState.Locomotion.ToString(),
            t => m_PlayerMeleeAttack.GetState() >= MeleeAttackState.CanDoAnything);

        m_FSM.SetStartState(PlayerState.Locomotion.ToString());
        m_FSM.Init();
    }

    private void Update()
    {
        m_FSM.OnLogic();
    }

    private void FixedUpdate()
    {
        m_FSM.OnAction(nameof(FixedUpdate));
    }
}