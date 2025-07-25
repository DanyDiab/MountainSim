using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PerlinNoise : MonoBehaviour
{
    int cellSize;
    // x by x grid of X cells
    int gridSize; 
    Texture2D noiseTexture;
    Vector2[,] gradientVectors;
    public Renderer targetRenderer;


    void Start()
    {
        cellSize = 10;
        gridSize = 20;
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        noiseTexture = new Texture2D(gridSize * cellSize,gridSize * cellSize);

        generatePerlinNoise();
    }
    void generatePerlinNoise(){
        generateGraidentVectors();
        getPerlinValues();
        
    }

    void Update()
    {
        renderTexture();
        
    }


    void generateGraidentVectors(){
        for(int i = 0; i < gridSize + 1; i++){
            for(int j = 0; j < gridSize + 1; j++){
                Vector2 origin = new Vector2(i * cellSize, j * cellSize);
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector2 gradientVector = new Vector2(randDirX,randDirY).normalized;
                gradientVectors[i,j] = gradientVector;
            }   
        }
    }

// returning the first 4 vectors (off set vectors)
// returning the last 4 vectors (the corresponding cells)

    void getPerlinValues(){
        Color[] pixels = new Color[gridSize * gridSize];
        float maxObserved = float.MinValue;
        float minObserved = float.MaxValue;
        for(int i = 0; i < noiseTexture.height; i++){
            for(int j = 0; j < noiseTexture.width; j++){
                // calculate percent of cell which contains the point
                float sampleX = (float)j / cellSize;
                float sampleY = (float)i / cellSize;
                int gridX = Mathf.FloorToInt(sampleX);
                int gridY = Mathf.FloorToInt(sampleY);


                float LocalX = sampleX - gridX;
                float LocalY = sampleY - gridY;

                Debug.Log(LocalX);
                Debug.Log(LocalY);

                // calculate the apropraite grid intersections
                Vector2 tl = new Vector2(LocalX, LocalY);
                Vector2 tr = new Vector2(LocalX - 1 ,LocalY);
                Vector2 bl = new Vector2(LocalX ,LocalY - 1);
                Vector2 br = new Vector2(LocalX - 1 ,LocalY - 1);
                // find the apropriate gradientVectors
                Vector2 tlGrad = gradientVectors[gridX,gridY];
                Vector2 trGrad = gradientVectors[gridX + 1,gridY];
                Vector2 blGrad = gradientVectors[gridX,gridY + 1];
                Vector2 brGrad = gradientVectors[gridX + 1, gridY + 1];
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
                if(final > maxObserved){
                    maxObserved = final;
                }
                if(final < minObserved){
                    minObserved = final;
                }
                float brightness = valueToBrightness(final);
                // noiseTexture.SetPixel
                noiseTexture.SetPixel(j,i, new Color(brightness,brightness,brightness));

            }
        }
        Debug.Log("max is" + maxObserved);
        Debug.Log("min is " + minObserved);
        noiseTexture.Apply();
    }

    void renderTexture(){
        if (targetRenderer != null){
            targetRenderer.material.mainTexture = noiseTexture;
        }
        else{
            Debug.LogWarning("Target Renderer not assigned. Please assign a Plane or Quad to display the noise.");
        }
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


// steps
// generate X vectors, point them in random directions
// for each point in the grid:
    // determine which cell it belongs in
    // calculate influence of each of the 4 corners to the point
    // compute dot product of offset and gradient vectors
    // fade the fractinals 
    // interpolate the dot products using the faded vault