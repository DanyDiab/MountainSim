using System;
using System.Collections;
using System.Collections.Generic;
using SFB;
using UnityEngine;
using System.IO;

public static class JsonSOConvert
{

    public static ParametersSaveData OnLoadClicked() {
        // open dialog and choose and take the first path
        var paths = StandaloneFileBrowser.OpenFilePanel("presets", "", "preset",false);
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0])) {
            string path = paths[0];
            string jsonContent = File.ReadAllText(path);
            return ParametersSaveData.GetSaveFromJsonString(jsonContent);
        }
        return null;
    }

    public static void OnSaveToDiskClicked(ParametersSaveData data) {
        // open dialog and choose and take the first path
        string name = data.name;
        string path = StandaloneFileBrowser.SaveFilePanel("Save Preset","", name,"preset");

        string jsonString = data.GetJsonString(data);
        try {
            File.WriteAllText(path,jsonString);
        }
        catch(Exception e) {
            Debug.LogFormat("something went wrong with the writing to disk: {0}", e.Message);
        }


    }
}
