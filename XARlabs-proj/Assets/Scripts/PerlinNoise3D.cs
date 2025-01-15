using System;
using UnityEngine;

/// <summary>
/// Aux Class to generate 3D Perlin Noise, since Unity only generates 3D
/// Based on: https://www.youtube.com/watch?app=desktop&v=Aga0TBJkchM&t=0s
/// </summary>
[Serializable]
public class PerlinNoise3D
{
    [SerializeField] private float m_shiftX;
    [SerializeField] private float m_shiftY;
    [SerializeField] private float m_shiftZ;
    
    [SerializeField] private float m_Amplitude = 2f;
    
    public float Evaluate(float x, float y, float z)
    {
        float AB = Mathf.PerlinNoise(m_Amplitude * (x + m_shiftX), m_Amplitude * (y + m_shiftY));
        float BC = Mathf.PerlinNoise(m_Amplitude * (y + m_shiftY), m_Amplitude * (z + m_shiftZ));
        float AC = Mathf.PerlinNoise(m_Amplitude * (x + m_shiftX), m_Amplitude * (z + m_shiftZ));
        float BA = Mathf.PerlinNoise(m_Amplitude * (y + m_shiftY), m_Amplitude * (x + m_shiftX));
        float CB = Mathf.PerlinNoise(m_Amplitude * (z + m_shiftZ), m_Amplitude * (y + m_shiftY));
        float CA = Mathf.PerlinNoise(m_Amplitude * (z + m_shiftZ), m_Amplitude * (x + m_shiftX));
        
        float ABC = AB + BC + AC + BA + CB + CA;

        return ABC / 6f;
    }

    public float Evaluate(Vector3 vertex)
    {
        return Evaluate(vertex.x, vertex.y, vertex.z);
    }

    public void ShiftAll(float shift)
    {
        m_shiftX = shift + m_shiftX;
        m_shiftY = shift + m_shiftY;
        m_shiftZ = shift + m_shiftZ;
    }
    
}
