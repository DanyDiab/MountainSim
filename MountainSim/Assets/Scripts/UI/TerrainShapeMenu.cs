using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

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
    [SerializeField] TMP_InputField octaveCountInputField;
    [SerializeField] TMP_InputField lacunarityInputField;
    [SerializeField] TMP_InputField persistenceInputField;
    [SerializeField] TMP_InputField heightExagerationInputField;
    [SerializeField] TMP_InputField rFactorInputField;
    [SerializeField] TMP_InputField gridSizeInputField;
    [SerializeField] TMP_InputField cellSizeInputField;

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

    [Header("Complexity Warning")]
    [SerializeField] List<GameObject> warnIcons;

    float complexityTresh;
    List<GameObject> subPanels;
    private static string fileName = "parameters";

    void Awake(){
        if(parameters != null) {
            ParametersSaveData data = parameters.GetSaveData();
            MenuUtil.Load(data, fileName);
            parameters.LoadFromSaveData(data);
        }
        complexityTresh = 2000000;
        subPanels = new List<GameObject>{
            sizePanel,
            HeightPanel,
            FeaturePanel,
            GeneralPanel
        };

    }


    void OnEnable(){
        LoadParameters();
        MenuUtil.ShowPanel(sizePanel, subPanels);

        addOnClickListeners();
        UpdateComplexityWarning();
    }
    void addOnClickListeners() {
        MenuUtil.LinkSliderAndInputField(octaveCountSlider, octaveCountInputField, true, "F0", UpdateComplexityWarning);
        MenuUtil.LinkSliderAndInputField(lacunaritySlider, lacunarityInputField, false, "0.##");
        MenuUtil.LinkSliderAndInputField(persistenceSlider, persistenceInputField, false, "0.##");
        MenuUtil.LinkSliderAndInputField(heightExagerationSlider, heightExagerationInputField, false, "0.##");
        MenuUtil.LinkSliderAndInputField(rFactorSlider, rFactorInputField, true, "F0");
        MenuUtil.LinkSliderAndInputField(gridSizeSlider, gridSizeInputField, true, "F0", UpdateComplexityWarning);
        MenuUtil.LinkSliderAndInputField(cellSizeSlider, cellSizeInputField, true, "F0", UpdateComplexityWarning);
    }

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

        octaveCountInputField.SetTextWithoutNotify(parameters.OctaveCount.ToString());
        lacunarityInputField.SetTextWithoutNotify(parameters.Lacunarity.ToString("0.##"));
        persistenceInputField.SetTextWithoutNotify(parameters.Persistence.ToString("0.##"));
        heightExagerationInputField.SetTextWithoutNotify(parameters.HeightExageration.ToString("0.##"));
        rFactorInputField.SetTextWithoutNotify(parameters.RFactor.ToString());
        gridSizeInputField.SetTextWithoutNotify(parameters.GridSize.ToString());
        cellSizeInputField.SetTextWithoutNotify(parameters.CellSize.ToString());
        
        UpdateComplexityWarning();
    }

    public void SaveParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }

        if (int.TryParse(seedInputField.text, out int seed)) {
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

        MenuUtil.Save(parameters.GetSaveData(), fileName);
    }

    public void RandomizeSeed() 
    {
        parameters.CurrentSeed = Random.Range(int.MinValue, int.MaxValue);
        seedInputField.SetTextWithoutNotify(parameters.CurrentSeed.ToString());
    }

    void UpdateComplexityWarning()
    {
        float complexity = getCurrentComplexity();
        bool showWarnings = complexity >= complexityTresh;

        for (int i = 0; i < warnIcons.Count; i++)
        {
            warnIcons[i].SetActive(showWarnings);
        }
    }


// returns the total work that will be done to generate the terrain
    float getCurrentComplexity(){
        float octaveCount = octaveCountSlider.value;
        float cellSize = cellSizeSlider.value;
        float gridSize = gridSizeSlider.value;
        float width = cellSize * gridSize;

        return octaveCount * (width * width);
    }
    public void ShowSizePanel() => MenuUtil.ShowPanel(sizePanel, subPanels);
    public void ShowHeightPanel() => MenuUtil.ShowPanel(HeightPanel, subPanels);
    public void ShowFeaturePanel() => MenuUtil.ShowPanel(FeaturePanel, subPanels);
    public void ShowGeneralPanel() => MenuUtil.ShowPanel(GeneralPanel, subPanels);
}