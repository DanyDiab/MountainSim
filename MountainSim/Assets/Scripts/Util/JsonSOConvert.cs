using System;
using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using System.IO;

public static class JsonSOConvert
{
    // Update is called once per frame


    public static void OnLoadClicked() {
        // open dialog and choose and take the first path
        string path = StandaloneFileBrowser.OpenFilePanel("presets", "", "json",false)[0];

    }

    public static void OnSaveToDiskClicked(ParametersSaveData data) {
        // open dialog and choose and take the first path
        string name = data.name;
        string path = StandaloneFileBrowser.SaveFilePanel("Save Preset","", name,"preset");

        


    }
}
