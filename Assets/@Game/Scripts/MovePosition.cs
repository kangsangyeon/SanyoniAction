using UnityEngine;

public class MovePosition : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    private void Update()
    {
        transform.position = m_Target.position;
    }
}