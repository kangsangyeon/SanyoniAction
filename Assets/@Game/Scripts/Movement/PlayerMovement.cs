using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float GROUND_RAY_LENGTH = 0.2f;

    [SerializeField] private float m_MoveSpeed = 500.0f;
    [SerializeField] private float m_JumpForce = 1000.0f;

    [SerializeField] private KeyCode m_JumpKey = KeyCode.Space;

    [SerializeField] private LayerMask m_GroundMask;

    private Collider m_Collider;
    private Rigidbody m_RigidBody;
    private Vector2 m_Input;
    private Vector3 m_MoveDirection;
    private bool m_bGrounded;

    public float GetPlayerHeight() => m_Collider.bounds.size.y;

    private void Awake()
    {
        m_Collider = GetComponent<Collider>();
        m_RigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateGrounded();

        UpdateInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void UpdateGrounded() =>
        m_bGrounded = Physics.Raycast(transform.position, Vector3.down, GetPlayerHeight() * 0.5f + GROUND_RAY_LENGTH, m_GroundMask);

    private void UpdateInput()
    {
        m_Input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // when to jump
        if (Input.GetKey(m_JumpKey) && m_bGrounded)
        {
            Jump();
        }
    }

    private void MovePlayer()
    {
        m_MoveDirection = (transform.right * m_Input.x + transform.forward * m_Input.y).normalized;

        Vector3 _force = m_MoveDirection * m_MoveSpeed;
        m_RigidBody.AddForce(_force, ForceMode.Force);
    }

    private void Jump()
    {
        // 기존의 y축 velocity를 없앱니다.
        // 그 뒤 위쪽 방향으로 점프력만큼 올립니다.

        m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);

        Vector3 _force = transform.up * m_JumpForce;
        m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);

        Debug.Log(_force);
    }
}