using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int m_MaxHp;
    [SerializeField] private int m_CurrentHp;
    [SerializeField] private float AttackableDistance;

    [SerializeField] private bool m_IsChase;
    [SerializeField] private bool m_IsAttack;
    
    [SerializeField] private Transform m_ChaseTarget;
    [SerializeField] private LayerMask m_PlayerLayer;
    [SerializeField] private BoxCollider m_BasicAttackArea;
    // [SerializeField] private Material m_Material; 나중에 피격할때
    
    private NavMeshAgent m_NavMesh;
    private Rigidbody m_Rigid;
    private BoxCollider m_boxCollider;
    

    void Awake()
    {
        m_NavMesh = GetComponent<NavMeshAgent>();
        m_Rigid = GetComponent<Rigidbody>();
        m_boxCollider = GetComponent<BoxCollider>();
        // m_Material = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    void Update()
    {
        // Debug.Log($"위치: {m_ChaseTarget.position}");
        if (RaycastCone() == true)
        {
            this.Attack();
            m_NavMesh.SetDestination(m_ChaseTarget.position);
        }
    }

    bool RaycastCone()
    {
        // 내 주변에 감지된 친구들의 배열
        Collider[] cols = Physics.OverlapSphere(this.transform.position,5,m_PlayerLayer);
        Vector3 characterToCollider;
        float dot = 0;
        foreach (Collider _collider in cols)
        {
            // 방향을 구한다.
            characterToCollider = (_collider.transform.position-transform.position).normalized;
            
            // Dot: 두 벡터간 (적이 바라보는 방향, 적으로부터 플레이어 까지의 방향)
            dot = Vector3.Dot(characterToCollider, transform.forward);
            if (dot >= Mathf.Cos(55 * Mathf.Deg2Rad))
                return true;
        }
        return false;
    }
    
    void Attack()
    {
        Vector3 myTransform = this.transform.forward;
        Vector3 targetTransform = m_ChaseTarget.position;

        if (Vector3.Distance(myTransform, targetTransform) < AttackableDistance)
        {
            Debug.Log($"공격함");
        }
    }

    // IEnumerator OnDamage()
    // {
    //     
    // }
}
