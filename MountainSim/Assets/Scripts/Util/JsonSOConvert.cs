using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;

public class JsonSOConvert : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnLoadButtonClicked() {
        StandaloneFileBrowser.OpenFilePanel("presets", "", "json",false);
    }
}
