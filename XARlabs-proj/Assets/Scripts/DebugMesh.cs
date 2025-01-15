using UnityEngine;

public class DebugMesh : MonoBehaviour
{
    [SerializeField] private MeshFilter m_MeshFilter;
    [SerializeField] private bool m_ShowVertices = true, m_ShowNormals = false;
    
    private Mesh m_Mesh;

    private void Update()
    {
        if (m_Mesh)
        {
            return;
        }

        m_Mesh = m_MeshFilter.sharedMesh;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!m_Mesh)
        {
            return;
        }
        
        if (m_ShowVertices)
        {
            Gizmos.color = Color.green;
            foreach (Vector3 vert in m_Mesh.vertices)
            {
                Gizmos.DrawSphere(m_MeshFilter.transform.TransformPoint(vert), 0.01f);
            }
        }

        if (m_ShowNormals)
        {
            Gizmos.color = Color.yellow;
            for(int i = 0; i < m_Mesh.normals.Length; i ++)
            {
                Gizmos.DrawLine(m_MeshFilter.transform.TransformPoint(m_Mesh.vertices[i]), m_MeshFilter.transform.TransformPoint(m_Mesh.vertices[i]) + m_MeshFilter.transform.TransformDirection(m_Mesh.normals[i]) * 0.5f);
            }
        }
    }
}
