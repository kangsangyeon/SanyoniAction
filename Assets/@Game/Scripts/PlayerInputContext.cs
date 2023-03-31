using UnityEngine;

public class PlayerInputContext : MonoBehaviour
{
    [SerializeField] private KeyCode m_SprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode m_JumpKey = KeyCode.C;
    [SerializeField] private KeyCode m_MeleeAttackKey = KeyCode.Mouse0;

    private float m_InputHorizontal;
    private float m_InputVertical;
    private float m_InputMouseX;
    private float m_InputMouseY;
    private bool m_InputSprint;
    private bool m_InputJump;
    private bool m_InputMeleeAttack;

    public float GetInputHorizontal() => m_InputHorizontal;
    public float GetInputVertical() => m_InputVertical;
    public float GetInputMouseX() => m_InputMouseX;
    public float GetInputMouseY() => m_InputMouseY;
    public bool GetInputSprint() => m_InputSprint;
    public bool GetInputJump() => m_InputJump;
    public bool GetInputMeleeAttack() => m_InputMeleeAttack;

    private void Update()
    {
        m_InputHorizontal = Input.GetAxisRaw("Horizontal");
        m_InputVertical = Input.GetAxisRaw("Vertical");
        m_InputMouseX = Input.GetAxisRaw("Mouse X");
        m_InputMouseY = Input.GetAxisRaw("Mouse Y");
        m_InputSprint = Input.GetKey(m_SprintKey);
        m_InputJump = Input.GetKeyDown(m_JumpKey);
        m_InputMeleeAttack = Input.GetKeyDown(m_MeleeAttackKey);
    }
}