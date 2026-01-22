using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Asset/TextureLibrary")]

public class TextureLibrary : ScriptableObject
{
    [SerializeField] Texture2D[] allTextures;
    
    private Dictionary<Texture2D, Texture2D> normalizedTextures = new Dictionary<Texture2D, Texture2D>();

    public Texture2D[] AllTextures
    {
        get => allTextures;
        set => allTextures = value;
    }

    public Dictionary<Texture2D, Texture2D> NormalizedTextures {
        get => normalizedTextures;
    }
}
