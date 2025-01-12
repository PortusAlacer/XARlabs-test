using System;
using UnityEngine;

public class CreateMesh : MonoBehaviour
{
    enum MeshType
    {
        SphereCone
    }
    
    [SerializeField] private GameObject m_ObjectAssigned;
    [SerializeField] private MeshType m_MeshType;
    [SerializeField] private Material m_Material;
    
    [Header("Mesh properties")]
    [SerializeField] private float m_Size = 1f;
    [SerializeField] private float m_Radius = 1f;
    [SerializeField] private int m_VerticalDivisions = 16;
    [SerializeField] private int m_Horizontalivisions = 16;
    
    private void Start()
    {
        Debug.Assert(m_ObjectAssigned != null, $"{nameof(m_ObjectAssigned)} not assigned in ProceduralMeshCreator in object {name}");
        Debug.Assert(m_Material != null, $"{nameof(m_Material)} not assigned in ProceduralMeshCreator in object {name}");
        
        ProceduralMesh proceduralMesh = null;
        
        switch (m_MeshType)
        {
            case MeshType.SphereCone:
                proceduralMesh = new SphereCone(m_Horizontalivisions, m_VerticalDivisions, m_Size, m_Radius);
                break;
            default:
                Debug.Assert(true, $"Procedural Mesh Creator Error: Mesh Type {m_MeshType} is not supported.");
                break;
        }
        
        m_ObjectAssigned.AddComponent<MeshFilter>().sharedMesh = proceduralMesh.Mesh;
        m_ObjectAssigned.AddComponent<MeshRenderer>().material = m_Material;
    }
}
