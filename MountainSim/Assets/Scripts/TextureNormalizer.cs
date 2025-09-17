using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureNormalizer : MonoBehaviour
{

    int resolution;
    TextureFormat formatting;
    bool generateMipMaps;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

// work in progress
    public Texture2D normalizeTexture(Texture2D texture){
        RenderTexture renderTexture = new RenderTexture(resolution,resolution,0,RenderTextureFormat.ARGB32);
        renderTexture.filterMode = FilterMode.Trilinear;
        Graphics.Blit(texture,renderTexture);
        return texture;
    }

    public void setResolution(int resolution){
        this.resolution = resolution;
    }

    public void setFormatting(TextureFormat textureFormat){
        formatting = textureFormat;
    }

    public void setMipMaps(bool generateMipMaps){
        this.generateMipMaps = generateMipMaps;
    }
}
