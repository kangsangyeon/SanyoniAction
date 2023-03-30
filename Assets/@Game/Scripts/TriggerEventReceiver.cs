using UnityEngine;
using UnityEngine.Events;

public class TriggerEventReceiver : MonoBehaviour
{
    private UnityEvent<Collider> m_TriggerEnterEvent = new UnityEvent<Collider>();
    private UnityEvent<Collider> m_TriggerExitEvent = new UnityEvent<Collider>();
    private UnityEvent<Collider> m_TriggerStayEvent = new UnityEvent<Collider>();

    public UnityEvent<Collider> TriggerEnterEvent => m_TriggerEnterEvent;
    public UnityEvent<Collider> TriggerExitEvent => m_TriggerExitEvent;
    public UnityEvent<Collider> TriggerStayEvent => m_TriggerStayEvent;

    private void OnTriggerEnter(Collider other) => m_TriggerEnterEvent.Invoke(other);
    private void OnTriggerExit(Collider other) => m_TriggerExitEvent.Invoke(other);
    private void OnTriggerStay(Collider other) => m_TriggerStayEvent.Invoke(other);
}