using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSymbol : MonoBehaviour
{
    [SerializeField] GameObject LoadingGameObject;
    [SerializeField] RectTransform tintRT;
    [SerializeField] RectTransform layoutGroupGameObject;

    void Start(){
        ToggleSpinner(false);
    }

    void OnEnable()
    {
        FbmNoise.OnNoiseStarted += ShowSpinner;
        NoiseRenderer.OnMeshGenerated += HideSpinner;
        Vector2 screenSize = new Vector2(Screen.width,Screen.height); 
        tintRT.sizeDelta = screenSize;
        foreach(RectTransform child in tintRT){
            child.sizeDelta = screenSize;
        }
        // layoutGroupGameObject.sizeDelta  = screenSize;
    }

    void OnDisable()
    {
        FbmNoise.OnNoiseStarted -= ShowSpinner;
        NoiseRenderer.OnMeshGenerated -= HideSpinner;
    }

    private void ShowSpinner()
    {
        ToggleSpinner(true);
    }

    private void HideSpinner()
    {
        ToggleSpinner(false);
    }

    public void ToggleSpinner(bool active)
    {
        if (LoadingGameObject != null)
        {
            LoadingGameObject.SetActive(active);
            Debug.Break();
        }
    }
}