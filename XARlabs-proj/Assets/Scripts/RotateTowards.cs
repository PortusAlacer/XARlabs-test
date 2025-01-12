using System;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    [SerializeField] private Transform m_TargetTransform;

    [SerializeField] private float m_AngularSpeed = 1f;
    
    private void Start()
    {
        Debug.Assert(m_TargetTransform != null, $"{nameof(m_TargetTransform)} can't be null in object {name}");
    }

    private void Update()
    {
        Vector3 targetDirection = m_TargetTransform.position - transform.position;
        
        float singleStep = m_AngularSpeed * Time.deltaTime;
        
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
