using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyboxSwapper : MonoBehaviour {
    [Header("UI Reference")]
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] Settings settings;

    [Header("Data")]
    [SerializeField] List<Material> skyboxMaterials;

    void Start() {
        if (dropdown.options.Count != skyboxMaterials.Count) {
            Debug.LogWarning("Warning: Dropdown options count does not match Skybox Materials count.");
        }

        if (settings != null) {
            int index = Mathf.Clamp(settings.SkyboxIndex, 0, skyboxMaterials.Count - 1);
            dropdown.SetValueWithoutNotify(index);
            SwapToSelected(index);
        }

        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void OnDropdownChanged(int index){
        SwapToSelected(index);
        if (settings != null){
            settings.SkyboxIndex = index;
            MenuUtil.Save(settings.GetSaveData(), "settings");
        }
    }

    void SwapToSelected(int index) {
        if (index < 0 || index >= skyboxMaterials.Count) {
            return;
        }

        RenderSettings.skybox = skyboxMaterials[index];

    }
}