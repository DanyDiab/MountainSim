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

    void Awake(){
        subPanels = new List<GameObject> {camPanel,controlsPanel,skyboxPanel};
    }

    void OnEnable(){
        MenuUtil.ShowPanel(camPanel);
        MenuUtil.LinkSliderAndInputField(mouseSens,sensInput,false,"F2");
        MenuUtil.LinkSliderAndInputField(moveSpeed,speedInput,false,"F2");
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
