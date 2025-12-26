using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
[CreateAssetMenu(menuName = "Asset/Presets")]

public class Presets : ScriptableObject{
    [SerializeField] List<Parameters> presets = new List<Parameters>();


    public List<Parameters> Preset{
        get => presets;
        set => presets = value;
    }

    [ContextMenu("Add Empty Preset")]
    public void addEmptyPreset(){
        Parameters presetToAdd = CreateInstance<Parameters>();
        presetToAdd.Name = "New Preset";
        presets.Add(presetToAdd);

        #if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.AssetDatabase.AddObjectToAsset(presetToAdd, this);
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
        }
        #endif
    }
}
