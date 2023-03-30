using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private float GROUND_RAY_LENGTH = 0.2f;

    [SerializeField] private Camera m_Cam;

    [SerializeField] private float m_MoveSpeed = 70.0f;
    [SerializeField] private float m_SprintSpeedMultiplier = 6.0f;
    [SerializeField] private float m_JumpForce = 5.0f;
    [SerializeField] private int m_MaxJumpCount = 2;

    [SerializeField] private KeyCode m_SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode m_JumpKey = KeyCode.Space;
    [SerializeField] private LayerMask m_GroundMask;

    private Collider m_Collider;
    private Rigidbody m_RigidBody;
    private Vector2 m_InputDirection;
    private Vector2 m_InputMouse;
    private bool m_InputSprint;
    private Vector3 m_MoveDirection;
    private bool m_bGrounded;
    private bool m_bGroundedPrevFrame;
    private int m_JumpCount;
    private Quaternion m_DesiredRotation;

    private bool m_bDebug = true;

    public Vector2 GetInputMovement() => m_InputDirection;
    public Vector2 GetInputMouse() => m_InputMouse;
    public bool GetInputSprint() => m_InputSprint;
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

        UpdateRotation();

        m_bGroundedPrevFrame = m_bGrounded;
    }

    private void UpdateRotation()
    {
        if (m_MoveDirection != Vector3.zero)
        {
            m_DesiredRotation = Quaternion.LookRotation(m_MoveDirection);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, m_DesiredRotation, 0.1f);
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
        m_bGrounded = Physics.Raycast(new Ray() { origin = _rayOrigin, direction = Vector3.down }, out _hitInfo,
            _rayDistance, m_GroundMask);

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
        m_InputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        m_InputMouse = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        m_InputSprint = Input.GetKey(m_SprintKey);


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
        m_MoveDirection = m_Cam.transform.right * m_InputDirection.x + m_Cam.transform.forward * m_InputDirection.y;
        m_MoveDirection.y = 0;
        m_MoveDirection.Normalize();

        float _moveSpeed = m_MoveSpeed;
        if (m_InputSprint) _moveSpeed *= m_SprintSpeedMultiplier;

        Vector3 _force = m_MoveDirection * _moveSpeed * Time.fixedDeltaTime;
        _force.y = m_RigidBody.velocity.y;
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