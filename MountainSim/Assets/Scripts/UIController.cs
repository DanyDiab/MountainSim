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
    [SerializeField]RectTransform lockTransform;
    [SerializeField] Image lockImage;

    public delegate void PauseEvent(bool paused);

    public static event PauseEvent OnPause;

    
    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("Menu");
        currState = UIState.Playing;
        CameraController.OnLock += checkLock;

        elemMove = GetComponent<UIElemMove>();
        elemMove.setAlpha(lockImage, 0);
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
        if (locked)
        {
            elemMove.fadeAlpha(lockImage, "in");
            elemMove.moveTowards(lockTransform,"bl");
        }
        else
        {
            elemMove.fadeAlpha(lockImage,"out");
        }
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
