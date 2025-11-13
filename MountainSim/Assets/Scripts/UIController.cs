using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum UIState
{
    Playing,
    Menu,
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
    // [SerializeField] TMP_Dropdown terrainColoringDropdown;
    
    [SerializeField] Button generateRandSeed;


    public delegate void PauseEvent(bool paused);

    public static event PauseEvent OnPause;
    bool isBtnPressed;
    bool seedDirtyFromUI;

    
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("Menu");
        currState = UIState.Playing;
        CameraController.OnLock += checkLock;
        RotateAround.OnRotate += checkRotate;

        elemMove = GetComponent<UIElemMove>();
        elemMove.setAlpha(lockImage, 0);
        elemMove.setAlpha(arrowImage,0);
        isBtnPressed = false;
        seedInputField.onValueChanged.AddListener(_ => seedDirtyFromUI = true);
        enableSizeParams();
    }

    void Update()
    {
        updateState();
        switch (currState)
        {
            case UIState.Playing:
                {
                    menu.SetActive(false);
                    break;
                }
            case UIState.Menu:
                {
                    menu.SetActive(true);
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
                Time.timeScale = 0.0f;
                updateMenuParameters();
                currState = UIState.Menu;
                OnPause?.Invoke(true);
            }
            else
            {
                Time.timeScale = 1.0f;
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
        if (seedDirtyFromUI && int.TryParse(seedInputField.text, out int seed)) {
                parameters.CurrentSeed = seed;
        }
        seedDirtyFromUI = false;
        parameters.OctaveCount = (int)octaveCountSlider.value;
        parameters.Lacunarity = lacunaritySlider.value;
        parameters.Persistence = persistenceSlider.value;
        parameters.HeightExageration = heightExagerationSlider.value;
        parameters.RFactor = (int)rFactorSlider.value;
        parameters.GridSize = (int)gridSizeSlider.value;
        parameters.CellSize = (int)cellSizeSlider.value;

        parameters.CurrAlgorithm = (NoiseAlgorithms)noiseAlgorithmDropdown.value;
        // parameters.TerrainColoring = (TerrainColoringParams)terrainColoringDropdown.value;
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
        if (EventSystem.current.currentSelectedGameObject == seedInputField.gameObject){
            EventSystem.current.SetSelectedGameObject(null);
        }
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

        if (seedInputField != null) {
            if (UnityEngine.EventSystems.EventSystem.current != null &&
                UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == seedInputField.gameObject) {
                UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            }

            seedInputField.SetTextWithoutNotify(parameters.CurrentSeed.ToString());
            seedInputField.ForceLabelUpdate();
        }

        seedDirtyFromUI = false;
        updateMenuParameters();
    }

    public void enableSizeParams(){
        turnOffAllElements();
        updateParametersFile();
        cellSizeSlider.transform.parent.gameObject.SetActive(true);
        gridSizeSlider.transform.parent.gameObject.SetActive(true);
    }

    public void enableHeightParams(){
        turnOffAllElements();
        updateParametersFile();
        heightExagerationSlider.transform.parent.gameObject.SetActive(true);
        rFactorSlider.transform.parent.gameObject.SetActive(true);
    }

    public void enableFeatureParams(){
        turnOffAllElements();
        updateParametersFile();
        octaveCountSlider.transform.parent.gameObject.SetActive(true);
        lacunaritySlider.transform.parent.gameObject.SetActive(true);
        persistenceSlider.transform.parent.gameObject.SetActive(true);
    }

    public void enableGeneralParams() {
        turnOffAllElements();
        updateParametersFile();
        seedInputField.transform.parent.gameObject.SetActive(true);
        noiseAlgorithmDropdown.transform.parent.gameObject.SetActive(true);
        generateRandSeed.gameObject.SetActive(true);

    }

    private void turnOffAllElements(){
        cellSizeSlider.transform.parent.gameObject.SetActive(false);
        gridSizeSlider.transform.parent.gameObject.SetActive(false);
        heightExagerationSlider.transform.parent.gameObject.SetActive(false);
        rFactorSlider.transform.parent.gameObject.SetActive(false);
        octaveCountSlider.transform.parent.gameObject.SetActive(false);
        lacunaritySlider.transform.parent.gameObject.SetActive(false);
        persistenceSlider.transform.parent.gameObject.SetActive(false);
        seedInputField.transform.parent.gameObject.SetActive(false);
        noiseAlgorithmDropdown.transform.parent.gameObject.SetActive(false);
        noiseAlgorithmDropdown.transform.parent.gameObject.SetActive(false);
        generateRandSeed.gameObject.SetActive(false);
    }
}
