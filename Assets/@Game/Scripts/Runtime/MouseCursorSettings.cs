using UnityEngine;

public class MouseCursorSettings : MonoBehaviour
{
    [SerializeField] private KeyCode m_ToggleCursorKey = KeyCode.LeftAlt;
    
    private void Start()
    {
        HideCursor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(m_ToggleCursorKey))
        {
            if (Cursor.visible) HideCursor();
            else ShowCursor();
        }
    }

    public void ShowCursor()
    {
        // Cursor.lockState를 Locked로 설정되어 있을 때 Cursor.visible는 false로 고정된 채로 바뀌지 않습니다.
        // 따라서 lockState를 먼저 None으로 변경한 후 visible을 변경해야 합니다.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}