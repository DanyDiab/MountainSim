using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PresetItemView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    [SerializeField] Button loadBtn;
    [SerializeField] Button deleteBtn;
    [SerializeField] GameObject deleteView;


    public void init(int idx, Action<int> onLoad, Action<int> onDelete){
        loadBtn.onClick.AddListener(() => onLoad(idx));
        deleteBtn.onClick.AddListener(() => onDelete(idx));

        deleteView.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data){
        deleteView.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData data){
        deleteView.SetActive(false);
    }
}
