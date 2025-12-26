using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.UI;

public class PresetsMenu : MonoBehaviour
{
    private static string fileName = "presets";
    [SerializeField] Presets presets;
    List<Parameters> paramList;
    [Space(10)]
    [SerializeField] GameObject mainPanel;

    [Header("Grid Settings")]
    [SerializeField] GridLayoutGroup grid;
    [SerializeField] GameObject prefab;
    
    void Start(){
        paramList = presets.Preset;
        MenuUtil.Load(presets, fileName);
        MenuUtil.ShowPanel(mainPanel);
        Texture2D[] texs = new Texture2D[paramList.Count];
        MenuUtil.loadDyanmicGrid(grid,paramList.Count,prefab,loadPreset,texs);
    }
    // public static void loadDyanmicGrid(GridLayoutGroup grid, int total, GameObject prefab, Action<int> onElementClickAction, Texture2D[] texList){

    void Update(){
        
    }

    public void loadPreset(int index){
        Parameters paramChosen = paramList[index];
        Debug.Log("CHOSEN");
    }

}
