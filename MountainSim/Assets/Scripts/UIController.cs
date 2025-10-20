using UnityEngine;
using UnityEngine.UI;


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

    public delegate void PauseEvent(bool paused);

    public static event PauseEvent OnPause;

    
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
    }

    // Update is called once per frame
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(currState == UIState.Playing) currState = UIState.Menu;
            else currState = UIState.Playing;
            OnPause?.Invoke(currState != UIState.Playing);
        }
    }

    void showMenu(bool show)
    {
        
    }


    


}
