using UnityEngine;

public class PlayerSkill_Dodge : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_PlayerMovement;
    [SerializeField] private PlayerAnimation m_PlayerAnim;
    [SerializeField] private AnimationCurve m_DodgeSpeedMultiplierCurve;
    [SerializeField] private KeyCode m_DodgeKey = KeyCode.Space;

    private bool m_bPlayingDodge;
    private Vector3 m_DodgeDirection;
    
    public bool GetPlayingDodge() => m_bPlayingDodge;

    public void StartDodge()
    {
        m_PlayerMovement.SetDontMove(true);
        m_bPlayingDodge = true;
        m_DodgeDirection = m_PlayerMovement.GetMoveDirection();
        m_PlayerMovement.SetDesiredRotation(Quaternion.LookRotation(m_DodgeDirection));
        m_PlayerAnim.Play("Dodge");
    }

    /// <summary>
    /// Dodge AnimationClip에 의해 실행되는 키프레임 이벤트 콜백입니다.
    /// </summary>
    public void EndDodge()
    {
        m_PlayerMovement.SetDontMove(false);
        m_bPlayingDodge = false;
    }
    
    private void FixedUpdate()
    {
        if (m_bPlayingDodge)
        {
            float _velocityMultiplier =
                m_DodgeSpeedMultiplierCurve.Evaluate(m_PlayerAnim.GetCurrentClipPlayingTimeNormalized());
            Vector3 _velocity = m_DodgeDirection * 70.0f * _velocityMultiplier * Time.fixedDeltaTime;
            m_PlayerMovement.SetPlaneVelocity(_velocity);
        }
    }
}