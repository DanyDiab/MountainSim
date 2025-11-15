using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class TerrainShapeMenu : MonoBehaviour 
{
    [Header("Parameters")]
    [SerializeField] Parameters parameters;

    [Header("Sub-Panels")]
    [SerializeField] GameObject sizePanel;
    [SerializeField] GameObject HeightPanel;
    [SerializeField] GameObject FeaturePanel;
    [SerializeField] GameObject GeneralPanel;

    [Header("Menu UI Elements")]
    //input fields 
    [SerializeField] TMP_InputField seedInputField;

    // Sliders
    [SerializeField] Slider octaveCountSlider;
    [SerializeField] Slider lacunaritySlider;
    [SerializeField] Slider persistenceSlider;
    [SerializeField] Slider heightExagerationSlider;
    [SerializeField] Slider rFactorSlider;
    [SerializeField] Slider gridSizeSlider;
    [SerializeField] Slider cellSizeSlider;
  
    // Dropdowns
    [SerializeField] TMP_Dropdown noiseAlgorithmDropdown;

    // Buttons
    [SerializeField] Button generateRandSeed;

    List<GameObject> subPanels;
    bool seedDirtyFromUI;

    void Start()
    {
        subPanels = new List<GameObject>
        {
            sizePanel,
            HeightPanel,
            FeaturePanel,
            GeneralPanel
        };

        ShowSubPanel(sizePanel); 

        seedInputField.onValueChanged.AddListener(_ => seedDirtyFromUI = true);
        
    }

    /// <summary>
    /// This is called by the UIController when the menu is OPENED.
    /// It loads the data from the Parameters asset into the UI elements.
    /// </summary>
    public void LoadParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }

        seedInputField.SetTextWithoutNotify(parameters.CurrentSeed.ToString());
        octaveCountSlider.value = parameters.OctaveCount;
        lacunaritySlider.value = parameters.Lacunarity;
        persistenceSlider.value = parameters.Persistence;
        heightExagerationSlider.value = parameters.HeightExageration;
        rFactorSlider.value = parameters.RFactor;
        gridSizeSlider.value = parameters.GridSize;
        cellSizeSlider.value = parameters.CellSize;
        noiseAlgorithmDropdown.value = (int)parameters.CurrAlgorithm;
        
        seedDirtyFromUI = false; // Reset dirty flag
    }

    public void SaveParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }

        if (seedDirtyFromUI && int.TryParse(seedInputField.text, out int seed)) {
            parameters.CurrentSeed = seed;
        }
        
        parameters.OctaveCount = (int)octaveCountSlider.value;
        parameters.Lacunarity = lacunaritySlider.value;
        parameters.Persistence = persistenceSlider.value;
        parameters.HeightExageration = heightExagerationSlider.value;
        parameters.RFactor = (int)rFactorSlider.value;
        parameters.GridSize = (int)gridSizeSlider.value;
        parameters.CellSize = (int)cellSizeSlider.value;
        parameters.CurrAlgorithm = (NoiseAlgorithms)noiseAlgorithmDropdown.value;

        seedDirtyFromUI = false;
    }

    public void RandomizeSeed() 
    {
        parameters.CurrentSeed = Random.Range(int.MinValue, int.MaxValue);
        seedInputField.SetTextWithoutNotify(parameters.CurrentSeed.ToString());
        seedDirtyFromUI = false; // The parameter is already updated, no need to parse
    }

    private void ShowSubPanel(GameObject panelToShow)
    {
        foreach(GameObject panel in subPanels)
        {
            panel.SetActive(panel == panelToShow);
        }
    }

    public void ShowSizePanel() => ShowSubPanel(sizePanel);
    public void ShowHeightPanel() => ShowSubPanel(HeightPanel);
    public void ShowFeaturePanel() => ShowSubPanel(FeaturePanel);
    public void ShowGeneralPanel() => ShowSubPanel(GeneralPanel);
}