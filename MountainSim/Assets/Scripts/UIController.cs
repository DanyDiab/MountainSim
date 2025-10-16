using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    UIElemMove elemMove;
    [SerializeField]RectTransform lockTransform;
    [SerializeField] Image lockImage;

    
    // Start is called before the first frame update
    void Start()
    {
        CameraController.OnLock += checkLock;

        elemMove = GetComponent<UIElemMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void checkLock(bool locked){
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


}
