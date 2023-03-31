using System;
using UnityEngine;
using UnityEngine.Assertions;

public enum GameFocusMode
{
    None,
    InGame,
    UI
}

public class GameManager : MonoBehaviour
{
    private static GameManager m_Instance;
    public static GameManager Instance => m_Instance;

    [SerializeField] private MouseCursorSettings m_MouseCursorSettings;

    private GameFocusMode m_FocusMode;

    public MouseCursorSettings GetMouseCursorSettings() => m_MouseCursorSettings;
    public GameFocusMode GetFocusMode() => m_FocusMode;

    private void Awake()
    {
        Assert.IsTrue(m_Instance == null);
        m_Instance = this;
    }

    private void OnDestroy()
    {
        Assert.IsTrue(m_Instance != null);
        m_Instance = null;
    }

    private void Update()
    {
        if (Cursor.visible) m_FocusMode = GameFocusMode.UI;
        else m_FocusMode = GameFocusMode.InGame;
    }
}