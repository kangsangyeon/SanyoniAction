using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float GROUND_RAY_LENGTH = 0.2f;

    [SerializeField] private float m_MoveSpeed = 250.0f;
    [SerializeField] private float m_JumpForce = 5.0f;
    [SerializeField] private int m_MaxJumpCount = 2;

    [SerializeField] private KeyCode m_JumpKey = KeyCode.Space;

    [SerializeField] private LayerMask m_GroundMask;

    private Collider m_Collider;
    private Rigidbody m_RigidBody;
    private Vector2 m_Input;
    private Vector3 m_MoveDirection;
    private bool m_bGrounded;
    private bool m_bGroundedPrevFrame;
    private int m_JumpCount;

    private bool m_bDebug = true;

    public Vector3 GetPlayerCenter() => m_Collider.bounds.center;
    public float GetPlayerHeight() => m_Collider.bounds.size.y;
    public bool IsBeGroundedThisFrame() => m_bGrounded == true && m_bGroundedPrevFrame == false;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateGrounded();

        if (IsBeGroundedThisFrame())
        {
            m_JumpCount = 0;
        }

        UpdateInput();

        m_bGroundedPrevFrame = m_bGrounded;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateGrounded()
    {
        if (m_RigidBody.velocity.y > 0)
        {
            m_bGrounded = false;
            return;
        }

        float _rayDistance = GetPlayerHeight() * 0.5f + 0.1f;
        Vector3 _rayOrigin = GetPlayerCenter();
        Vector3 _rayEnd = _rayOrigin + Vector3.down * _rayDistance;
        Vector3 _rayHitEnd = Vector3.zero;

        RaycastHit _hitInfo;
        m_bGrounded = Physics.Raycast(new Ray() { origin = _rayOrigin, direction = Vector3.down }, out _hitInfo, _rayDistance, m_GroundMask);

        if (m_bGrounded)
            _rayHitEnd = _hitInfo.point;

        if (m_bDebug)
        {
            Debug.DrawLine(_rayOrigin, _rayEnd, Color.green);
            if (m_bGrounded) Debug.DrawLine(_rayOrigin, _rayHitEnd, Color.red);
        }
    }

    private void UpdateInput()
    {
        m_Input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(m_JumpKey) && m_JumpCount < m_MaxJumpCount)
        {
            // 점프 카운트가 남아있을 때 점프가 가능합니다.
            // 땅에서 떨어졌을 때부터 최대 몇 번까지 공중에서 점프가 가능한지를
            // 제한하기 위함입니다.
            ++m_JumpCount;
            Jump();
        }
    }

    private void MovePlayer()
    {
        m_MoveDirection = (transform.right * m_Input.x + transform.forward * m_Input.y).normalized;

        Vector3 _force = m_MoveDirection * m_MoveSpeed * Time.fixedDeltaTime;
        m_RigidBody.velocity = _force;
    }

    private void Jump()
    {
        // 기존의 y축 velocity를 없앱니다.
        // 그 뒤 위쪽 방향으로 점프력만큼 올립니다.

        m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);

        Vector3 _force = transform.up * m_JumpForce;
        m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);

        if (m_bDebug) Debug.Log(_force);
    }
}