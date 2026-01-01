using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TextureLibrary))]
public class TextureLibEditor : Editor {
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        TextureLibrary library = (TextureLibrary)target;
        
        GUILayout.Space(20);

        if(GUILayout.Button("Populate Textures")){
            LoadAllTextures(library);
        }
    }

    public void LoadAllTextures(TextureLibrary library){
        string[] guids = AssetDatabase.FindAssets("t:Texture2D", new[] { "Assets/Textures" });
        library.AllTextures = new Texture2D[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            library.AllTextures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }
        EditorUtility.SetDirty(library);
        AssetDatabase.SaveAssets();
        Debug.Log($"Loaded {library.AllTextures.Length} textures into TextureLibrary.");
    }
}