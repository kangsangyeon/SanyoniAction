using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_Movement;
    [SerializeField] private PlayerInputContext m_Input;
    [SerializeField] private Animator m_Anim;
    [SerializeField] private float m_LerpSpeed = 0.1f;


    private float m_MoveSpeed;
    private float m_MouseX;

    public AnimatorStateInfo GetCurrentAnimStateInfo() => m_Anim.GetCurrentAnimatorStateInfo(0);
    public AnimatorClipInfo GetCurrentClipInfo() => m_Anim.GetCurrentAnimatorClipInfo(0)[0];
    public float GetCurrentClipLength() => GetCurrentClipInfo().clip.length;
    public float GetCurrentClipPlayingTimeNormalized() => GetCurrentAnimStateInfo().normalizedTime;

    public AnimationEventReceiver GetAnimEventReceiver() => m_Anim.GetOrAddComponent<AnimationEventReceiver>();

    private void Update()
    {
        float _moveSpeed = m_Movement.GetMoveDirection() != Vector3.zero ? 1 : 0;
        if (m_Input.GetInputSprint()) _moveSpeed *= 2;

        float _mouseX = m_Input.GetInputMouseX();

        if (Mathf.Abs(m_MoveSpeed - _moveSpeed) > 0.001f)
        {
            // MoveSpeed 값이 흔들리지 않도록 합니다.
            m_MoveSpeed = Mathf.Lerp(m_MoveSpeed, _moveSpeed, m_LerpSpeed);
        }

        if (Mathf.Abs(m_MouseX - _mouseX) > 0.001f)
        {
            // MouseX 값이 흔들리지 않도록 합니다.
            m_MouseX = Mathf.Lerp(m_MouseX, _mouseX, m_LerpSpeed);
        }

        m_Anim.SetFloat("MoveSpeed", m_MoveSpeed);
        m_Anim.SetFloat("MouseX", m_MouseX);
    }

    public void Play(string _stateName)
    {
        m_Anim.Play(_stateName);
    }
}