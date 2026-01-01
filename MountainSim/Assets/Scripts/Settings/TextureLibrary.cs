using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Asset/TextureLibrary")]

public class TextureLibrary : ScriptableObject
{
    [SerializeField] Texture2D[] allTextures;

    public Texture2D[] AllTextures
    {
        get => allTextures;
        set => allTextures = value;
    }

}
