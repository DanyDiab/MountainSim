using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PerlinNoise : MonoBehaviour
{
    [Header("Perlin Noise Parameters")] 
    [SerializeField]int cellSize = 5;
    // x by x grid of X cells
    [SerializeField]int gridSize = 40; 
    Texture2D noiseTexture;
    Vector2[,] gradientVectors;
    public Renderer targetRenderer;
    [Header("Height Parameters")] 

    [SerializeField]float minHeight = -5f;
    [SerializeField]float maxHeight = 5f;

    [Header("Terrain Coloring")]
    public TerrainColoring terrainColoring;


    void Start()
    {
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        noiseTexture = new Texture2D(gridSize * cellSize,gridSize * cellSize);
        generatePerlinNoise();
    }
    void generatePerlinNoise(){
        generateGraidentVectors();
        Color[] pixels = getPerlinValues();
        renderTexture();
        changeVerticeHeights(pixels);
        terrainColoring.updatePixelColors();
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

    Color[] getPerlinValues(){
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
        return pixels;
    }

    void renderTexture(){
        if (targetRenderer != null){
            targetRenderer.material.mainTexture = noiseTexture;
        }
        else{
            Debug.LogWarning("Target Renderer not assigned. Please assign a Plane or Quad to display the noise.");
        }
    }


void changeVerticeHeights(Color[] pixels) {
    MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();
    
    int width = noiseTexture.width;
    int height = noiseTexture.height;
    
    Vector3[] vertices = new Vector3[pixels.Length];
    Vector2[] uvs = new Vector2[pixels.Length];
    
    
    for (int y = 0; y < height; y++) {
        for (int x = 0; x < width; x++) {
            int index = y * width + x;
            
            Vector3 pos = new Vector3(x, 0, y);
            
            Color pixelColor = pixels[index];
            float pixelHeight = pixelColor.r * (maxHeight - minHeight) + minHeight;
            pos.y = pixelHeight;
            
            vertices[index] = pos;
            
            uvs[index] = new Vector2((float)x / (width - 1), (float)y / (height - 1));
        }
    }
    
    int numQuads = (width - 1) * (height - 1);
    int[] triangles = new int[numQuads * 6]; 
    int triangleIndex = 0;
    
    for (int y = 0; y < height - 1; y++) {
        for (int x = 0; x < width - 1; x++) {
            // Get the four vertices of current quad
            int bottomLeft = y * width + x;
            int bottomRight = y * width + (x + 1);
            int topLeft = (y + 1) * width + x;
            int topRight = (y + 1) * width + (x + 1);
            
            triangles[triangleIndex++] = bottomLeft;
            triangles[triangleIndex++] = topLeft;
            triangles[triangleIndex++] = bottomRight;

            triangles[triangleIndex++] = bottomRight;
            triangles[triangleIndex++] = topLeft;
            triangles[triangleIndex++] = topRight;
        }
    }
    

    Mesh mesh = new Mesh();
    
    // Handle large vertex counts
    if (vertices.Length > 65535) {
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
    }
    
    mesh.vertices = vertices;
    mesh.triangles = triangles;
    mesh.uv = uvs;
    mesh.RecalculateNormals();
    mesh.RecalculateBounds();
    
    meshFilter.mesh = mesh;
    
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