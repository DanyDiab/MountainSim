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

    public void LoadAllTextures()
    {
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Textures" });
        allTextures = new Texture2D[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            allTextures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.Log($"Loaded {allTextures.Length} textures into TextureLibrary.");
    }
}
