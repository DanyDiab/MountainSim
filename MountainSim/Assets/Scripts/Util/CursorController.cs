using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Start(){
        UIController.OnPause += updateCursor;
    }

    void updateCursor(bool inMenu){
        Cursor.lockState = inMenu ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inMenu;
    }
}
