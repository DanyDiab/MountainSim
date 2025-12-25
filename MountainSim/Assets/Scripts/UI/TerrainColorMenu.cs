using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TerrainColorMenu : MonoBehaviour
{
    [SerializeField] Parameters parameters;

    [Header("Menu UI Elements")]
    [SerializeField] Slider numberLayers;
    [SerializeField] TMP_Dropdown colorAlgo;
    [SerializeField] Slider uvScaleSlider;
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

    void Start(){
        MenuUtil.Load(parameters, fileName);
        LoadParameters();
        SaveParameters();
        texturePickerGrid = TexturePickerSubMenu.GetComponentInChildren<GridLayoutGroup>();
        layerPickerGrid = LayerPickingGridParent.GetComponent<GridLayoutGroup>();
        totalPossibleChoices = parameters.NumPossibleElements;
        subPanels = new List<GameObject> { LayerPanel, otherPanel, TexturePickerSubMenu };
        MenuUtil.ShowPanel(LayerPanel, subPanels);
        MenuUtil.loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, loadPickMenu, currTextures);
    }

    void Update(){
        if(parameters.Layers == numberLayers.value && !updateUI) return;
        SaveParameters();
        MenuUtil.loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, loadPickMenu, currTextures);
        updateUI = false;
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

public void SaveParameters()
    {
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
        MenuUtil.Save(parameters, fileName);
    }



    void loadPickMenu(int elementIndex) {
        currentIndexEditing = elementIndex;
        MenuUtil.ShowPanel(TexturePickerSubMenu, subPanels);
        MenuUtil.loadDyanmicGrid(texturePickerGrid, totalPossibleChoices,elementPicker, loadMainMenu, textureList);
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
