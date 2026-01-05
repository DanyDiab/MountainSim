using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PresetItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    [Header("Buttons")]
    [SerializeField] Button loadBtn;
    [SerializeField] Button deleteBtn;
    [SerializeField] Button editNameBtn;
    [SerializeField] GameObject deleteView;

    [Header("Text")]
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_InputField titleInput;

    public void init(int idx, string name, Action<int> onLoad, Action<int> onDelete, Action<int, string> onRename){
        loadBtn.onClick.AddListener(() => onLoad(idx));
        deleteBtn.onClick.AddListener(() => onDelete(idx));
        
        initEditText(idx,name,onRename);
        deleteView.SetActive(false);
    }


    void initEditText(int idx, string name, Action<int, string> onRename){
        editNameBtn.onClick.AddListener(() => {
            titleText.gameObject.SetActive(false);
            titleInput.gameObject.SetActive(true);
            titleInput.text = titleText.text;
            titleInput.Select();
            titleInput.ActivateInputField();
        });

        titleInput.onEndEdit.AddListener((newName) => {
            titleInput.gameObject.SetActive(false);
            titleText.gameObject.SetActive(true);
            titleText.text = newName;
            onRename(idx, newName);
        });

        titleText.text = name;
        titleInput.gameObject.SetActive(false);
        titleText.gameObject.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData data){
        deleteView.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData data){
        deleteView.SetActive(false);
    }
}
