using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;

public class MiscMenu : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Settings settings;

    [Header("Menu Elements")]
    [SerializeField] Slider mouseSens;
    [SerializeField] Slider moveSpeed;

    [SerializeField] TMP_InputField sensInput;
    [SerializeField] TMP_InputField speedInput;


    [Header("Panels")]

    [SerializeField] GameObject camPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject skyboxPanel;
    List<GameObject> subPanels;
    private static string fileName = "settings";

    void Awake(){
        if(settings != null){
            SettingsSaveData data = settings.GetSaveData();
            MenuUtil.Load(data, fileName);
            settings.LoadFromSaveData(data);
        }
        subPanels = new List<GameObject> {camPanel,controlsPanel,skyboxPanel};
    }

    void OnEnable(){
        LoadSettings();
        MenuUtil.ShowPanel(camPanel, subPanels);
        MenuUtil.LinkSliderAndInputField(mouseSens,sensInput,false,"F2", SaveSettings);
        MenuUtil.LinkSliderAndInputField(moveSpeed,speedInput,false,"F2", SaveSettings);
    }

    public void LoadSettings(){
        if(settings == null) return;

        mouseSens.value = settings.MouseSensitivity;
        moveSpeed.value = settings.CameraSpeed;

        sensInput.SetTextWithoutNotify(settings.MouseSensitivity.ToString("F2"));
        speedInput.SetTextWithoutNotify(settings.CameraSpeed.ToString("F2"));
    }

    public void SaveSettings(){
        if(settings == null) return;

        settings.MouseSensitivity = mouseSens.value;
        settings.CameraSpeed = moveSpeed.value;
        
        MenuUtil.Save(settings.GetSaveData(), fileName);
    }

    public void loadCamPanel(){
        MenuUtil.ShowPanel(camPanel, subPanels);
    }

    public void loadControlsPanel(){
        MenuUtil.ShowPanel(controlsPanel, subPanels);
    }

    public void loadSkyboxPanel(){
        MenuUtil.ShowPanel(skyboxPanel, subPanels);
    }
}
