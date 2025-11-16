using UnityEngine;

[CreateAssetMenu(menuName = "Asset/Parameters")]
public class Parameters : ScriptableObject
{
    [Header("Generation Parameters")]
    [SerializeField] NoiseAlgorithms currAlgorithm;
    [SerializeField] int octaveCount;
    [SerializeField] float lacunarity;
    [SerializeField] float persistence;
    [SerializeField] int currentSeed;
    [SerializeField] float heightExageration;
    [SerializeField] int rFactor;
    [SerializeField] int gridSize;
    [SerializeField] int cellSize;

    [Space(10)]
    [Header("Terrain Coloring Parameters")]
    [SerializeField] TerrainColoringParams terrainColoring;
    
    [SerializeField] int numPossibleElements;
    [SerializeField] int layers;

    [ColorUsage(true,true)]
    [SerializeField] Color[] colors;
    [SerializeField] Texture2D[] allTextures;
    [SerializeField] Texture2D[] currTextures;



// getters / setters 
    public NoiseAlgorithms CurrAlgorithm
    {
        get => currAlgorithm;
        set => currAlgorithm = value;
    }

    public int OctaveCount
    {
        get => octaveCount;
        set => octaveCount = value;
    }

    public float Lacunarity
    {
        get => lacunarity;
        set => lacunarity = value;
    }

    public float Persistence
    {
        get => persistence;
        set => persistence = value;
    }

    public int CurrentSeed
    {
        get => currentSeed;
        set => currentSeed = value;
    }

    public float HeightExageration
    {
        get => heightExageration;
        set => heightExageration = value;
    }

    public int RFactor
    {
        get => rFactor;
        set => rFactor = value;
    }

    public int GridSize
    {
        get => gridSize;
        set => gridSize = value;
    }

    public int CellSize
    {
        get => cellSize;
        set => cellSize = value;
    }

    public TerrainColoringParams TerrainColoring
    {
        get => terrainColoring;
        set => terrainColoring = value;
    }

    public Color[] Colors
    {
        get => colors;
        set => colors = value;
    }

    public Texture2D[] AllTextures
    {
        get => allTextures;
        set => allTextures = value;
    }
    public Texture2D[] CurrTextures
    {
        get => currTextures;
        set => currTextures = value;
    }

    public int NumPossibleElements
    {
        get => numPossibleElements;
        set => numPossibleElements = value;
    }
    public int Layers
    {
        get => layers;
        set => layers = value;
    }
}
