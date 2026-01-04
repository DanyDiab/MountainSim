using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PresetsMenu : MonoBehaviour
{
    private static string fileName = "presets";
    [SerializeField] Presets presets;
    List<ParametersSaveData> paramList;

    [Space(10)]
    [SerializeField] GameObject mainPanel;
    [SerializeField] Parameters paramFile;
    Texture2D[] texs;
    [Header("Grid Settings")]
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] GameObject prefab;
    
    void Awake(){
        paramList = presets.Preset;
    }
    void OnEnable(){
        MenuUtil.ShowPanel(mainPanel);
        showGrid();
    }

    void showGrid(){
        texs = new Texture2D[paramList.Count];
        MenuUtil.loadDyanmicGrid(grid,paramList.Count,prefab,initPresetButton,texs);
    }

    public void initPresetButton(GameObject prefabCreated, int index){
        PresetItemView piv = prefabCreated.GetComponent<PresetItemView>();
        if(piv == null) {
            PresetItemView[] presetItemViews = GetComponentsInChildren<PresetItemView>();
            piv = presetItemViews[presetItemViews.Length - 1];
        }
        
        string currentName = "Preset " + index;
        if(index >= 0 && index < paramList.Count && paramList[index] != null){
            currentName = paramList[index].name;
        }

        piv.init(index, currentName, loadPreset, deletePreset, renamePreset);
    }

    public void loadPreset(int index){
        if (index >= 0 && index < paramList.Count){
            ParametersSaveData paramChosen = paramList[index];
            paramFile.LoadFromSaveData(paramChosen);
        }
    }

    void deletePreset(int index){
        if (index >= 0 && index < paramList.Count){
            paramList.RemoveAt(index);
            showGrid();
        }
    }

    void renamePreset(int index, string newName){
        if (index >= 0 && index < paramList.Count){
            paramList[index].name = newName;
            presets.Preset = paramList;             
        }
    }

    public void saveCurrent(){
        ParametersSaveData newParam = paramFile.GetSaveData();
        newParam.name = "Preset Name";
        paramList.Add(newParam);
        presets.Preset = paramList;

        showGrid();
    }


}
