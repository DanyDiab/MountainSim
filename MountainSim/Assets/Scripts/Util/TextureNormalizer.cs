using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureNormalizer : MonoBehaviour
{

    int resolution;
    RenderTextureFormat formatting;


    // work in progress
    public Texture2D normalizeTexture(Texture2D oldTex){
        Texture2D newTex = new Texture2D(resolution,resolution);
        RenderTexture rt = new RenderTexture(resolution,resolution,0,formatting,-1);
        // store the previous active texture
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;
        rt.filterMode = FilterMode.Trilinear;
        // copy data from the old texture to the render texture
        Graphics.Blit(oldTex,rt);
        // read from render texture to the new texture
        newTex.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
        newTex.Apply();
        // release the hardware used by the rt
        rt.Release();
        RenderTexture.active = previous;

        return newTex;
    }





    public void setResolution(int resolution){
        this.resolution = resolution;
    }

    public void setFormatting(RenderTextureFormat textureFormat){
        formatting = textureFormat;
    }
}
