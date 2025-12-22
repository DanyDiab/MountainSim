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
    bool seedDirtyFromUI;

    private static string fileName = "parameters";

    void Start()
    {
        complexityTresh = 2000000;
        SaveManger.Load(parameters, fileName);
        subPanels = new List<GameObject>
        {
            sizePanel,
            HeightPanel,
            FeaturePanel,
            GeneralPanel
        };
        addOnClickListeners();
        ShowSubPanel(sizePanel);
        UpdateComplexityWarning();
    }

    void addOnClickListeners() {
        seedInputField.onValueChanged.AddListener(_ => seedDirtyFromUI = true);

        // Link sliders to input fields
        octaveCountSlider.onValueChanged.AddListener(value => { octaveCountInputField.SetTextWithoutNotify(((int)value).ToString()); UpdateComplexityWarning(); });
        lacunaritySlider.onValueChanged.AddListener(value => lacunarityInputField.SetTextWithoutNotify(value.ToString("F2")));
        persistenceSlider.onValueChanged.AddListener(value => persistenceInputField.SetTextWithoutNotify(value.ToString("F2")));
        heightExagerationSlider.onValueChanged.AddListener(value => heightExagerationInputField.SetTextWithoutNotify(value.ToString("F2")));
        rFactorSlider.onValueChanged.AddListener(value => rFactorInputField.SetTextWithoutNotify(((int)value).ToString()));
        gridSizeSlider.onValueChanged.AddListener(value => { gridSizeInputField.SetTextWithoutNotify(((int)value).ToString()); UpdateComplexityWarning(); });
        cellSizeSlider.onValueChanged.AddListener(value => { cellSizeInputField.SetTextWithoutNotify(((int)value).ToString()); UpdateComplexityWarning(); });

        // Link input fields to sliders
        octaveCountInputField.onEndEdit.AddListener(text => { UpdateSliderFromInput(octaveCountSlider, text); UpdateComplexityWarning(); });
        lacunarityInputField.onEndEdit.AddListener(text => UpdateSliderFromInput(lacunaritySlider, text));
        persistenceInputField.onEndEdit.AddListener(text => UpdateSliderFromInput(persistenceSlider, text));
        heightExagerationInputField.onEndEdit.AddListener(text => UpdateSliderFromInput(heightExagerationSlider, text));
        rFactorInputField.onEndEdit.AddListener(text => UpdateSliderFromInput(rFactorSlider, text));
        gridSizeInputField.onEndEdit.AddListener(text => { UpdateSliderFromInput(gridSizeSlider, text); UpdateComplexityWarning(); });
        cellSizeInputField.onEndEdit.AddListener(text => { UpdateSliderFromInput(cellSizeSlider, text); UpdateComplexityWarning(); });
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

        // Update input fields with loaded values
        octaveCountInputField.SetTextWithoutNotify(parameters.OctaveCount.ToString());
        lacunarityInputField.SetTextWithoutNotify(parameters.Lacunarity.ToString("F2"));
        persistenceInputField.SetTextWithoutNotify(parameters.Persistence.ToString("F2"));
        heightExagerationInputField.SetTextWithoutNotify(parameters.HeightExageration.ToString("F2"));
        rFactorInputField.SetTextWithoutNotify(parameters.RFactor.ToString());
        gridSizeInputField.SetTextWithoutNotify(parameters.GridSize.ToString());
        cellSizeInputField.SetTextWithoutNotify(parameters.CellSize.ToString());
        
        seedDirtyFromUI = false;
        UpdateComplexityWarning();
    }

    public void SaveParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }
        SaveManger.Save(parameters, fileName);

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

    private void UpdateSliderFromInput(Slider slider, string text)
    {
        if (float.TryParse(text, out float value))
        {
            slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
        }
    }

    private void ShowSubPanel(GameObject panelToShow){
        foreach(GameObject panel in subPanels){
            panel.SetActive(panel == panelToShow);
        }
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
    public void ShowSizePanel() => ShowSubPanel(sizePanel);
    public void ShowHeightPanel() => ShowSubPanel(HeightPanel);
    public void ShowFeaturePanel() => ShowSubPanel(FeaturePanel);
    public void ShowGeneralPanel() => ShowSubPanel(GeneralPanel);
}