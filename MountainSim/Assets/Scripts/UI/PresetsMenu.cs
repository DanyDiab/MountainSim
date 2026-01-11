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
        loadPresets();
        MenuUtil.ShowPanel(mainPanel);
        showGrid();
    }

    void showGrid(){
        texs = new Texture2D[paramList.Count];
        MenuUtil.loadDyanmicGrid(grid,paramList.Count,prefab,initPresetButton,texs);
    }

    void initPresetButton(GameObject prefabCreated, int index){
        PresetItemView piv = prefabCreated.GetComponent<PresetItemView>();
        if(piv == null) {
            PresetItemView[] presetItemViews = GetComponentsInChildren<PresetItemView>();
            piv = presetItemViews[presetItemViews.Length - 1];
        }
        
        string currentName = "Preset " + index;
        if(index >= 0 && index < paramList.Count && paramList[index] != null){
            currentName = paramList[index].name;
        }

        piv.init(index, currentName, loadPreset, deletePreset, renamePreset,saveToDisk);
    }

    void loadPreset(int index){
        if (index >= 0 && index < paramList.Count){
            ParametersSaveData paramChosen = paramList[index];
            paramFile.LoadFromSaveData(paramChosen);
        }
    }

    void deletePreset(int index){
        if (index >= 0 && index < paramList.Count){
            paramList.RemoveAt(index);
            showGrid();
            SavePresets();
        }
    }

    void renamePreset(int index, string newName){
        if (index >= 0 && index < paramList.Count){
            paramList[index].name = newName;
            presets.Preset = paramList;
            SavePresets();
        }
    }

    void saveCurrent(){
        ParametersSaveData newParam = paramFile.GetSaveData();
        newParam.name = "Preset Name";
        paramList.Add(newParam);
        presets.Preset = paramList;

        showGrid();
        SavePresets();
    }


    public void saveToDisk(int index) {
        if (index >= 0 && index < paramList.Count){
            ParametersSaveData data = paramList[index];
            JsonSOConvert.OnSaveToDiskClicked(data);
        }
    }


    public void SavePresets(){
        if (presets == null){
            Debug.LogError("Presets asset is not assigned in the Inspector!");
            return;
        }
        MenuUtil.Save(presets, fileName);
    }


    public void loadPresets(){
        if (presets == null){
            Debug.LogError("Presets asset is not assigned in the Inspector!");
            return;
        }
        MenuUtil.Load(presets,fileName);
    }


}
