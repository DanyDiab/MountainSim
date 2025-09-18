using TreeEditor;
using UnityEngine;
using UnityEngine.UI;


public enum NoiseAlgorithms{
    Perlin,
    fBm
}

public enum TerrainColoringParams{
    Texture,
    Color
}

public class NoiseRenderer : MonoBehaviour{
    public Renderer targetRenderer;
    
    PerlinNoise perlin;
    FbmNoise fBm;

    [Header("Seed")]
    [SerializeField] float seed;
        
    [Header("Size Parameters")]
    [SerializeField] int gridSize = 20;
    [SerializeField] int cellSize = 5;
    [SerializeField]float heightExageration = 5f;
    
    [Header("Current Noise Algorithm")]
    [SerializeField] NoiseAlgorithms currentNoiseAlgorithm;

    TerrainColoring terrainColoring;
    [Header("TerrainColoring Params")]
    [SerializeField] TerrainColoringParams currTerrainParams;





    void Start(){
        terrainColoring = GetComponent<TerrainColoring>();
        perlin = GetComponent<PerlinNoise>();
        fBm = GetComponent<FbmNoise>();
        currentNoiseAlgorithm = NoiseAlgorithms.fBm;
    }
    void Update(){
        bool updateNoise = false;
        if(Input.GetMouseButtonDown(0)){
            updateNoise = true;
        }
        if(Input.GetMouseButtonDown(1)){
            Debug.Log("seed");
            generateSeed();
        }
        if(!updateNoise){
            return;
        }
        switch(currentNoiseAlgorithm){
          case NoiseAlgorithms.Perlin:
                displayNoise(perlin.generatePerlinNoise(gridSize,cellSize));
                break;
            case NoiseAlgorithms.fBm:
                displayNoise(fBm.generateFBMNoise(gridSize,cellSize));
                break;
        }
    }

    public void generateSeed(){
        seed = Random.Range(-2147483648, 2147483647);
    }
    public void displayNoise(Color[] pixels){
        BrightnessToMesh(pixels);
        switch(currTerrainParams){
            case TerrainColoringParams.Texture:
                terrainColoring.updatePixelTex();
                break;
            case TerrainColoringParams.Color:
                terrainColoring.updatePixelColors();
                break;
        }
    }

    void renderTexture(Texture2D tex){
        if (targetRenderer != null){
            targetRenderer.material.mainTexture = tex;
        }
    }
    public Vector2[,] generateGraidentVectors(int gridSize){
        // update the random with the current seed
        Random.InitState((int)seed);
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

    void BrightnessToMesh(Color[] pixels) {
        Texture2D tex = new Texture2D(gridSize * cellSize, gridSize * cellSize);
        (Vector3[] vertices, Vector2[] uvs) = changeVerticeHeights(tex, pixels);
        generateMesh(tex,uvs,vertices);
    }

    (Vector3[], Vector2[]) changeVerticeHeights(Texture2D tex, Color[] pixels){
        Vector3[] vertices = new Vector3[pixels.Length];
        Vector2[] uvs = new Vector2[pixels.Length];
        int width = tex.width;
        int height = tex.height;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int index = y * height + x;
                
                Vector3 pos = new Vector3(x, 0, y);
                Color vertColor = pixels[index];
                float vertHeight = vertColor.r * (heightExageration - 0);
                pos.y = vertHeight;
                
                vertices[index] = pos;
                
                uvs[index] = new Vector2((float)x / (width - 1), (float)y / (height - 1));
            }
        }
        return (vertices, uvs);
    }

    void generateMesh(Texture2D tex, Vector2[] uvs, Vector3[] vertices){
        MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();

        int[] triangles = generateQuads(tex);
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


    int[] generateQuads(Texture2D tex){
        int height = tex.height;
        int width = tex.width;
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
        return triangles;
    }
}

