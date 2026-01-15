using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class TerrainColorMenu : MonoBehaviour
{
    [SerializeField] Parameters parameters;

    [Header("Sliders")]
    [SerializeField] Slider numberLayers;
    [SerializeField] Slider uvScaleSlider;

    [Header("other Elements")]
    [SerializeField] TMP_Dropdown colorAlgo;

    [SerializeField] TMP_InputField layerInput;
    [SerializeField] TMP_InputField UVInput;

    [SerializeField] GameObject bugTooltip;
    Texture2D[] textureList;
    Texture2D[] currTextures;


    // Buttons
    [Header("Picker Elements")]
    [SerializeField] GameObject elementPicker;
    [SerializeField] GameObject layerPicker;
    GridLayoutGroup texturePickerGrid;
    GridLayoutGroup layerPickerGrid;
    int currentIndexEditing;


    [Header("Panels")]
    [SerializeField] GameObject LayerPanel;
    [SerializeField] GameObject LayerPickingGridParent;
    [SerializeField] GameObject TexturePickerSubMenu;
    [SerializeField] GameObject otherPanel;
    private static string fileName = "parameters";
    List<GameObject> subPanels;


    // other
    int totalPossibleChoices;
    bool updateUI;

    void Awake(){
        if(parameters != null) {
            ParametersSaveData data = parameters.GetSaveData();
            MenuUtil.Load(data, fileName);
            parameters.LoadFromSaveData(data);
        }
        texturePickerGrid = TexturePickerSubMenu.GetComponentInChildren<GridLayoutGroup>();
        layerPickerGrid = LayerPickingGridParent.GetComponent<GridLayoutGroup>();
        totalPossibleChoices = parameters.textureLibrary.AllTextures.Length;
        subPanels = new List<GameObject> { LayerPanel, otherPanel, TexturePickerSubMenu };
    }

    void OnEnable(){
        LoadParameters();
        MenuUtil.ShowPanel(LayerPanel, subPanels);
        MenuUtil.loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, buttonToPickMenu, currTextures);
        MenuUtil.LinkSliderAndInputField(numberLayers, layerInput, true, "F0", showKnownBugToolTip);
        MenuUtil.LinkSliderAndInputField(uvScaleSlider, UVInput, false, "F2");
    }

    void Update(){
        if(parameters.Layers == numberLayers.value && !updateUI) return;
        SaveParameters();
        MenuUtil.loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, buttonToPickMenu, currTextures);
        updateUI = false;
    }

    void buttonToPickMenu(GameObject prefabCreated, int index){
        Button button = prefabCreated.GetComponentInChildren<Button>();
        if(button == null) return;

        button.onClick.AddListener(() => loadPickMenu(index));
    }

    void buttonToMainMenu(GameObject prefabCreated, int index){
        Button button = prefabCreated.GetComponentInChildren<Button>();
        if(button == null) return;
        button.onClick.AddListener(() => loadMainMenu(index));
    }


    public void LoadParameters(){
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }
        colorAlgo.value = (int)parameters.TerrainColoring;
        numberLayers.value = parameters.Layers;
        currTextures = parameters.CurrTextures;
        textureList = parameters.AllTextures;
        uvScaleSlider.value = parameters.UVScale;
    }

    public void SaveParameters(){
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }

        int newLayerCount = (int)numberLayers.value;

        if (currTextures == null || currTextures.Length != newLayerCount){
            Texture2D[] newTextures = new Texture2D[newLayerCount];
            if (currTextures != null){
                int itemsToCopy = Mathf.Min(newLayerCount, currTextures.Length);
                for (int i = 0; i < itemsToCopy; i++)
                {
                    newTextures[i] = currTextures[i];
                }
            }
            
            currTextures = newTextures;
        }
        parameters.UVScale = uvScaleSlider.value;
        parameters.TerrainColoring = (TerrainColoringParams)colorAlgo.value;
        parameters.Layers = newLayerCount;
        parameters.CurrTextures = currTextures;
        MenuUtil.Save(parameters.GetSaveData(), fileName);
    }

    void showKnownBugToolTip(){
        bool showBugTooltip = numberLayers.value > 2 && (TerrainColoringParams)colorAlgo.value == TerrainColoringParams.TextureGrad;
        bugTooltip.SetActive(showBugTooltip);
    }

    void loadPickMenu(int elementIndex) {
        currentIndexEditing = elementIndex;
        MenuUtil.ShowPanel(TexturePickerSubMenu, subPanels);
        MenuUtil.loadDyanmicGrid(texturePickerGrid, totalPossibleChoices,elementPicker, buttonToMainMenu, textureList);
    }

    void loadMainMenu(int elementIndex) {
        parameters.CurrTextures[currentIndexEditing] = textureList[elementIndex];
        updateUI = true;
        MenuUtil.ShowPanel(LayerPanel, subPanels);
    }

    public void loadLayerPanel() {
        MenuUtil.ShowPanel(LayerPanel, subPanels);
    }

    public void loadOtherPanel() {
        MenuUtil.ShowPanel(otherPanel, subPanels);
    }


}
