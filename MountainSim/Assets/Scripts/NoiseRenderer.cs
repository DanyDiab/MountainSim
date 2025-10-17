using TreeEditor;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;


public enum NoiseAlgorithms{
    Perlin,
    fBm,

    Ridge

}

public enum TerrainColoringParams{
    Texture,
    Color,
    TextureGrad
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
    

    bool inMenu;



    


    void Start(){
        terrainColoring = GetComponent<TerrainColoring>();
        perlin = GetComponent<PerlinNoise>();
        fBm = GetComponent<FbmNoise>();
        inMenu = false;
        UIController.OnPause += updateInMenu;


    }
    void Update(){
        bool updateNoise = false;
        if(Input.GetMouseButtonDown(0) && !inMenu){
            updateNoise = true;
        }
        if(Input.GetMouseButtonDown(1)){
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
                displayNoise(fBm.generateFBMNoise(gridSize,cellSize, false));
                break;
            case NoiseAlgorithms.Ridge:
                displayNoise(fBm.generateFBMNoise(gridSize,cellSize, true));
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
            case TerrainColoringParams.TextureGrad:
                terrainColoring.updateGradTex();
                break;
        }
    }


    public Texture2D brightnessToTex(Color[] colors, int size){
        Texture2D newTex = new Texture2D(size,size);
        newTex.SetPixels(colors);
        newTex.Apply();
        return newTex;
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
        int size = gridSize * cellSize;
        (Vector3[] vertices, Vector2[] uvs) = changeVerticeHeights(size, pixels);
        generateMesh(uvs,vertices, size);
    }

    (Vector3[], Vector2[]) changeVerticeHeights(int size, Color[] pixels){
        Vector3[] vertices = new Vector3[pixels.Length];
        Vector2[] uvs = new Vector2[pixels.Length];
        int width = size;
        int height = size;
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                int index = y * height + x;
                
                Vector3 pos = new Vector3(x, 0, y);
                Color vertColor = pixels[index];
                float vertHeight = Mathf.Clamp(vertColor.r * heightExageration, -100000, 100000);
                pos.y = vertHeight;
                vertices[index] = pos;
                
                uvs[index] = new Vector2((float)x / (width - 1), (float)y / (height - 1));
            }
        }
        return (vertices, uvs);
    }


    void generateMesh(Vector2[] uvs, Vector3[] vertices, int size){
        MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();

        int[] triangles = generateQuads(size);
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


// size is the size of the entire mesh
    int[] generateQuads(int size){
        int height = size;
        int width = size;
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


    void updateInMenu(bool inMenu)
    {
        this.inMenu = inMenu;
    }
}

