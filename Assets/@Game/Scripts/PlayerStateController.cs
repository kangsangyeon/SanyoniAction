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
        Attack
    }

    [SerializeField] private PlayerMovement m_PlayerMovement;

    private StateMachine m_FSM;

    void Start()
    {
        m_FSM = new StateMachine();

        var playerStateLocomotion = new PlayerState_Locomotion() { m_Movement = m_PlayerMovement };

        m_FSM.AddState(PlayerState.Locomotion.ToString(), playerStateLocomotion);

        // fsm.AddState("FollowPlayer", new State(
        //     onLogic: (state) => MoveTowardsPlayer(1)
        // ));

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