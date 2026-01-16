using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;

public class FbmNoise : MonoBehaviour
{
    // number of octaves to generate
    [Header("fBm parameters")]

    [SerializeField] Vector2 offset;


    NoiseRenderer noiseRenderer;

    [SerializeField] float freqeuncy = 1;
    [SerializeField] float amplitude = 1;
    [SerializeField] Parameters parameters;



    PerlinNoise perlinNoise;
    
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
        float totalAmplitude = 0;
        int width = gridSize * cellSize;
        Vector2[,] gradientVectors = noiseRenderer.generateGraidentVectors(gridSize);
        float[] pixelBrightness = new float[width * width];
        Color[] pixelColors = new Color[width * width];
        for (int octave = 0; octave < parameters.OctaveCount; octave++){
            totalAmplitude += amplitude;
            for(int i = 0; i < width; i++){
                for(int j = 0; j < width; j++){
                    float x = ((float)j / width * freqeuncy) + (offset.x * freqeuncy);
                    float y = ((float)i / width * freqeuncy) + (offset.y * freqeuncy);
                    float val;
                    float perlinVal = perlinNoise.getPerlinValue(x , y, gradientVectors, gridSize, true);
                    if(ridge){
                        val = 1 - math.abs(perlinVal);
                    }
                    else{
                        val = perlinVal;
                    }
                    float normalized = (val + 1f) / 2f;
                    float brightness = normalized * amplitude;
                    pixelBrightness[i * width + j] += brightness;
                }
            }
            freqeuncy *= parameters.Lacunarity;
            amplitude *= parameters.Persistence;
        }
        for(int k = 0; k < pixelBrightness.Length; k++){
            float brightness = pixelBrightness[k] / totalAmplitude;
            if(ridge) brightness = (float) math.pow(brightness, parameters.RFactor);
            pixelColors[k] = new Color(brightness, brightness, brightness);
        }
        return pixelColors;
    }
    public Color[] generateFBMNoiseJobs(int gridSize, int cellSize, bool ridge){
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        float randX = UnityEngine.Random.value;
        float randY = UnityEngine.Random.value;
        offset = new Vector2(randX,randY);
        int width = gridSize * cellSize;
        float[] pixelBrightness = new float[width * width];
        float maxPossibleAmplitude = 0;
        float currentAmp = 1;
        for (int i = 0; i < parameters.OctaveCount; i++) {
            maxPossibleAmplitude += currentAmp;
            currentAmp *= parameters.Persistence;
        }
        Color[] pixelColors = new Color[width * width];

        Vector2[] gradientVectors = noiseRenderer.generateGraidentVectors1D(gridSize);

        NativeArray<Vector2> gradNative = new NativeArray<Vector2>(gradientVectors,Allocator.Persistent);
        NativeArray<float> pixelBrightNative = new NativeArray<float>(pixelBrightness, Allocator.Persistent);

        fbmJob job = new fbmJob {
            gridSize = gridSize,
            cellSize = cellSize,
            octaveCount = parameters.OctaveCount,
            ridge = ridge,
            freqeuncy = 1,
            amplitude = 1,
            lacunarity = parameters.Lacunarity,
            persistence = parameters.Persistence,
            offset = offset,
            pixelBrightness = pixelBrightNative,
            gradientVectors = gradNative
        };

        JobHandle jobHandle = job.Schedule(width * width, 32);

        jobHandle.Complete();

        for(int k = 0; k < pixelBrightNative.Length; k++){
            float brightness = pixelBrightNative[k] / maxPossibleAmplitude;
            if(ridge) brightness = (float) math.pow(brightness, parameters.RFactor);
            pixelColors[k] = new Color(brightness, brightness, brightness);
        }

        pixelBrightNative.Dispose();
        gradNative.Dispose();
        stopwatch.Stop();
        Debug.Log("took "+ stopwatch.ElapsedMilliseconds + " ms");
        return pixelColors;
    }

}


[BurstCompile]
struct fbmJob : IJobParallelFor {
    public int gridSize;
    public int cellSize;
    public int octaveCount;
    public bool ridge;
    public float freqeuncy;
    public float amplitude;
    public float lacunarity;
    public float persistence;
    public Vector2 offset;
    [WriteOnly] public NativeArray<float> pixelBrightness;
    [ReadOnly] public NativeArray<Vector2> gradientVectors;
    public void Execute(int index) {

        int width = gridSize * cellSize;
        int row = index / width;
        int col = index % width;

        float currentFreq = freqeuncy;
        float currentAmp = amplitude;

        float totalPixelBrightness = 0f;
        for(int octave = 0; octave < octaveCount; octave++){
            float x = ((float)col / width * currentFreq) + (offset.x * currentFreq);
            float y = ((float)row / width * currentFreq) + (offset.y * currentFreq);
            float val;
            float perlinVal = PerlinNoise.getPerlinValue(x , y, gradientVectors, gridSize, true);
            if(ridge){
                val = 1 - math.abs(perlinVal);
            }
            else{
                val = perlinVal;
            }
            float normalized = (val + 1f) / 2f;
            float brightness = normalized * currentAmp;
            totalPixelBrightness += brightness;

            currentFreq *= lacunarity;
            currentAmp *= persistence;
        }
        pixelBrightness[index] = totalPixelBrightness;

    }
}
