using System;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;

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
                float brightness = getPerlinValue(sampleX,sampleY, grads, gridSize);
                pixels[i * width + j] = new Color(brightness,brightness,brightness);
            }
        }
        return pixels;
    }


    public float getPerlinValue(float sampleX, float sampleY, Vector2[,] grads, int sizeOfGrid, bool useTiling = false){
        int gridX = Mathf.FloorToInt(sampleX);
        int gridY = Mathf.FloorToInt(sampleY);

        float localX = sampleX - gridX;
        float localY = sampleY - gridY;

        int gridX_0, gridY_0, gridX_1, gridY_1;

        if (useTiling){
            gridX_0 = gridX % sizeOfGrid;
            gridY_0 = gridY % sizeOfGrid;
            if (gridX_0 < 0) gridX_0 += sizeOfGrid;
            if (gridY_0 < 0) gridY_0 += sizeOfGrid;

            gridX_1 = (gridX_0 + 1) % sizeOfGrid;
            gridY_1 = (gridY_0 + 1) % sizeOfGrid;
        }
        else{
            gridX_0 = gridX;
            gridY_0 = gridY;
            gridX_1 = gridX + 1;
            gridY_1 = gridY + 1;
        }

        Vector2 tlGrad = grads[gridX_0, gridY_0];
        Vector2 trGrad = grads[gridX_1, gridY_0];
        Vector2 blGrad = grads[gridX_0, gridY_1];
        Vector2 brGrad = grads[gridX_1, gridY_1];

        Vector2 tl = new Vector2(localX, localY);
        Vector2 tr = new Vector2(localX - 1, localY);
        Vector2 bl = new Vector2(localX, localY - 1);
        Vector2 br = new Vector2(localX - 1, localY - 1);

        float tlI = Vector2.Dot(tl, tlGrad);
        float trI = Vector2.Dot(tr, trGrad);
        float blI = Vector2.Dot(bl, blGrad);
        float brI = Vector2.Dot(br, brGrad);

        float u = fade(localX);
        float v = fade(localY);

        float top = Mathf.Lerp(tlI, trI, u);
        float bot = Mathf.Lerp(blI, brI, u);
        float final = Mathf.Lerp(top, bot, v);

        return final;
    }

    public static float getPerlinValue(float sampleX, float sampleY, NativeArray<float2>grads, int sizeOfGrid, bool useTiling = false){
        int gridX = (int)math.floor(sampleX);
        int gridY = (int)math.floor(sampleY);

        float localX = sampleX - gridX;
        float localY = sampleY - gridY;

        int gridX_0, gridY_0, gridX_1, gridY_1;

        if (useTiling)
        {
            gridX_0 = gridX % sizeOfGrid;
            gridY_0 = gridY % sizeOfGrid;
            if (gridX_0 < 0) gridX_0 += sizeOfGrid;
            if (gridY_0 < 0) gridY_0 += sizeOfGrid;

            gridX_1 = (gridX_0 + 1) % sizeOfGrid;
            gridY_1 = (gridY_0 + 1) % sizeOfGrid;
        }
        else
        {
            gridX_0 = gridX;
            gridY_0 = gridY;
            gridX_1 = gridX + 1;
            gridY_1 = gridY + 1;
        }
        
        float2 tlGrad = grads[gridX_0 + (gridY_0 * sizeOfGrid)];
        float2 trGrad = grads[gridX_1 + (gridY_0 * sizeOfGrid)];
        float2 blGrad = grads[gridX_0 +(gridY_1 * sizeOfGrid)];
        float2 brGrad = grads[gridX_1 + (gridY_1 * sizeOfGrid)];

        float2 tl = new float2(localX, localY);
        float2 tr = new float2(localX - 1, localY);
        float2 bl = new float2(localX, localY - 1);
        float2 br = new float2(localX - 1, localY - 1);

        float tlI = math.dot(tl, tlGrad);
        float trI = math.dot(tr, trGrad);
        float blI = math.dot(bl, blGrad);
        float brI = math.dot(br, brGrad);

        float u = fade(localX);
        float v = fade(localY);

        float top = math.lerp(tlI, trI, u);
        float bot = math.lerp(blI, brI, u);
        float final = math.lerp(top, bot, v);

        return final;
    }

        float valueToBrightness(float value){
        return (value + 1f) / 2f;
    }

    static float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }




}
