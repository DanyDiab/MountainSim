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

    // other
    int totalPossibleChoices;
    bool updateUI;

    void Start(){
        SaveManger.LoadParameters(parameters);
        LoadParameters();
        SaveParameters();
        texturePickerGrid = TexturePickerSubMenu.GetComponentInChildren<GridLayoutGroup>();
        layerPickerGrid = LayerPickingGridParent.GetComponent<GridLayoutGroup>();
        totalPossibleChoices = parameters.NumPossibleElements;
        loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, loadPickMenu, currTextures);
    }

    // Update is called once per frame
    void Update(){
        if(parameters.Layers == numberLayers.value && !updateUI) return;
        SaveParameters();
        loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, loadPickMenu, currTextures);
        updateUI = false;
    }


    /// <summary>
    /// This is called by the UIController when the menu is OPENED.
    /// It loads the data from the Parameters asset into the UI elements.
    /// </summary>
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

        if (currTextures == null || currTextures.Length != newLayerCount)
        {
            Texture2D[] newTextures = new Texture2D[newLayerCount];

            if (currTextures != null)
            {
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
        SaveManger.SaveParameters(parameters);
    }

    public void loadDyanmicGrid(GridLayoutGroup grid, int total, GameObject prefab, Action<int> onElementClickAction, Texture2D[] texList){
        if (grid == null) {
            Debug.LogError("Grid Layout Group is not assigned!");
            return;
        }


        foreach (Transform child in grid.transform) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < total; i++) {
            GameObject newElement = Instantiate(prefab, grid.transform);
            newElement.transform.localScale = Vector3.one;
            Button button = newElement.GetComponentInChildren<Button>();
            RawImage img = newElement.GetComponentInChildren<RawImage>();
            if (img != null && texList[i] != null) {
                img.texture = texList[i];
            }
            if (button != null) {
                int capturedIndex = i;
                button.onClick.AddListener(() => onElementClickAction(capturedIndex));
            }
        }
    }



    void loadPickMenu(int elementIndex) {
        Debug.Log("Loading pick menu for element index: " + elementIndex);
        currentIndexEditing = elementIndex;
        LayerPanel.SetActive(false);
        TexturePickerSubMenu.SetActive(true);
        loadDyanmicGrid(texturePickerGrid, totalPossibleChoices,elementPicker, loadMainMenu, textureList);
    }

    void loadMainMenu(int elementIndex) {
        Debug.Log("Picked index " + elementIndex + " for layer " + currentIndexEditing);
        parameters.CurrTextures[currentIndexEditing] = textureList[elementIndex];
        updateUI = true;
        LayerPanel.SetActive(true);
        TexturePickerSubMenu.SetActive(false);
    }

    public void loadLayerPanel() {
        LayerPanel.SetActive(true);
        otherPanel.SetActive(false);
    }

    public void loadOtherPanel() {
        LayerPanel.SetActive(false);
        otherPanel.SetActive(true);
    }


}
