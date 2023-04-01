using System.Collections;
using System.Collections.Generic;
using MilkShake;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSkill_SprintMeleeAttack : MonoBehaviour
{
    [SerializeField] private Collider m_WeaponCollider;
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private PlayerAnimation m_PlayerAnim;
    [SerializeField] private AnimationCurve m_SpeedMultiplierCurve;

    [SerializeField] private GameObject m_Prefab_HitParticle;
    [SerializeField] private PlayerCameraController m_PlayerCam;
    [SerializeField] private ShakePreset m_ShakePreset;
    [SerializeField] private LayerMask m_HittableMask;

    private bool m_bPlaying;
    private Vector3 m_AttackDirection;

    private bool m_JudgeState;
    private List<Collider> m_HitList = new List<Collider>();
    private Coroutine m_TimeScaleCoroutine = null;

    public bool GetPlaying() => m_bPlaying;

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
        m_HitList.Clear();
        m_WeaponCollider.enabled = true;
        m_PlayerMovement.SetDesiredRotation(Quaternion.LookRotation(m_AttackDirection));
        m_PlayerMovement.SetDontMove(true);
        m_PlayerAnim.Play("SprintMeleeAttack");
    }

    public void EndSprintMeleeAttack()
    {
        if (m_JudgeState)
        {
            EndSprintMeleeAttackJudgeState();
        }

        m_bPlaying = false;
        m_WeaponCollider.enabled = false;
        m_PlayerMovement.SetDontMove(false);
    }

    public void StartSprintMeleeAttackJudgeState()
    {
        m_JudgeState = true;
        m_WeaponCollider.enabled = true;
        var _triggerReceiver = m_WeaponCollider.GetOrAddComponent<TriggerEventReceiver>();
        _triggerReceiver.TriggerEnterEvent.AddListener(OnWeaponTriggerEnter);
    }

    public void EndSprintMeleeAttackJudgeState()
    {
        m_JudgeState = false;
        m_WeaponCollider.enabled = false;
        var _triggerReceiver = m_WeaponCollider.GetOrAddComponent<TriggerEventReceiver>();
        _triggerReceiver.TriggerEnterEvent.RemoveListener(OnWeaponTriggerEnter);
    }

    private void OnWeaponTriggerEnter(Collider _collider)
    {
        if (_collider.CompareTag("Enemy") && m_HitList.Contains(_collider) == false)
        {
            // trigger enter 대상이 적이며 이번 공격에서 아직 때리지 않은 대상일 때 실행됩니다.
            m_HitList.Add(_collider);

            if (m_HitList.Count == 1)
            {
                // 이번 공격의 첫 번째 적을 때렸을 때에만 카메라를 흔듭니다.
                m_PlayerCam.ShakeUsingPreset(m_ShakePreset);
            }

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
            
            Debug.Log("sprint attack hit!!");
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