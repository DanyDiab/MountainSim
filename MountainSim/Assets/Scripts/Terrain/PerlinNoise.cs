using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    Vector2[,] gradientVectors;
    NoiseRenderer noiseRenderer;

    public void Start()
    {
        noiseRenderer = GetComponent<NoiseRenderer>();
    }

    public Color[] generatePerlinNoise(int gridSize, int cellSize){
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        gradientVectors = noiseRenderer.generateGraidentVectors(gridSize);
        Color[] pixels = getPerlinValues(gridSize,cellSize, gradientVectors);
        return pixels;
    }




    Color[] getPerlinValues(int gridSize, int cellSize, Vector2[,] grads){
        int width = gridSize * cellSize;
        Color[] pixels = new Color[width * width];
        for(int i = 0; i < width; i++){
            for(int j = 0; j < width; j++){
                float sampleX = (float)j / cellSize;
                float sampleY = (float)i / cellSize;
                float brightness = getPerlinValue(sampleX,sampleY, grads, cellSize, gridSize);
                pixels[i * cellSize + j] = new Color(brightness,brightness,brightness);
            }
        }
        return pixels;
    }


public float getPerlinValue(float sampleX, float sampleY, Vector2[,] grads, int sizeOfCell, int sizeOfGrid)
    {
        int gridX = Mathf.FloorToInt(sampleX);
        int gridY = Mathf.FloorToInt(sampleY);

        float localX = sampleX - gridX;
        float localY = sampleY - gridY;

        int gridX_0 = gridX % sizeOfGrid;
        int gridY_0 = gridY % sizeOfGrid;
        if (gridX_0 < 0) gridX_0 += sizeOfGrid;
        if (gridY_0 < 0) gridY_0 += sizeOfGrid;

        int gridX_1 = (gridX_0 + 1) % sizeOfGrid;
        int gridY_1 = (gridY_0 + 1) % sizeOfGrid;

        Vector2 tlGrad = grads[gridX_0, gridY_0];
        Vector2 trGrad = grads[gridX_1, gridY_0];
        Vector2 blGrad = grads[gridX_0, gridY_1];
        Vector2 brGrad = grads[gridX_1, gridY_1];

        Vector2 tl = new Vector2(localX, localY);
        Vector2 tr = new Vector2(localX - 1, localY);
        Vector2 bl = new Vector2(localX, localY - 1);
        Vector2 br = new Vector2(localX - 1, localY - 1);

        float tlI = Vector2Dot(tl, tlGrad);
        float trI = Vector2Dot(tr, trGrad);
        float blI = Vector2Dot(bl, blGrad);
        float brI = Vector2Dot(br, brGrad);

        float u = fade(localX);
        float v = fade(localY);

        float top = lerp(tlI, trI, u);
        float bot = lerp(blI, brI, u);
        float final = lerp(top, bot, v);

        return final;
    }

        float valueToBrightness(float value){
        return (value + 1f) / 2f;
    }

    float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    float lerp(float a, float b, float t){
        return a + (b - a) * t;
    }
    

    float Vector2Dot(Vector2 v1, Vector2 v2){
        return (v1.x * v2.x) + (v1.y * v2.y);
    }



}
