using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private float GROUND_RAY_LENGTH = 0.2f;

    [SerializeField] private Camera m_Cam;
    [SerializeField] private PlayerInputContext m_Input;

    [SerializeField] private float m_DefaultMoveSpeed = 70.0f;
    [SerializeField] private float m_SprintSpeedMultiplier = 4.0f;
    [SerializeField] private float m_JumpForce = 5.0f;
    [SerializeField] private int m_MaxJumpCount;

    [SerializeField] private LayerMask m_GroundMask;

    private Collider m_Collider;
    private Rigidbody m_RigidBody;
    private PlayerAnimation m_PlayerAnim;
    private bool m_bDontMove;
    private Vector3 m_MoveDirection;
    private bool m_bGrounded;
    private bool m_bGroundedPrevFrame;
    private int m_JumpCount = 0;
    private Quaternion m_DesiredRotation;

    private bool m_bDebug = true;

    public float GetDefaultMoveSpeed() => m_DefaultMoveSpeed;
    public float GetSprintSpeedMultiplier() => m_SprintSpeedMultiplier;

    public Vector3 GetMoveDirection() => m_MoveDirection;
    public Vector3 GetPlayerCenter() => m_Collider.bounds.center;
    public float GetPlayerHeight() => m_Collider.bounds.size.y;
    public bool IsBeGroundedThisFrame() => m_bGrounded == true && m_bGroundedPrevFrame == false;
    public Rigidbody GetRigidBody() => m_RigidBody;

    public void SetDontMove(bool _value) => m_bDontMove = _value;
    public void SetDesiredRotation(Quaternion _rotation) => m_DesiredRotation = _rotation;

    public void SetPlaneVelocity(Vector3 _velocity)
    {
        _velocity.y = m_RigidBody.velocity.y;
        m_RigidBody.velocity = _velocity;
    }

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

        UpdateMovementDirection();
        UpdateRotation();

        m_bGroundedPrevFrame = m_bGrounded;
    }

    private void UpdateRotation()
    {
        if (m_MoveDirection != Vector3.zero && m_bDontMove == false)
        {
            m_DesiredRotation = Quaternion.LookRotation(m_MoveDirection);
        }

        if (Quaternion.Angle(transform.rotation, m_DesiredRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, m_DesiredRotation, 0.1f);
        }
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


    private void UpdateMovementDirection()
    {
        m_MoveDirection = m_Cam.transform.right * m_Input.GetInputHorizontal() +
                          m_Cam.transform.forward * m_Input.GetInputVertical();
        m_MoveDirection.y = 0;
        m_MoveDirection.Normalize();
    }

    public void Jump()
    {
        if (m_JumpCount >= m_MaxJumpCount)
        {
            // 점프 카운트가 남아있을 때 점프가 가능합니다.
            // 땅에서 떨어졌을 때부터 최대 몇 번까지 공중에서 점프가 가능한지를
            // 제한하기 위함입니다.
            return;
        }

        ++m_JumpCount;

        // 기존의 y축 velocity를 없앱니다.
        // 그 뒤 위쪽 방향으로 점프력만큼 올립니다.

        m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);

        Vector3 _force = transform.up * m_JumpForce;
        m_RigidBody.AddForce(transform.up * m_JumpForce, ForceMode.Impulse);
    }
}