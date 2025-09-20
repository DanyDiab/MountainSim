using System;
using Unity.Mathematics;
using UnityEngine;

public class FbmNoise : MonoBehaviour
{
    // number of octaves to generate
    [Header("fBm parameters")]
    [SerializeField] int numOctaves;
    // noise freqeuncy change per octave (increasing) 
    [SerializeField] float lacunarity;

    // amplitude change between octaves (decreasing)
    [SerializeField] float peristence;
    [SerializeField] float rFactor;


    NoiseRenderer noiseRenderer;

    [SerializeField] float freqeuncy = 1;
    [SerializeField] float amplitude = 1;
    float totalAmplitude;

    PerlinNoise perlinNoise;
    


    Vector2[,] gradientVectors;
    void Start(){
        noiseRenderer = GetComponent<NoiseRenderer>();
        perlinNoise = GetComponent<PerlinNoise>();
        if(perlinNoise == null){
            Debug.LogWarning("couldnt find the perlin noise instance");
        }
    }
    
    public Color[] generateFBMNoise(int gridSize, int cellSize, bool ridge){
        freqeuncy = 1;
        amplitude = 1;
        totalAmplitude = 0;
        int width = gridSize * cellSize;
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        gradientVectors = noiseRenderer.generateGraidentVectors(gridSize);
        float[] pixelBrightness = new float[width * width];
        Color[] pixelColors = new Color[width * width];
        for (int octave = 0; octave < numOctaves; octave++){
            totalAmplitude += amplitude;
            Debug.Log(amplitude);
            for(int i = 0; i < width; i++){
                for(int j = 0; j < width; j++){
                    float x = (float)j / width * freqeuncy + (10 * octave);
                    float y = (float)i / width * freqeuncy + (10 * octave);
                    float brightness;
                    if(ridge){
                        brightness = 1- math.abs(perlinNoise.getPerlinValue(x , y, gradientVectors, cellSize, gridSize)) * amplitude;
                    }
                    else{
                        brightness = perlinNoise.getPerlinValue(x , y, gradientVectors, cellSize, gridSize) * amplitude;
                    }
                    pixelBrightness[i * width + j] += brightness;
                }
            }
            freqeuncy *= lacunarity;
            amplitude *= peristence;
        }
        for(int k = 0; k < pixelBrightness.Length; k++){
            float currBrightness = pixelBrightness[k] / totalAmplitude;
            float finalBrightness  = (float) Math.Pow(currBrightness, rFactor);
            pixelColors[k] = new Color(finalBrightness, finalBrightness, finalBrightness);
        }
        return pixelColors;
    }

}
