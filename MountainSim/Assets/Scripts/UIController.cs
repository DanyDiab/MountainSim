using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum UIState
{
    Playing,
    Menu
}
public class UIController : MonoBehaviour
{
    GameObject menu;
    UIState currState;
    UIElemMove elemMove;

    [Header("Lock")]
    [SerializeField]RectTransform lockTransform;
    [SerializeField] Image lockImage;

    [Header("RotateArrow")]
    [SerializeField] RectTransform arrowTransform;
    [SerializeField] Image arrowImage;
    [SerializeField] Parameters parameters;

    [Header("Menu Panels")]
    [SerializeField] GameObject sizePanel;
    [SerializeField] GameObject HeightPanel;
    [SerializeField] GameObject FeaturePanel;
    [SerializeField] GameObject GeneralPanel;

    List<GameObject> panels;


    [Header("Menu UI Elements")]
    
    // Input Field
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
    [SerializeField] TMP_Dropdown terrainColoringDropdown;
    
    // Button
    [SerializeField] Button generateButton;

    public delegate void PauseEvent(bool paused);

    public static event PauseEvent OnPause;
    bool isBtnPressed;

    
    // Start is called before the first frame update
    void Start()
    {
        panels = new List<GameObject>();
        menu = GameObject.FindGameObjectWithTag("Menu");
        currState = UIState.Playing;
        CameraController.OnLock += checkLock;
        RotateAround.OnRotate += checkRotate;

        elemMove = GetComponent<UIElemMove>();
        elemMove.setAlpha(lockImage, 0);
        elemMove.setAlpha(arrowImage,0);
        isBtnPressed = false;
        panels.Add(sizePanel);
        panels.Add(HeightPanel);
        panels.Add(FeaturePanel);
        panels.Add(GeneralPanel);
        enablePanel(sizePanel);
    }

    void Update()
    {
        updateState();
        switch (currState)
        {
            case UIState.Playing:
                {
                    Time.timeScale = 1.0f;
                    menu.SetActive(false);
                    break;
                }
            case UIState.Menu:
                {
                    Time.timeScale = 0.0f;
                    menu.SetActive(true);
                    if (generateButton.IsInvoking())
                    {
                        updateParametersFile();
                        updateMenuParameters();
                        currState = UIState.Playing;
                    }
                    break;
                }
        }
    }

    void checkLock(bool locked){
        if(currState != UIState.Playing) return; 
        if (!locked)
        {
            elemMove.fadeAlpha(lockImage,"out");
            return;
        }
        elemMove.fadeAlpha(lockImage, "in");
        elemMove.moveTowards(lockTransform,"bl");
    }

    void checkRotate(bool rotating)
    {
        if(currState != UIState.Playing) return;
        if (!rotating)
        {
            elemMove.fadeAlpha(arrowImage, "out");
            return;
        }
        elemMove.fadeAlpha(arrowImage, "in");
        elemMove.moveTowards(arrowTransform,"tl");

    }


void updateState()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || isBtnPressed)
        {
            if (currState == UIState.Playing)
            {
                updateMenuParameters();
                currState = UIState.Menu;
                OnPause?.Invoke(true);
            }
            else
            {
                updateParametersFile();
                currState = UIState.Playing;
                OnPause?.Invoke(false);
                isBtnPressed = false;
            }
        }

        if (currState == UIState.Menu && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            updateParametersFile();
            currState = UIState.Playing;
            OnPause?.Invoke(false);
            isBtnPressed = false;
        }
    }

    public void btnPressed(){
        isBtnPressed = true;
    }

    // push changes to the file
    void updateParametersFile(){
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }
        if (int.TryParse(seedInputField.text, out int seed))
        {
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
        parameters.TerrainColoring = (TerrainColoringParams)terrainColoringDropdown.value;
    }



// update menu refrence
    void updateMenuParameters()
    {
        if (parameters == null)
        {
            Debug.LogError("Parameters asset is not assigned in the Inspector!");
            return;
        }
        // Input Field
        seedInputField.text = parameters.CurrentSeed.ToString();

        // Sliders
        octaveCountSlider.value = parameters.OctaveCount;
        lacunaritySlider.value = parameters.Lacunarity;
        persistenceSlider.value = parameters.Persistence;
        heightExagerationSlider.value = parameters.HeightExageration;
        rFactorSlider.value = parameters.RFactor;
        gridSizeSlider.value = parameters.GridSize;
        cellSizeSlider.value = parameters.CellSize;

        // Dropdowns
        noiseAlgorithmDropdown.value = (int)parameters.CurrAlgorithm;
        // terrainColoringDropdown.value = (int)parameters.TerrainColoring;
    }

    public void randomizeSeed() {
        parameters.CurrentSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue); // your current call :contentReference[oaicite:4]{index=4}
        seedInputField.SetTextWithoutNotify(parameters.CurrentSeed.ToString());
        seedInputField.ForceLabelUpdate();
        updateMenuParameters();
    }
    
    void disableAllPanels() {
        foreach(GameObject panel in panels) {
            panel.SetActive(false);
        }
    }

    public void enablePanel(GameObject panelToEnable) {
        foreach(GameObject panel in panels) {
            panel.SetActive(panel == panelToEnable);
        }
    }
}
