using UnityEngine;
using System.Collections;

[System.Serializable]
[CreateAssetMenu(menuName = "Asset/Presets")]

public class Presets : ScriptableObject{
    [SerializeField] ArrayList presets = new ArrayList();


    public ArrayList Preset{
        get => presets;
        set => presets = value;
    }
}
