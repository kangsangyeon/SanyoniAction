using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Transform m_CameraArm;
    [SerializeField] private Transform m_Camera;
    [SerializeField] private Transform m_Player;

    [SerializeField] private float m_SensitivityX = 5.0f;
    [SerializeField] private float m_SensitivityY = 5.0f;
    [SerializeField] private float m_Multiplier = 0.1f;

    private float m_RotationX;
    private float m_RotationY;

    private void Start()
    {
        m_RotationY = m_CameraArm.rotation.eulerAngles.y;
        m_RotationX = m_Camera.rotation.eulerAngles.x;
    }

    private void Update()
    {
        float _mouseX = Input.GetAxisRaw("Mouse X") * m_SensitivityX;
        float _mouseY = Input.GetAxisRaw("Mouse Y") * m_SensitivityY;

        // 마우스의 이동 값으로 회전 양을 결정합니다.
        // 위아래 회전할 때, 카메라가 월드를 거꾸로 비추지 않도록 최대 회전값 제한을 둡니다.
        m_RotationY += _mouseX * m_Multiplier;

        m_RotationX -= _mouseY * m_Multiplier;
        m_RotationX = Mathf.Clamp(m_RotationX, -25.0f, 12.5f);

        // 카메라와 캐릭터가 바라보는 방향을 회전시킵니다.
        m_CameraArm.rotation = Quaternion.Euler(0, m_RotationY, 0);
        m_Camera.localRotation = Quaternion.Euler(m_RotationX, 0, 0);
        m_Player.rotation = Quaternion.Euler(0, m_RotationY, 0);
    }
}