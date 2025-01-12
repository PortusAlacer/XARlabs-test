using System;
using UnityEngine;

public class ColorUpdate : MonoBehaviour
{
    [SerializeField] private Gradient m_DirectionColorGradient;
    
    [SerializeField] private Transform m_TargetTransform;
    
    private Renderer m_Renderer;
    private Material m_Material;
    private void Start()
    {
        Debug.Assert(m_TargetTransform != null, $"{nameof(m_TargetTransform)} can't be null in object {name}");
    }

    private void Update()
    {
        //This check is necessary as the components of the object as procedurally added
        //this could be removed, by adding the objects directly in the scene
        if (!m_Material)
        {
            m_Renderer = GetComponent<Renderer>();
            m_Material = m_Renderer.material;
            return;
        }
        
        Vector3 targetDirection = m_TargetTransform.position - transform.position;
        
        float dot = Vector3.Dot(transform.forward, targetDirection);

        m_Material.color = m_DirectionColorGradient.Evaluate(Remap(dot, -1f, 1f, 0f, 1f));
    }
    
    private static float Remap (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
