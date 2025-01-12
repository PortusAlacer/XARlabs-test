using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class LissajousAnimation : MonoBehaviour
{
    [SerializeField] private float A = 1f;
    [SerializeField] private float B = 1f;
    [SerializeField] private float minorA = 1f;
    [SerializeField] private float minorB = 1f;
    [SerializeField] private float delta = 1f;
    
    private void Update()
    {
        transform.position =
            new UnityEngine.Vector3(A * Mathf.Sin(minorA * Time.time + delta), B * Mathf.Sin(minorB * Time.time), 0f);
    }
}
