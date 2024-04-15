using System;
using System.Collections;
using System.Collections.Generic;
using MilkShake;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerSkill_SprintMeleeAttack : MonoBehaviour
{
    [SerializeField] private Collider m_WeaponCollider;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private PlayerAnimation m_PlayerAnim;
    [SerializeField] private AnimationCurve m_SpeedMultiplierCurve;

    [SerializeField] private GameObject m_Prefab_AttackSweepBox;

    private bool m_bPlaying;
    private Vector3 m_AttackDirection;

    public bool GetPlaying() => m_bPlaying;

    private void OnEnable()
    {
        m_PlayerAnim.GetAnimEventReceiver().AddEvent(nameof(EndSprintMeleeAttack), EndSprintMeleeAttack);
        m_PlayerAnim.GetAnimEventReceiver().AddEvent(nameof(SpawnSwordSweepBox), SpawnSwordSweepBox);
    }

    private void OnDisable()
    {
        m_PlayerAnim.GetAnimEventReceiver().RemoveEvent(nameof(EndSprintMeleeAttack));
        m_PlayerAnim.GetAnimEventReceiver().RemoveEvent(nameof(SpawnSwordSweepBox));
    }

    private void FixedUpdate()
    {
        if (m_bPlaying)
        {
            float _velocityMultiplier =
                m_SpeedMultiplierCurve.Evaluate(m_PlayerAnim.GetCurrentClipPlayingTimeNormalized());
            Vector3 _velocity = m_AttackDirection * 70.0f * _velocityMultiplier * Time.fixedDeltaTime;
            m_PlayerMovement.SetPlaneVelocity(_velocity);
        }
    }

    public void StartSprintMeleeAttack()
    {
        m_bPlaying = true;
        m_AttackDirection = m_PlayerMovement.GetMoveDirection();
        m_WeaponCollider.enabled = true;
        m_PlayerMovement.SetDesiredRotation(Quaternion.LookRotation(m_AttackDirection));
        m_PlayerMovement.SetDontMove(true);
        m_PlayerAnim.Play("SprintMeleeAttack");
    }

    public void EndSprintMeleeAttack()
    {
        m_bPlaying = false;
        m_WeaponCollider.enabled = false;
        m_PlayerMovement.SetDontMove(false);
    }

    public void SpawnSwordSweepBox()
    {
        GameObject.Instantiate(
            m_Prefab_AttackSweepBox,
            m_PlayerMovement.GetPlayerCenter() + m_AttackDirection * 1.0f,
            Quaternion.LookRotation(m_AttackDirection, Vector3.up));
    }
}