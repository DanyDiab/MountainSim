using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    bool showing;
    // Start is called before the first frame update
    void Start()
    {
        showing = true;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        updateCursor(hasFocus);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            showing = !showing;

            updateCursor(showing);
        }
    }


    void updateCursor(bool setTo)
    {
        Cursor.lockState = setTo ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = setTo;
    }
}
