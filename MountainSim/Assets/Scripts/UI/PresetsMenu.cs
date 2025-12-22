using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetsMenu : MonoBehaviour
{
    private static string fileName = "presets";
    Presets presets;
    void Start()
    {
        SaveManger.Load(presets, fileName);
        
    }


    void Update()
    {
        
    }
}
