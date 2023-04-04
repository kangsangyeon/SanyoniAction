using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class AnimationEventReceiver : MonoBehaviour
{
    private Animator m_Anim;
    private Dictionary<string, UnityAction> m_EventDict = new Dictionary<string, UnityAction>();

    private void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    public void AddEvent(string _functionName, UnityAction _callback)
    {
        Assert.IsTrue(m_EventDict.ContainsKey(_functionName) == false);
        m_EventDict.Add(_functionName, _callback);
    }

    public void RemoveEvent(string _functionName)
    {
        Assert.IsTrue(m_EventDict.ContainsKey(_functionName));
        m_EventDict.Remove(_functionName);
    }

    private void OnAnimationEvent(string _functionName)
    {
        if (m_EventDict.ContainsKey(_functionName) == false)
        {
            string _clipName = m_Anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            Debug.LogError($"AnimationEvent (clip){_clipName}::{_functionName}가 등록되지 않았습니다.");
            return;
        }

        m_EventDict[_functionName].Invoke();
    }
}