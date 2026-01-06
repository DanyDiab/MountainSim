using UnityEngine;

public enum NoiseAlgorithms{
    Ridge,
    fBm,
    Perlin
}

public enum TerrainColoringParams{
    TextureGrad,
    Texture,
    Color,
    
}

public class NoiseRenderer : MonoBehaviour{
    [SerializeField] Parameters parameters;
    public Renderer targetRenderer;
    PerlinNoise perlin;
    FbmNoise fBm;
    bool generateBtnPressed;


    TerrainColoring terrainColoring;
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
        if(Input.GetKeyDown(KeyCode.G) && !inMenu || generateBtnPressed){
            updateNoise = true;
            generateBtnPressed = false;
        }
        if(Input.GetKeyDown(KeyCode.N)){
            generateSeed();
        }
        if(!updateNoise){
            return;
        }
        switch(parameters.CurrAlgorithm){
            case NoiseAlgorithms.Perlin:
                displayNoise(perlin.generatePerlinNoise(parameters.GridSize,parameters.CellSize));
                break;
            case NoiseAlgorithms.fBm:
                displayNoise(fBm.generateFBMNoise(parameters.GridSize,parameters.CellSize, false));
                break;
            case NoiseAlgorithms.Ridge:
                displayNoise(fBm.generateFBMNoise(parameters.GridSize,parameters.CellSize, true));
                break;
        }
    }

    public void generateTerrain()
    {
        generateBtnPressed = true;
    }

    public void generateSeed(){
        parameters.CurrentSeed = Random.Range(-2147483648, 2147483647);
    }
    public void displayNoise(Color[] pixels){
        BrightnessToMesh(pixels);
        switch(parameters.TerrainColoring){
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
        Random.InitState((int)parameters.CurrentSeed);
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
        int size = parameters.GridSize * parameters.CellSize;
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
                float vertHeight = Mathf.Clamp(vertColor.r * parameters.HeightExageration, -100000, 100000);
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

