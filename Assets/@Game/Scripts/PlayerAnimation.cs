using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement m_Movement;
    [SerializeField] private Animator m_Anim;
    [SerializeField] private float m_LerpSpeed = 0.1f;


    private float m_MoveSpeed;
    private float m_MouseHorizontal;

    public AnimatorStateInfo GetCurrentAnimStateInfo() => m_Anim.GetCurrentAnimatorStateInfo(0);
    public AnimatorClipInfo GetCurrentClipInfo() => m_Anim.GetCurrentAnimatorClipInfo(0)[0];
    public float GetCurrentClipLength() => GetCurrentClipInfo().clip.length;
    public float GetCurrentClipPlayingTimeNormalized() => GetCurrentAnimStateInfo().normalizedTime;

    private void Update()
    {
        float _moveSpeed = m_Movement.GetInputMovement().normalized.magnitude;
        if (m_Movement.GetInputSprint()) _moveSpeed *= 2;

        float _mouseHorizontal = m_Movement.GetInputMouse().x;

        m_MoveSpeed = Mathf.Lerp(m_MoveSpeed, _moveSpeed, m_LerpSpeed);
        m_MouseHorizontal = Mathf.Lerp(m_MouseHorizontal, _mouseHorizontal, m_LerpSpeed);

        m_Anim.SetFloat("MoveSpeed", m_MoveSpeed);
        m_Anim.SetFloat("MouseHorizontal", m_MouseHorizontal);
    }

    public void Play(string _stateName)
    {
        m_Anim.Play(_stateName);
    }
}