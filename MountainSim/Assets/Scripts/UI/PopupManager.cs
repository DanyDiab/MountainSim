using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour {
    public static PopupManager instance;

    [SerializeField] GameObject panel;
    [SerializeField] TextMeshProUGUI headerObj;
    [SerializeField] TextMeshProUGUI descObj;
    [SerializeField] Button closeButton;
    [SerializeField] Button closeOffFocus;

    void Awake() {
        instance = this;
    }

    void Start() {
        if (closeButton != null) {
            closeButton.onClick.AddListener(ClosePanel);
        }

        if (closeOffFocus != null) {
            RectTransform rt = closeOffFocus.GetComponent<RectTransform>();
            if (rt != null) {
                rt.sizeDelta = new Vector2(Screen.width, Screen.height);
            }
            closeOffFocus.onClick.AddListener(ClosePanel);
        }
    }

    public static void Show(string header, string description) {
        if (instance == null) return;
        
        if (instance.headerObj != null) instance.headerObj.text = header;
        if (instance.descObj != null) instance.descObj.text = description;
        
        if (instance.panel != null) instance.panel.SetActive(true);
    }

    public void ClosePanel() {
        if (panel != null) panel.SetActive(false);
    }
}
