using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;
using System.Collections;

public enum NoiseAlgorithms{
    Ridge,
    fBm,
    Perlin
}

public enum TerrainColoringAlgorithms{
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
    bool computingMesh;

    void Start(){
        terrainColoring = GetComponent<TerrainColoring>();
        perlin = GetComponent<PerlinNoise>();
        fBm = GetComponent<FbmNoise>();
        inMenu = false;
        UIController.OnPause += updateInMenu;
        FbmNoise.OnGenerated += generateMeshJobs;
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
                generateMeshJobs(perlin.generatePerlinNoise(parameters.GridSize,parameters.CellSize));
                break;
            case NoiseAlgorithms.fBm:
                fBm.generateFBMNoiseJobs(parameters.GridSize,parameters.CellSize, false);
                break;
            case NoiseAlgorithms.Ridge:
                fBm.generateFBMNoiseJobs(parameters.GridSize,parameters.CellSize, true);
                break;
        }
    }

    public void generateTerrain()
    {
        generateBtnPressed = true;
    }

    public void generateSeed(){
        parameters.CurrentSeed = UnityEngine.Random.Range(-2147483648, 2147483647);
    }
    public void displayNoise(){
        switch(parameters.TerrainColoring){
            case TerrainColoringAlgorithms.Texture:
                terrainColoring.updatePixelTex();
                break;
            case TerrainColoringAlgorithms.Color:
                terrainColoring.updatePixelColors();
                break;
            case TerrainColoringAlgorithms.TextureGrad:
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
        UnityEngine.Random.InitState((int)parameters.CurrentSeed);
        Vector2[,] grads = new Vector2[gridSize + 1, gridSize + 1];
        for(int i = 0; i < gridSize + 1; i++){
            for(int j = 0; j < gridSize + 1; j++){
                float randDirX = UnityEngine.Random.Range(-1f,1f);
                float randDirY = UnityEngine.Random.Range(-1f,1f);
                Vector2 gradientVector = new Vector2(randDirX,randDirY).normalized;
                grads[i,j] = gradientVector;
            }   
        }
        return grads;
    }

    public Vector2[] generateGraidentVectors1D(int gridSize){
        // update the random with the current seed
        UnityEngine.Random.InitState((int)parameters.CurrentSeed);
        int totalSize = gridSize * gridSize;
        Vector2[] grads = new Vector2[totalSize];
        for(int i = 0; i < totalSize; i++){
            float randDirX = UnityEngine.Random.Range(-1f,1f);
            float randDirY = UnityEngine.Random.Range(-1f,1f);
            Vector2 gradientVector = new Vector2(randDirX,randDirY).normalized;
            grads[i] = gradientVector;
        }   
        return grads;
    }

    public void generateMeshJobs(Color[] pixels) {

        StartCoroutine(GenerateMeshRoutine(pixels));
    }

    IEnumerator GenerateMeshRoutine(Color[] pixels) {
        int size = parameters.GridSize * parameters.CellSize;
        computingMesh = true;
        
        int numVerts = size * size;
        int numTris = size * size * 6;
        int numQuads = (size - 1) * (size - 1);
        int numIndices = numQuads * 6;

        NativeArray<float3> verticesNative = new NativeArray<float3>(numVerts, Allocator.Persistent);
        NativeArray<float2> uvsNative = new NativeArray<float2>(numVerts, Allocator.Persistent);
        NativeArray<int> trianglesNative = new NativeArray<int>(numIndices, Allocator.Persistent);
        NativeArray<Color> pixelColorsNative = new NativeArray<Color>(pixels, Allocator.Persistent);


        meshJob meshJob = new meshJob {
            triangles = trianglesNative,
            size = size
        };

        vertexJob vertexJob = new vertexJob {
            vertices = verticesNative,
            uvs = uvsNative,
            pixelColors = pixelColorsNative,
            HeightExageration = parameters.HeightExageration,
            size = size
        };



        JobHandle meshjobHandle = meshJob.Schedule(numQuads, 32);
        
        JobHandle vertexJobHandle = vertexJob.Schedule(numVerts,32, meshjobHandle);

        while (!vertexJobHandle.IsCompleted) {
            yield return null; 
        }
        vertexJobHandle.Complete();

        MeshFilter meshFilter = targetRenderer.GetComponent<MeshFilter>();


        Mesh mesh = new Mesh();

        if (numVerts > 65535) mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        
        mesh.SetVertices(verticesNative.Reinterpret<Vector3>());
        mesh.triangles = trianglesNative.ToArray();
        mesh.SetUVs(0,uvsNative.Reinterpret<Vector2>());
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        meshFilter.mesh = mesh;
        computingMesh = false;

        verticesNative.Dispose();
        uvsNative.Dispose();
        trianglesNative.Dispose();
        pixelColorsNative.Dispose();

        displayNoise();
    }
    void updateInMenu(bool inMenu)
    {
        this.inMenu = inMenu;
    }
}

[BurstCompile]
struct meshJob : IJobParallelFor {
    [NativeDisableParallelForRestriction]
    [WriteOnly] public NativeArray<int> triangles;
    public int size; 

    public void Execute(int index) {
        // Get the four vertices of current quad
        
        int x = index % (size - 1);
        int y = index / (size - 1);

        int bottomLeft = y * size + x;
        int bottomRight = y * size + (x + 1);
        int topLeft = (y + 1) * size + x;
        int topRight = (y + 1) * size + (x + 1);
        
        int triIndex = index * 6;

        triangles[triIndex] = bottomLeft;
        triangles[triIndex + 1] = topLeft;
        triangles[triIndex + 2] = bottomRight;

        triangles[triIndex + 3] = bottomRight;
        triangles[triIndex + 4] = topLeft;
        triangles[triIndex + 5] = topRight;
    }

}


[BurstCompile] 
struct vertexJob : IJobParallelFor {
    [WriteOnly] public NativeArray<float3> vertices;
    [WriteOnly] public NativeArray<float2> uvs;
    [ReadOnly] public NativeArray<Color> pixelColors;
    public float HeightExageration;
    public int size;

    public void Execute(int index) {
        int width = size;
        int height = size;

        int x = index % size;
        int y = index / size;

        float3 pos = new float3(x, 0, y);
        Color vertColor = pixelColors[index];
        float vertHeight = math.clamp(vertColor.r * HeightExageration, -100000, 100000);
        pos.y = vertHeight;
        vertices[index] = pos;
        uvs[index] = new float2((float)x / (width - 1), (float)y / (height - 1));
    }
}

