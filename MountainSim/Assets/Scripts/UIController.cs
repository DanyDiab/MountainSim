using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public enum UIState
{
    Playing,
    Menu,
}
public class UIController : MonoBehaviour
{
    [Header("Menu Root")]
    [SerializeField] GameObject menuRootObject;
    
    UIState currState;
    UIElemMove elemMove;

    [Header("Playing UI (Lock/Arrow)")]
    [SerializeField]RectTransform lockTransform;
    [SerializeField] Image lockImage;
    [SerializeField] RectTransform arrowTransform;
    [SerializeField] Image arrowImage;

    [Header("Main Menu Panels")]
    [SerializeField] GameObject terrainShapeMenuPanel;
    [SerializeField] GameObject terrainColorMenuPanel;
    [SerializeField] GameObject miscMenuPanel;
    [SerializeField] GameObject presetsMenuPanel;

    List<GameObject> mainMenus;

    [Header("Menu Controllers")]
    [SerializeField] TerrainShapeMenu terrainShapeMenuController;
    [SerializeField] TerrainColorMenu terrainColorMenuController;

    public delegate void PauseEvent(bool paused);
    public static event PauseEvent OnPause;
    
    bool isGeneratePressed;

    void Start()
    {
        mainMenus = new List<GameObject>
        {
            terrainShapeMenuPanel,
            terrainColorMenuPanel,
            miscMenuPanel,
            presetsMenuPanel
        };

        currState = UIState.Playing;
        CameraController.OnLock += checkLock;
        RotateAround.OnRotate += checkRotate;

        elemMove = GetComponent<UIElemMove>();
        elemMove.setAlpha(lockImage, 0);
        elemMove.setAlpha(arrowImage, 0);
        
        isGeneratePressed = false;
        
        ShowMenu(terrainShapeMenuPanel);
    }

    void Update(){
        updateState();
        menuRootObject.SetActive(currState == UIState.Menu);
    }

    void updateState()
    {
        if (currState == UIState.Playing && Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMenu();
        }
        else if (currState == UIState.Menu)
        {
            if (Input.GetKeyDown(KeyCode.Escape)){
                CloseAndSaveMenu();
            }
            if (isGeneratePressed || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CloseAndSaveMenu();
            }
        }
    }

    void OpenMenu()
    {
        Time.timeScale = 0.0f;
        currState = UIState.Menu;
        OnPause?.Invoke(true);

        if (terrainShapeMenuController) terrainShapeMenuController.LoadParameters();
    }

    void CloseAndSaveMenu()
    {
        Time.timeScale = 1.0f;
        currState = UIState.Playing;
        OnPause?.Invoke(false);
        isGeneratePressed = false;

        if (terrainShapeMenuController) terrainShapeMenuController.SaveParameters();
        if (terrainColorMenuController) terrainColorMenuController.SaveParameters();
    }

    /// <summary>
    /// This is for your "Generate" button's OnClick() event.
    /// </summary>
    public void OnGenerateButtonPressed()
    {
        isGeneratePressed = true;
    }


    /// <summary>
    /// Hides all main menus and shows the one specified.
    /// </summary>
    private void ShowMenu(GameObject menuToEnable) 
    {
        foreach(GameObject menu in mainMenus) 
        {
            // Use '==' for GameObjects, or 'is' if checking component
            if (menu != null)
            {
                menu.SetActive(menu == menuToEnable);
            }
        }
    }

    public void ShowTerrainShapeMenu() => ShowMenu(terrainShapeMenuPanel);
    public void ShowTerrainColorMenu() => ShowMenu(terrainColorMenuPanel);
    public void ShowMiscMenu() => ShowMenu(miscMenuPanel);
    public void ShowPresetsMenu() => ShowMenu(presetsMenuPanel);

    // ... (checkLock and checkRotate methods are unchanged) ...
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
}