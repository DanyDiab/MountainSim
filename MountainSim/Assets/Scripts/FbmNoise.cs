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
    [SerializeField] Vector2 offset;


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
        float randX = UnityEngine.Random.value;
        float randY = UnityEngine.Random.value;
        offset = new Vector2(randX,randY);
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
                    float x = ((float)j / width * freqeuncy) + (offset.x * freqeuncy);
                    float y = ((float)i / width * freqeuncy) + (offset.y * freqeuncy);
                    float val;
                    if(ridge){
                        val = 1 - math.abs(perlinNoise.getPerlinValue(x , y, gradientVectors, cellSize, gridSize));
                    }
                    else{
                        val = perlinNoise.getPerlinValue(x , y, gradientVectors, cellSize, gridSize);
                    }
                    float normalized = (val + 1f) / 2f;
                    float brightness = normalized * amplitude;
                    pixelBrightness[i * width + j] += brightness;
                }
            }
            freqeuncy *= lacunarity;
            amplitude *= peristence;
        }
        for(int k = 0; k < pixelBrightness.Length; k++){
            float currBrightness = pixelBrightness[k] / totalAmplitude;
            float finalBrightness  = (float) math.pow(currBrightness, rFactor);
            pixelColors[k] = new Color(finalBrightness, finalBrightness, finalBrightness);
        }
        return pixelColors;
    }

}
