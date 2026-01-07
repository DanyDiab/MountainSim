using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkyboxSwapper : MonoBehaviour {
    [Header("UI Reference")]
    [SerializeField] TMP_Dropdown dropdown;

    [Header("Data")]
    [SerializeField] List<Material> skyboxMaterials;

    void Start() {
        if (dropdown.options.Count != skyboxMaterials.Count) {
            Debug.LogWarning("Warning: Dropdown options count does not match Skybox Materials count.");
        }

        dropdown.onValueChanged.AddListener(SwapToSelected);
    }

    void SwapToSelected(int index) {
        if (index < 0 || index >= skyboxMaterials.Count) {
            return;
        }

        RenderSettings.skybox = skyboxMaterials[index];

    }
}