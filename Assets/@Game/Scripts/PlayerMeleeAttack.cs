using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
struct AttackStateDamagePair
{
    public string stateName;
    public int damage;
}

public enum MeleeAttackState
{
    // 공격 애니메이션 중 실제로 휘두르는 시간을 포함한 핵심적인 상태입니다.
    // 이 상태에서는 어떠한 입력도 처리하지 않습니다.
    KeyTime,

    // 이 상태 이후로부터 다음 공격에 대한 입력을 미리 받아 공격을 예약할 수 있습니다.
    CanReserveAttack,

    // 공격 예약이 있었거나 또는 다음 공격 입력을 받은 경우 다음 공격을 수행합니다.
    CanDoNext,

    // 어떠한 상태로도 진행할 수 있습니다.
    CanDoAnything,
}

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField] private Collider m_WeaponCollider;
    [SerializeField] private PlayerAnimation m_PlayerAnim;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private PlayerCameraController m_PlayerCam;
    [SerializeField] private AttackStateDamagePair[] m_AttackList;
    [SerializeField] private GameObject m_Prefab_HitParticle;
    [SerializeField] private LayerMask m_HittableMask;

    private int m_AttackIndex = -1;
    private MeleeAttackState m_State = MeleeAttackState.CanDoAnything;
    private bool m_bGoNext = true;
    private List<Collider> m_HitList = new List<Collider>();
    private Coroutine m_TimeScaleCoroutine = null;

    public MeleeAttackState GetState() => m_State;
    public bool ShouldAttackThisFrame() => m_bGoNext && m_State >= MeleeAttackState.CanDoNext;

    private void Start()
    {
        EndAttack();
    }

    private void Update()
    {
        if (ShouldAttackThisFrame())
        {
            StartAttack();
        }
    }

    public void StartAttackJudgeState()
    {
        m_WeaponCollider.enabled = true;
        var _triggerReceiver = m_WeaponCollider.GetOrAddComponent<TriggerEventReceiver>();
        _triggerReceiver.TriggerEnterEvent.AddListener(OnTriggerEnter);
    }

    public void EndAttackJudgeState()
    {
        m_WeaponCollider.enabled = false;
        var _triggerReceiver = m_WeaponCollider.GetOrAddComponent<TriggerEventReceiver>();
        _triggerReceiver.TriggerEnterEvent.RemoveListener(OnTriggerEnter);
    }

    public void CanReserveGoNext()
    {
        m_State = MeleeAttackState.CanReserveAttack;
    }

    public void CanGoNext()
    {
        m_State = MeleeAttackState.CanDoNext;
    }

    public void StartAttack()
    {
        ++m_AttackIndex;
        m_State = MeleeAttackState.KeyTime;
        m_bGoNext = false;
        m_HitList.Clear();
        m_PlayerAnim.Play(m_AttackList[m_AttackIndex].stateName);
        m_PlayerMovement.SetDontMove(true);
        if (m_PlayerMovement.GetMoveDirection() != Vector3.zero)
            m_PlayerMovement.SetDesiredRotation(Quaternion.LookRotation(m_PlayerMovement.GetMoveDirection()));
    }

    public void EndAttack()
    {
        m_AttackIndex = -1;
        m_State = MeleeAttackState.CanDoAnything;
        m_bGoNext = false;
        m_PlayerMovement.SetDontMove(false);
    }

    public void AttemptAttack()
    {
        if (m_State >= MeleeAttackState.CanReserveAttack)
            m_bGoNext = true;
    }

    public void CancelAttack()
    {
        EndAttack();
    }

    private void OnTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Enemy") && m_HitList.Contains(_collider) == false)
        {
            // trigger enter 대상이 적이며 이번 공격에서 아직 때리지 않은 대상일 때 실행됩니다.
            m_HitList.Add(_collider);

            // timescale animation을 실시합니다.
            if (m_TimeScaleCoroutine != null) StopCoroutine(m_TimeScaleCoroutine);
            m_TimeScaleCoroutine = StartCoroutine(TimeScaleCoroutine());

            // 적의 위치에 파티클을 생성합니다.
            Vector3 _dirToEnemy = _collider.bounds.center - m_PlayerCam.transform.position;
            RaycastHit _hitInfo;
            bool _hit = Physics.Raycast(new Ray() { origin = m_PlayerCam.transform.position, direction = _dirToEnemy },
                out _hitInfo, 10.0f, m_HittableMask);
            if (_hit)
            {
                GameObject _particle =
                    GameObject.Instantiate(m_Prefab_HitParticle, _hitInfo.point, Quaternion.LookRotation(_dirToEnemy));
                Destroy(_particle, 1.0f);
            }


            Debug.Log($"공격! 데미지 {m_AttackList[m_AttackIndex].damage}");
        }
    }

    private IEnumerator TimeScaleCoroutine()
    {
        float _timeScaleDelay = 0.2f;
        float _timeScale = 0.1f;

        Time.timeScale = _timeScale;
        yield return new WaitForSeconds(_timeScaleDelay * _timeScale);
        Time.timeScale = 1.0f;

        m_TimeScaleCoroutine = null;
    }
}