using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class TooltipManager : MonoBehaviour
{
    static GameObject tooltip; 
    static TextMeshProUGUI ttText;
    [SerializeField] Vector2 offset;
    

    void Awake(){
        tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        ttText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        show(false,null);
    }

    void Update(){
        if(!tooltip.activeSelf) return;
        moveToolTipToMouse();
    }

    void moveToolTipToMouse(){
        Vector2 mousePos = Input.mousePosition;
        Vector2 finalPos = mousePos + offset;
        tooltip.transform.position = finalPos;
    }
    public static void show(bool show, string text){
        ttText.text = text;
        tooltip.SetActive(show);
    }

}
