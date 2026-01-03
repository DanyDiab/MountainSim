using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
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
    
    
    void OnEnable(){
        paramList = presets.Preset;
        MenuUtil.ShowPanel(mainPanel);
        texs = new Texture2D[paramList.Count];
        MenuUtil.loadDyanmicGrid(grid,paramList.Count,prefab,loadPreset,texs);
    }

    public void loadPreset(int index){
        if (index >= 0 && index < paramList.Count){
            ParametersSaveData paramChosen = paramList[index];
            paramFile.LoadFromSaveData(paramChosen);
            Debug.Log("Updating, loading from " + index);
            return;
        }
        Debug.Log("Tried to update but index was not valid");
    }


    public void saveCurrent(){
        ParametersSaveData newParam = paramFile.GetSaveData();
        paramList.Add(newParam);
        presets.Preset = paramList;

        texs = new Texture2D[paramList.Count];
        MenuUtil.loadDyanmicGrid(grid,paramList.Count,prefab,loadPreset,texs);
    }
}
