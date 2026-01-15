using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using TMPro;

public static class MenuUtil{
    public static void loadDyanmicGrid(GridLayoutGroup grid, int total, GameObject prefab, Action<GameObject,int> initFunc, Texture2D[] texList){
        if (grid == null) {
            Debug.LogError("Grid Layout Group is not assigned!");
            return;
        }

        foreach (Transform child in grid.transform) {
            UnityEngine.Object.Destroy(child.gameObject);
        }

        for (int i = 0; i < total; i++) {
            GameObject newElement = UnityEngine.Object.Instantiate(prefab, grid.transform);
            newElement.transform.localScale = Vector3.one;
            RawImage img = newElement.GetComponentInChildren<RawImage>();
            if (img != null && texList[i] != null) {
                img.texture = texList[i];
            }
            initFunc(newElement, i);
        }
    }

    public static void Save<T>(T data, string filename){
        string path = Path.Combine(Application.persistentDataPath, filename);
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public static void Load<T>(T data, string filename) where T : class{
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, data);
        }
    }

    public static void LinkSliderAndInputField(Slider slider, TMP_InputField inputField, bool isInt, string format, Action onUpdate = null){
        slider.onValueChanged.AddListener(value => {
            inputField.SetTextWithoutNotify(isInt ? ((int)value).ToString() : value.ToString(format));
            onUpdate?.Invoke();
        });

        inputField.onEndEdit.AddListener(text => {
            if (float.TryParse(text, out float value))
            {
                slider.value = Mathf.Clamp(value, slider.minValue, slider.maxValue);
                onUpdate?.Invoke();
            }
        });
        // invoke the onupdate once at the start
        onUpdate?.Invoke();
        // update text on start
        inputField.text = slider.value.ToString();
    }

// show this panel and hide all other panels
    public static void ShowPanel(GameObject panelToShow, List<GameObject> allPanels)
    {
        if (allPanels == null) return;
        foreach (GameObject panel in allPanels)
        {
            if (panel != null)
            {
                panel.SetActive(panel == panelToShow);
            }
        }
    }

// show this panel
    public static void ShowPanel(GameObject panelToShow){
        if(panelToShow == null) return;

        panelToShow.SetActive(true);
    }
}
