using UnityEngine;

[CreateAssetMenu(menuName = "Asset/Parameters")]
public class Parameters : ScriptableObject
{
    [Header("Generation Parameters")]
    [SerializeField] private NoiseAlgorithms noiseAlgorithms;
    [SerializeField] private int octaveCount;
    [SerializeField] private float lacunarity;
    [SerializeField] private float persistence;
    [SerializeField] private int currentSeed;
    [SerializeField] private float heightExageration;
    [SerializeField] private int rFactor;
    [SerializeField] private int gridSize;
    [SerializeField] private int cellSize;

    [Space(10)]
    [Header("Terrain Coloring Parameters")]
    [SerializeField] private TerrainColoringParams terrainColoring;
    [ColorUsage(true,true)]
    [SerializeField] private Color[] colors;
    [SerializeField] private Texture2D[] textures;


// getters / setters 
    public NoiseAlgorithms NoiseAlgorithms
    {
        get => noiseAlgorithms;
        set => noiseAlgorithms = value;
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

    public Texture2D[] Textures
    {
        get => textures;
        set => textures = value;
    }
}
