using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    static TooltipManager instance;
    static GameObject tooltip; 
    static TextMeshProUGUI ttText;
    static Image image;

    [SerializeField] Color infoColor;
    [SerializeField] Color warnColor;
    [SerializeField] Vector2 offset;
    

    void Awake(){
        instance = this;
        tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        ttText = tooltip.GetComponentInChildren<TextMeshProUGUI>();
        image = tooltip.GetComponent<Image>();
        show(false,null, ToolTipType.Info);
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
    public static void show(bool show, string text, ToolTipType type){
        ttText.text = text;
        Color currColor = instance.infoColor;

        switch(type){
            case ToolTipType.Info:
                currColor = instance.infoColor;
                break;
            case ToolTipType.Warn:
                currColor = instance.warnColor;
                break;
        }

        image.color = currColor;
        tooltip.SetActive(show);
    }

}
