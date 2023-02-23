using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GeneratorSettings : ScriptableObject
{
    [SerializeField] public int seed = 1;
    [SerializeField] public float noiseScale = 1.0f;
    [SerializeField] public float amplitude = 10;
    [SerializeField] public float frequency = 0.02f;
    [SerializeField] public int octaves = 8;
    [SerializeField, Range(0f, 1f)] public float groundPercent = 0.2f;
}
