using UnityEngine;

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



    void updatePerlinParams(int cellSize){
        float newGridSize = gridSize / cellSize;
        this.cellSize = cellSize;
    }

    public Color[] generatePerlinNoise(){
        gradientVectors = new Vector2[gridSize + 1, gridSize + 1];
        noiseTexture = new Texture2D(gridSize * cellSize,gridSize * cellSize);
        gradientVectors = generateGraidentVectors(gridSize);
        Color[] pixels = getPerlinValues();
        return pixels;
    }


    public void displayNoise(Color[] pixels, Texture2D tex){
        changeVerticeHeights(pixels, tex);
        terrainColoring.updatePixelColors();
        // renderTexture(tex);
    }

    void Update()
    {

    }


    public Vector2[,] generateGraidentVectors(int size){
        Vector2[,] grads = new Vector2[size + 1, size + 1];
        for(int i = 0; i < size + 1; i++){
            for(int j = 0; j < size + 1; j++){
                Vector2 origin = new Vector2(i * cellSize, j * cellSize);
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector2 gradientVector = new Vector2(randDirX,randDirY).normalized;
                grads[i,j] = gradientVector;
            }   
        }
        return grads;
    }

    Color[] getPerlinValues(){
        Color[] pixels = new Color[noiseTexture.width * noiseTexture.height];
        for(int i = 0; i < noiseTexture.height; i++){
            for(int j = 0; j < noiseTexture.width; j++){
                float brightness = getPerlinValue(i,j, gradientVectors, cellSize, gridSize);
                 pixels[i * noiseTexture.width + j] = new Color(brightness,brightness,brightness);
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

    void renderTexture(Texture2D tex){
        if (targetRenderer != null){
            targetRenderer.material.mainTexture = tex;
        }
    }


void changeVerticeHeights(Color[] pixels, Texture2D tex) {
    MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();
    
    int width = tex.width;
    int height = tex.height;
    
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

    public Texture2D getTex(){
        return noiseTexture;
    }

}
