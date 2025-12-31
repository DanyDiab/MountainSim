using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ParametersSaveData
{
    public NoiseAlgorithms currAlgorithm;
    public int octaveCount;
    public float lacunarity;
    public float persistence;
    public int currentSeed;
    public float heightExageration;
    public int rFactor;
    public int gridSize;
    public int cellSize;
    public TerrainColoringParams terrainColoring;
    public int layers;
    public Color[] colors;
    public float uvScale;
    public string name;
    public List<int> textureIndices = new List<int>();
}

[System.Serializable]
[CreateAssetMenu(menuName = "Asset/Parameters")]
public class Parameters : ScriptableObject
{
    [SerializeField] string name;
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
    
    [SerializeField] int layers;

    [ColorUsage(true,true)]
    [SerializeField] Color[] colors;
    [SerializeField] public TextureLibrary textureLibrary;
    [SerializeField] Texture2D[] currTextures;
    [SerializeField] float uvScale;

    public ParametersSaveData GetSaveData()
    {
        ParametersSaveData data = new ParametersSaveData();
        data.currAlgorithm = currAlgorithm;
        data.octaveCount = octaveCount;
        data.lacunarity = lacunarity;
        data.persistence = persistence;
        data.currentSeed =  currentSeed;
        data.heightExageration = heightExageration;
        data.rFactor = rFactor;
        data.gridSize = gridSize;
        data.cellSize = cellSize;
        data.terrainColoring = terrainColoring;
        data.layers = layers;
        data.colors = colors;
        data.uvScale = uvScale;
        data.name = name;

        data.textureIndices = new List<int>();
        if (currTextures == null || textureLibrary == null || textureLibrary.AllTextures == null){
            Debug.LogFormat("currTexs | {0}\ntexLib | {1}\ntextureLibrary.AllTextures | {2}", currTextures, textureLibrary, textureLibrary != null ? textureLibrary.AllTextures : "null");
            return data;
        }
        Debug.Log("loading texs from library");
        foreach (Texture2D tex in currTextures){
            int i = System.Array.IndexOf(textureLibrary.AllTextures, tex);
            data.textureIndices.Add(i);
        }

        return data;
    }

    public void LoadFromSaveData(ParametersSaveData data)
    {
        currAlgorithm = data.currAlgorithm;
        octaveCount = data.octaveCount;
        lacunarity = data.lacunarity;
        persistence = data.persistence;
        currentSeed = data.currentSeed;
        heightExageration = data.heightExageration;
        rFactor = data.rFactor;
        gridSize = data.gridSize;
        cellSize = data.cellSize;
        terrainColoring = data.terrainColoring;
        layers = data.layers;
        colors = data.colors;
        uvScale = data.uvScale;
        name = data.name;

        if (data.textureIndices == null || textureLibrary == null || textureLibrary.AllTextures == null) return;
        
        List<Texture2D> loadedTextures = new List<Texture2D>();
        if(textureLibrary.AllTextures.Length == 0){
            textureLibrary.LoadAllTextures();
            Debug.Log("loading textures into the library");
        }
        foreach (int i in data.textureIndices){
            if (i >= 0 && i < textureLibrary.AllTextures.Length){
                loadedTextures.Add(textureLibrary.AllTextures[i]);
                continue;
            }
            loadedTextures.Add(null);
        }
        currTextures = loadedTextures.ToArray();
    }



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
        get => textureLibrary != null ? textureLibrary.AllTextures : null;
    }
    public Texture2D[] CurrTextures
    {
        get => currTextures;
        set => currTextures = value;
    }

    public int Layers
    {
        get => layers;
        set => layers = value;
    }
    public float UVScale
    {
        get => uvScale;
        set => uvScale = value;
    }

    public string Name{
        get => name;
        set => name = value;
    }

}
