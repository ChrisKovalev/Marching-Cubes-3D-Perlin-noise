using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : MonoBehaviour
{
    ComputeBuffer _weightsBuffer;
    public ComputeShader NoiseShader;
    public GeneratorSettings generatorSettings; 

    private void Awake() {
        CreateBuffers();
    }

    // private void onValidate() {
    //     ReleaseBuffers();
    //     CreateBuffers();
    //     GetNoise();
    // }

    private void OnDestroy() {
        ReleaseBuffers();
    }

    public float[] GetNoise(Vector3 pos) {
        float[] noiseValues =
            new float[GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk];


        NoiseShader.SetBuffer(0, "_Weights", _weightsBuffer);

        NoiseShader.SetInt("_ChunkSize", GridMetrics.PointsPerChunk);
        NoiseShader.SetFloat("_NoiseScale", generatorSettings.noiseScale);
        NoiseShader.SetFloat("_Amplitude", generatorSettings.amplitude);
        NoiseShader.SetFloat("_Frequency", generatorSettings.frequency);
        NoiseShader.SetInt("_Octaves", generatorSettings.octaves);
        NoiseShader.SetFloat("_GroundPercent", generatorSettings.groundPercent);
        NoiseShader.SetVector("_Position", pos);
        NoiseShader.SetInt("_Seed", generatorSettings.seed);


        NoiseShader.Dispatch(
            0, GridMetrics.PointsPerChunk / GridMetrics.NumThreads, GridMetrics.PointsPerChunk / GridMetrics.NumThreads, GridMetrics.PointsPerChunk / GridMetrics.NumThreads
        );

        _weightsBuffer.GetData(noiseValues);

        return noiseValues;
    }

    void CreateBuffers() {
        _weightsBuffer = new ComputeBuffer(
            GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk * GridMetrics.PointsPerChunk, sizeof(float)
        );
    }

    void ReleaseBuffers() {
        _weightsBuffer.Release();
    }
}
