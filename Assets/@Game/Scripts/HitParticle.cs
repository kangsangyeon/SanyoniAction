using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HitParticle : MonoBehaviour
{
    [SerializeField] private float m_AliveDuration;
    [SerializeField] private List<Light> m_LightList;

    private void Start()
    {
        Destroy(this.gameObject, m_AliveDuration);
        m_LightList.ForEach(l => l.DOIntensity(0, m_AliveDuration).SetEase(Ease.Linear));
    }
}