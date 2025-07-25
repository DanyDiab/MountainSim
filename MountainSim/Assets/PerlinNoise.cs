using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    int cellSize;
    // x by x grid of X cells
    int gridSize; 
    Texture2D noiseTexture;
    Vector3[,] gradientVectors;
    public Renderer targetRenderer;


    void Start()
    {
        cellSize = 10;
        gridSize = 200;
        gradientVectors = new Vector3[gridSize + 1, gridSize + 1];
        noiseTexture = new Texture2D(gridSize,gridSize);

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
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector2 point = new Vector2(i,j);
                // calculate percent of cell which contains the point
                float pointNormalX = point.x % cellSize / cellSize;
                float pointNormalY = point.y % cellSize / cellSize;
                // calculate the apropraite grid intersections
                Vector2 tl = new Vector2(point.x % (gridSize /cellSize) ,point.y % (gridSize /cellSize));
                Vector2 tr = new Vector2((point.x % (gridSize /cellSize)) + 1 ,point.y % (gridSize /cellSize));
                Vector2 bl = new Vector2(point.x % (gridSize /cellSize) ,(point.y % (gridSize /cellSize)) + 1);
                Vector2 br = new Vector2((point.x % (gridSize /cellSize)) + 1 ,(point.y % (gridSize /cellSize)) + 1);
                // find the apropriate gradientVectors
                Vector2 tlGrad = gradientVectors[(int)tl.x,(int)tl.y];
                Vector2 trGrad = gradientVectors[(int)tr.x,(int)tr.y];
                Vector2 blGrad = gradientVectors[(int)bl.x,(int)bl.y];
                Vector2 brGrad = gradientVectors[(int)br.x,(int)br.y];
                // calculate the influences of gradient vectors on the point
                float tlI = Vector2Dot(tl,tlGrad);
                float trI = Vector2Dot(tr,trGrad);
                float blI = Vector2Dot(bl,blGrad);
                float brI = Vector2Dot(br,brGrad);
                // fade to smooth the values
                float u = fade(pointNormalX);
                float v = fade(pointNormalY);
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
                // noiseTexture.SetPixel
                noiseTexture.SetPixel(i,j, new Color(final,final,final));

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

    float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    float lerp(float a, float b, float t){
        return a + (b - a) * t;
    }
    
    float valueToBrightness(float value){
        float brightness = (value + 1) / 2;
        return brightness;
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