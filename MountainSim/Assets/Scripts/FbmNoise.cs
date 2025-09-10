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
    
    public Color[] generateFBMNoise(int gridSize, int cellSize){
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
            for(int i = 0; i < width; i++){
                for(int j = 0; j < width; j++){
                    float x = (float)j / width * freqeuncy;
                    float y = (float)i / width * freqeuncy;
                    float brightness = perlinNoise.getPerlinValue(x , y, gradientVectors, cellSize, gridSize) * amplitude;
                    pixelBrightness[i * width + j] += brightness;
                }
            }
            freqeuncy *= lacunarity;
            amplitude *= peristence;
        }

        for(int k = 0; k < pixelBrightness.Length; k++){
            float currBrightness = pixelBrightness[k] / numOctaves;
            // float normalBrightness = (currBrightness + 1) / 2;
            pixelColors[k] = new Color(currBrightness, currBrightness, currBrightness);
        }
        Debug.Log("size" + pixelBrightness.Length);
        return pixelColors;
    }


    



}
