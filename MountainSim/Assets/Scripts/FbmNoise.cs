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
    [SerializeField] float peristence ;

    float freqeuncy = 1;
    float amplitude = 1;
    float totalAmplitude;

    Texture2D tex;
    PerlinNoise perlinNoise;

    int gridSize = 20;
    int cellSize = 5;
    Vector2[,] gradientVectors;
    void Start(){
        perlinNoise = GetComponent<PerlinNoise>();
        if(perlinNoise == null){
            Debug.LogWarning("couldnt find the perlin noise instance");
        }
    }

    // Update is called once per frame
    void Update(){
        if(Input.GetMouseButtonDown(0)){
            perlinNoise.displayNoise(generateNoise(), tex);
        }
    }


    Color[] generateNoise(){
        freqeuncy = 1;
        amplitude = 1;
        totalAmplitude = 0;

        tex = new Texture2D(gridSize * cellSize,gridSize * cellSize);
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        gradientVectors = perlinNoise.generateGraidentVectors(gridSize);
        float[] pixelBrightness = new float[tex.width * tex.height];
        Color[] pixelColors = new Color[tex.width * tex.height];
        for (int octave = 0; octave < numOctaves; octave++){
            totalAmplitude += amplitude;

            for(int i = 0; i < tex.height; i++){
                for(int j = 0; j < tex.width; j++){
                    float adjustedI = i * freqeuncy;
                    float adjustedJ = j * freqeuncy;
                    float brightness = perlinNoise.getPerlinValue(adjustedI, adjustedJ, gradientVectors, cellSize, gridSize) * amplitude;
                    pixelBrightness[i * tex.width + j] += brightness;
                }
            }
            freqeuncy *= lacunarity;
            amplitude *= peristence;
        }

        for(int k = 0; k < pixelBrightness.Length; k++){
            float currBrightness = pixelBrightness[k] / totalAmplitude;
            float normalBrightness = (currBrightness + 1) / 2;
            pixelColors[k] = new Color(normalBrightness, normalBrightness, normalBrightness);
        }
        return pixelColors;
    }


    



}
