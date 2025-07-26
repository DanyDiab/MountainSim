using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PerlinNoise : MonoBehaviour
{
    [Header("Perlin Noise Parameters")] 
    [SerializeField]int cellSize;
    // x by x grid of X cells
    [SerializeField]int gridSize; 
    Texture2D noiseTexture;
    Vector2[,] gradientVectors;
    public Renderer targetRenderer;
    [Header("Height Parameters")] 

    [SerializeField]float minHeight;
    [SerializeField]float maxHeight;


    void Start()
    {
        minHeight = -5;
        maxHeight = 5;
        cellSize = 5;
        gridSize = 40;
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        noiseTexture = new Texture2D(gridSize * cellSize,gridSize * cellSize);
        generatePerlinNoise();
    }
    void generatePerlinNoise(){
        generateGraidentVectors();
        getPerlinValues();
        renderTexture();
        changeVerticeHeights();
    }

    void Update()
    {
        if(cellSize * gridSize != noiseTexture.width){
            noiseTexture = new Texture2D(gridSize * cellSize, gridSize * cellSize);
            generatePerlinNoise();
        }
        if(Input.GetMouseButtonDown(0)){
            generatePerlinNoise();
        }
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

    (Color[], float, float)  getPerlinValues(){
        Color[] pixels = new Color[noiseTexture.width * noiseTexture.height];
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

                float brightness = valueToBrightness(final);
                // noiseTexture.SetPixel
                pixels[j * noiseTexture.width + i] = new Color(brightness,brightness,brightness);
                if(brightness > maxObserved){
                    maxObserved = brightness;
                }
                if(brightness < minObserved){
                    minObserved = brightness;
                }
            }
        }
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
        return (pixels, minObserved,maxObserved);
    }

    void renderTexture(){
        if (targetRenderer != null){
            targetRenderer.material.mainTexture = noiseTexture;
        }
        else{
            Debug.LogWarning("Target Renderer not assigned. Please assign a Plane or Quad to display the noise.");
        }
    }


    void changeVerticeHeights(){
        MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        Bounds bounds = mesh.bounds;
        float minX = bounds.min.x;
        float maxX = bounds.max.x;
        float minZ = bounds.min.z;
        float maxZ = bounds.max.z;
        Vector3[] vertices = mesh.vertices;
        
        for(int i = 0; i < vertices.Length; i++){
            Vector3 currVertex = vertices[i];
            float u = (currVertex.x - minX) / (maxX - minX);
            float v = (currVertex.z - minZ) / (maxZ - minZ);
            int texX = Mathf.Clamp(Mathf.FloorToInt(u * noiseTexture.width), 0, noiseTexture.width - 1);
            int texY = Mathf.Clamp(Mathf.FloorToInt(v * noiseTexture.height), 0, noiseTexture.height - 1);
            Color pixelColor = noiseTexture.GetPixel(texX, texY);
            float height = pixelColor.r * (maxHeight - minHeight) + minHeight;
            vertices[i].y = height;
        }
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
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