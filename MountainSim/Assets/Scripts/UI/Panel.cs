using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour{
    [SerializeField] TextMeshProUGUI headerObj;
    [SerializeField] TextMeshProUGUI descObj;
    [SerializeField] Button closeButton;
    [SerializeField] Button closeOffFocus;
    GameObject panel;

    void Start(){
        panel = transform.gameObject;
        closeButton?.onClick.AddListener(ClosePanel);

        if (closeOffFocus == null) return;
        RectTransform rt = closeOffFocus.GetComponent<RectTransform>();
        if(rt == null) return;
        rt.sizeDelta = new Vector2(Screen.width, Screen.height);
        closeOffFocus.onClick.AddListener(ClosePanel);
    }

    public void showPanel(string header, string description){
       if (headerObj != null) headerObj.text = header;
       if (descObj != null) descObj.text = description;
       if (panel != null) panel.SetActive(true);
    }

    public void ClosePanel(){
        if (panel != null) panel.SetActive(false);
    }
}
