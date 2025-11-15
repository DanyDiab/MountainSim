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
    // Buttons
    [Header("TextureList")]
    [SerializeField] Texture[] textureList;
    [SerializeField] GameObject elementPicker;
    [SerializeField] GameObject layerPicker;


    [Header("Panels")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject colorPickingPanel;
    [SerializeField] GameObject layerPickerSubMenu;

    GridLayoutGroup elementPickerGrid;
    GridLayoutGroup layerPickerGrid;


    RectTransform layerRectTransform;
    RectTransform elementRectTransform;
    int currentNumLayers;



    Vector2 desiredCellSize;
    Vector2 desiredSpacing;
    int numColumns;
    int totalPossibleChoices;

    void Start(){
        LoadParameters();
        elementRectTransform = elementPicker.GetComponent<RectTransform>();
        elementPickerGrid = colorPickingPanel.GetComponent<GridLayoutGroup>();
        layerRectTransform = layerPicker.GetComponent<RectTransform>();
        layerPickerGrid = layerPickerSubMenu.GetComponent<GridLayoutGroup>();
        desiredCellSize = new Vector2(100, 100);
        desiredSpacing = new Vector2(10, 10);
        numColumns = 5;
        totalPossibleChoices = parameters.NumPossibleElements;
    }

    // Update is called once per frame
    void Update(){
        if(parameters.NumLayers == numberLayers.value) return;
        SaveParameters();
        loadDyanmicGrid(layerPickerGrid, (int)numberLayers.value, layerPicker, layerPickerSubMenu.transform, loadPickMenu);
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
        numberLayers.value = parameters.NumLayers;

    }

    public void loadDyanmicGrid(GridLayoutGroup grid, int total, GameObject prefab, Transform parent, Action<int> onElementClickAction) {
        if (grid == null) {
            Debug.LogError("Grid Layout Group is not assigned!");
            return;
        }

        grid.cellSize = desiredCellSize;
        grid.spacing = desiredSpacing;

        foreach (Transform child in parent) {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < total; i++) {
            GameObject newElement = Instantiate(prefab, parent);
            newElement.transform.localScale = Vector3.one;
            Button button = newElement.GetComponentInChildren<Button>();
            if (button != null) {
                button.onClick.AddListener(() => onElementClickAction(i));
            }
        }
    }

    public void SaveParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }
        parameters.NumLayers = (int) numberLayers.value;

    }

    public void loadPickMenu(int elementIndex) {
        mainPanel.SetActive(false);
        colorPickingPanel.SetActive(true);
        loadDyanmicGrid(elementPickerGrid, totalPossibleChoices,elementPicker, colorPickingPanel.transform, loadMainMenu);
    }

    public void loadMainMenu(int elementIndex) {
        mainPanel.SetActive(true);
        colorPickingPanel.SetActive(false);
    }
}
