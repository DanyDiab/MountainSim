using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    bool showing;
    // Start is called before the first frame update
    void Start()
    {
        showing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            showing = !showing;
            Cursor.visible = showing;
        }
    }
}
