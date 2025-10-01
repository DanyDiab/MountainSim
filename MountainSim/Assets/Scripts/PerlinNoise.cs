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
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < cellSize; j++){
                float brightness = getPerlinValue(i,j, grads, cellSize, gridSize);
                 pixels[i * cellSize + j] = new Color(brightness,brightness,brightness);
            }
        }
        return pixels;
    }


    public float getPerlinValue(float i, float j, Vector2[,] grads, int sizeOfCell, int sizeOfGrid){
        // calculate percent of cell which contains the point
        float sampleX = (float)j / sizeOfCell;
        float sampleY = (float)i / sizeOfCell;
        int maxIndex = sizeOfGrid - 1;
        sampleX = Mathf.Min(sampleX, maxIndex);
        sampleY = Mathf.Min(sampleY, maxIndex);
        int gridX = Mathf.FloorToInt(sampleX) % sizeOfGrid;
        int gridY = Mathf.FloorToInt(sampleY) % sizeOfGrid;
        if (gridX < 0) gridX += sizeOfGrid;
        if (gridY < 0) gridY += sizeOfGrid;

        int nextX = (gridX + 1) % sizeOfGrid;
        int nextY = (gridY + 1) % sizeOfGrid;

        float LocalX = sampleX - gridX;
        float LocalY = sampleY - gridY;

        // calculate the apropraite grid intersections
        Vector2 tl = new Vector2(LocalX, LocalY);
        Vector2 tr = new Vector2(LocalX - 1 ,LocalY);
        Vector2 bl = new Vector2(LocalX ,LocalY - 1);
        Vector2 br = new Vector2(LocalX - 1 ,LocalY - 1);
        // find the apropriate gradientVectors
        Vector2 tlGrad = grads[gridX,gridY];
        Vector2 trGrad = grads[nextX,gridY];
        Vector2 blGrad = grads[gridX,nextY];
        Vector2 brGrad = grads[nextX, nextY];
        // calculate the influences of gradient vectors on the point
        float tlI = Vector2Dot(tl,tlGrad);
        float trI = Vector2Dot(tr,trGrad);
        float blI = Vector2Dot(bl,blGrad);
        float brI = Vector2Dot(br,brGrad);
        // fade to smooth the values
        float u = fade(LocalX);
        float v = fade(LocalY);
        // lerp across the top and bottom
        float top = lerp(tlI,trI,u);
        float bot = lerp(blI,brI,u);
        // lerp the top and bottom
        float final = lerp(top,bot,v);

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
