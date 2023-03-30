using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform m_CameraArm;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private Rigidbody m_PlayerRigidBody;

    [SerializeField] private float m_SensitivityX = 5.0f;
    [SerializeField] private float m_SensitivityY = 5.0f;
    [SerializeField] private float m_Multiplier = 0.1f;

    [SerializeField] private float m_MinVelocity = 5;
    [SerializeField] private float m_MaxVelocity = 20;
    [SerializeField] private float m_MinFov = 80;
    [SerializeField] private float m_MaxFov = 90;

    private float m_RotationX;
    private float m_RotationY;
    private float m_TargetFov;

    private void Start()
    {
        m_RotationY = m_CameraArm.rotation.eulerAngles.y;
        m_RotationX = m_Camera.transform.rotation.eulerAngles.x;
    }

    private void Update()
    {
        UpdateRotation();
    }

    private void FixedUpdate()
    {
        UpdateTargetFov();
        UpdateCamFov();
    }

    private void UpdateRotation()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * m_SensitivityX;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * m_SensitivityY;

        // 마우스의 이동 값으로 회전 양을 결정합니다.
        // 위아래 회전할 때, 카메라가 월드를 거꾸로 비추지 않도록 최대 회전값 제한을 둡니다.
        m_RotationY += _mouseX * m_Multiplier;

        m_RotationX -= _mouseY * m_Multiplier;
        m_RotationX = Mathf.Clamp(m_RotationX, -25.0f, 12.5f);

        // 카메라가 바라보는 방향을 회전시킵니다.
        m_CameraArm.rotation = Quaternion.Euler(0, m_RotationY, 0);
        m_Camera.transform.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
    }

    private void UpdateTargetFov()
    {
        float _moveSpeedDiff = m_MaxVelocity - m_MinVelocity;
        float _fovDiff = m_MaxFov - m_MinFov;

        float _planeVelocity = new Vector3(m_PlayerRigidBody.velocity.x, 0, m_PlayerRigidBody.velocity.z).magnitude;
        float _velocityExceedAmount = _planeVelocity - m_MinVelocity;

        float _targetFov = m_MinFov;

        if (_velocityExceedAmount <= 0) _targetFov = m_MinFov;
        else if (_velocityExceedAmount >= _moveSpeedDiff) _targetFov = m_MaxFov;
        else
        {
            float _velocityPercentage = Mathf.Clamp01(_velocityExceedAmount / _moveSpeedDiff);
            _targetFov = m_MinFov + (_fovDiff * _velocityPercentage);
        }

        m_TargetFov = _targetFov;
    }

    private void UpdateCamFov()
    {
        // float _fovLerped = Mathf.Lerp(m_Camera.fieldOfView, m_TargetFov, Time.deltaTime * 100.0f);
        float _fovLerped = Mathf.Lerp(m_Camera.fieldOfView, m_TargetFov, 5.0f * Time.deltaTime);
        m_Camera.fieldOfView = _fovLerped;
    }
}