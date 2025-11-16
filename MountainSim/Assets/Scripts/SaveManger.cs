using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveManger{
    public static void SaveParameters(Parameters parameters){
        PlayerPrefs.SetInt("CurrAlgorithm", (int)parameters.CurrAlgorithm);
        PlayerPrefs.SetInt("OctaveCount", parameters.OctaveCount);
        PlayerPrefs.SetFloat("Lacunarity", parameters.Lacunarity);
        PlayerPrefs.SetFloat("Persistence", parameters.Persistence);
        PlayerPrefs.SetInt("CurrentSeed", parameters.CurrentSeed);
        PlayerPrefs.SetFloat("HeightExageration", parameters.HeightExageration);
        PlayerPrefs.SetInt("RFactor", parameters.RFactor);
        PlayerPrefs.SetInt("GridSize", parameters.GridSize);
        PlayerPrefs.SetInt("CellSize", parameters.CellSize);
    }

    public static void LoadParameters(Parameters parameters){
        if(PlayerPrefs.HasKey("CurrAlgorithm")) parameters.CurrAlgorithm = (NoiseAlgorithms)PlayerPrefs.GetInt("CurrAlgorithm");
        if(PlayerPrefs.HasKey("OctaveCount"))parameters.OctaveCount = PlayerPrefs.GetInt("OctaveCount");
        if(PlayerPrefs.HasKey("Lacunarity"))parameters.Lacunarity = PlayerPrefs.GetFloat("Lacunarity");
        if(PlayerPrefs.HasKey("Persistence"))parameters.Persistence = PlayerPrefs.GetFloat("Persistence");
        if(PlayerPrefs.HasKey("CurrentSeed"))parameters.CurrentSeed = PlayerPrefs.GetInt("CurrentSeed");
        if(PlayerPrefs.HasKey("HeightExageration"))parameters.HeightExageration = PlayerPrefs.GetFloat("HeightExageration");
        if(PlayerPrefs.HasKey("RFactor"))parameters.RFactor = PlayerPrefs.GetInt("RFactor");
        if(PlayerPrefs.HasKey("GridSize"))parameters.GridSize = PlayerPrefs.GetInt("GridSize");
        if(PlayerPrefs.HasKey("CellSize"))parameters.CellSize = PlayerPrefs.GetInt("CellSize");
    }
}
