using System;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class AnimateVertices : MonoBehaviour
{
    [SerializeField] private PerlinNoise3D m_NoiseGenerator = new PerlinNoise3D();
    
    [SerializeField] private float m_Amplitude = 0.5f;
    
    private MeshFilter m_MeshFilter;
    private Mesh m_Mesh;
    private Vector3[] m_OriginalVertices;
    private Vector3[] m_OriginalNormals;
    
    private void Update()
    {
        if (!m_Mesh)
        {
            m_MeshFilter = GetComponent<MeshFilter>();

            if (!m_MeshFilter)
            {
                return;
            }
            
            m_Mesh = m_MeshFilter.mesh;

            if (!m_Mesh)
            {
                return;
            }

            m_OriginalVertices = new Vector3[m_Mesh.vertexCount];
            m_OriginalNormals = new Vector3[m_Mesh.vertexCount];
            
            for (int i = 0; i < m_Mesh.vertexCount; i++)
            {
                m_OriginalVertices[i] = m_Mesh.vertices[i];
                m_OriginalNormals[i] = m_Mesh.normals[i];
            }
        }
        
        m_NoiseGenerator.ShiftAll(Time.deltaTime);
        
        Vector3[] newVertices = new Vector3[m_OriginalVertices.Length];
        
        for (int i = 0; i < m_OriginalVertices.Length; i++)
        {
            newVertices[i] = m_OriginalVertices[i] + m_OriginalNormals[i] * (m_NoiseGenerator.Evaluate(m_OriginalVertices[i]) * m_Amplitude);
        }
        
        m_Mesh.vertices = newVertices;
    }
}
