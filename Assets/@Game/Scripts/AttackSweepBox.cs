using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AttackSweepBox : MonoBehaviour
{
    [SerializeField] private int m_Damage = 50;
    [SerializeField] private float m_Duration = 2.0f;
    [SerializeField] private float m_MoveDistance = 2.0f;
    [SerializeField] private AnimationCurve m_Curve;
    [SerializeField] private LayerMask m_HittableMask;
    [SerializeField] private GameObject m_Prefab_HitParticle;

    private List<Collider> m_HitList = new List<Collider>();
    private Coroutine m_TimeScaleCoroutine = null;

    private void OnEnable()
    {
        m_HitList.Clear();
        transform.DOMove(transform.position + transform.forward * m_MoveDistance, m_Duration)
            .SetEase(m_Curve)
            .OnComplete(() => Destroy(gameObject));
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
            Vector3 _dirToEnemy = (_collider.bounds.center - transform.position).normalized;
            RaycastHit _hitInfo;
            bool _hit = Physics.Raycast(new Ray() { origin = transform.position, direction = _dirToEnemy },
                out _hitInfo, 10.0f, m_HittableMask);
            if (_hit)
            {
                GameObject _particle =
                    GameObject.Instantiate(m_Prefab_HitParticle, _hitInfo.point, Quaternion.LookRotation(_dirToEnemy));
            }


            Debug.Log($"공격! 데미지 {m_Damage}");
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