using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Presets))]
public class PresetsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        Presets presets = (Presets)target;

        GUILayout.Space(10);
        
        // Add a button to the inspector
        if (GUILayout.Button("Add Empty Preset", GUILayout.Height(30)))
        {
            presets.addEmptyPreset();
        }
    }
}