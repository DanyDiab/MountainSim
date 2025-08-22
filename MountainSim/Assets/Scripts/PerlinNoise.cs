using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    Vector2[,] gradientVectors;

    public Color[] generatePerlinNoise(int gridSize, int cellSize){
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        gradientVectors = generateGraidentVectors(gridSize);
        Color[] pixels = getPerlinValues(gridSize,cellSize, gradientVectors);
        return pixels;
    }


    public Vector2[,] generateGraidentVectors(int gridSize){
        Vector2[,] grads = new Vector2[gridSize + 1, gridSize + 1];
        for(int i = 0; i < gridSize + 1; i++){
            for(int j = 0; j < gridSize + 1; j++){
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector2 gradientVector = new Vector2(randDirX,randDirY).normalized;
                grads[i,j] = gradientVector;
            }   
        }
        return grads;
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
        int gridX = Mathf.FloorToInt(sampleX) % sizeOfGrid;
        int gridY = Mathf.FloorToInt(sampleY) % sizeOfGrid;

        float LocalX = sampleX - gridX;
        float LocalY = sampleY - gridY;

        // calculate the apropraite grid intersections
        Vector2 tl = new Vector2(LocalX, LocalY);
        Vector2 tr = new Vector2(LocalX - 1 ,LocalY);
        Vector2 bl = new Vector2(LocalX ,LocalY - 1);
        Vector2 br = new Vector2(LocalX - 1 ,LocalY - 1);
        // find the apropriate gradientVectors
        Vector2 tlGrad = grads[gridX,gridY];
        Vector2 trGrad = grads[gridX + 1,gridY];
        Vector2 blGrad = grads[gridX,gridY + 1];
        Vector2 brGrad = grads[gridX + 1, gridY + 1];
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

        float brightness = valueToBrightness(final);
        return brightness;
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
