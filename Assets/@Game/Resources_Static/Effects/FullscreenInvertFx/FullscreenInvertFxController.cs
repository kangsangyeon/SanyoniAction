using System.Linq;
using Cyan;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FullscreenInvertFxController : MonoBehaviour
{
    [SerializeField] private UniversalRendererData m_RendererData;
    [SerializeField] private float m_Duration;
    [SerializeField] private AnimationCurve m_Curve;

    [SerializeField] private string m_FeatureName;
    // [SerializeField] private Material m_Material;
    // [SerializeField] private float m_PropertyName;

    private Blit m_Feature;
    private bool m_FeatureActiveOrigin;
    private Tweener m_Tweener;

    private bool TryGetFeature(out Blit _outFeature)
    {
        if (m_Feature)
        {
            _outFeature = m_Feature;
            return true;
        }

        m_Feature = m_RendererData.rendererFeatures.First(f => f.name == m_FeatureName) as Cyan.Blit;
        _outFeature = m_Feature;
        return m_Feature != null;
    }

    private void Start()
    {
        if (TryGetFeature(out var _feature))
        {
            m_FeatureActiveOrigin = _feature.isActive;
            OnReset();
        }
    }

    private void OnDestroy() => OnReset();

    private void OnReset()
    {
        if (TryGetFeature(out var _feature))
        {
            _feature.blitPass.blitMaterial.SetFloat("_Weight", 0);
            _feature.SetActive(m_FeatureActiveOrigin);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Invoke();
        }
    }

    public void Invoke()
    {
        if (TryGetFeature(out var _feature))
        {
            if (m_Tweener != null && m_Tweener.IsPlaying())
            {
                m_Tweener.Rewind();
            }

            _feature.blitPass.blitMaterial.SetFloat("_Weight", 1);
            _feature.SetActive(true);

            m_Tweener
                = _feature.blitPass.blitMaterial.DOFloat(0, "_Weight", m_Duration)
                    .SetEase(m_Curve)
                    .OnComplete(OnEndTransition)
                    .SetAutoKill(false);
        }
    }

    private void OnEndTransition()
    {
        if (TryGetFeature(out var _feature))
        {
            _feature.blitPass.blitMaterial.SetFloat("_Weight", 0);
            _feature.SetActive(false);
        }
    }
}