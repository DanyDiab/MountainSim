using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainColoring : MonoBehaviour
{
    public MeshFilter meshFilter;
    Mesh mesh;

    Vector3[] verticies;
    Color[] pixelColors;
    [SerializeField] Parameters parameters;
    Material currMat;
    [SerializeField]Material colorMat;
    [SerializeField]Material textureMat;
    [SerializeField] Material gradMat;
    TextureNormalizer tn;
    MeshRenderer mr;
    float[] bounds;


    void Start(){
    
        tn = new TextureNormalizer();
        tn.setResolution(1024);
        tn.setFormatting(RenderTextureFormat.ARGB32);
        mr = meshFilter.GetComponent<MeshRenderer>();

    }    
    public void updatePixelColors(){
        currMat = colorMat;
        mesh = meshFilter.mesh;
        mr.material = currMat;
        determineBounds(parameters.Colors.Length, mesh.bounds.min.y, mesh.bounds.max.y);
        verticies = mesh.vertices;
        pixelColors = new Color[verticies.Length];
        for(int i = 0; i < verticies.Length; i++){
            Vector3 currVertex = verticies[i];
            float currY = currVertex.y;

            (float[] vertBound, Color[] vertColor) = findCloseBounds(currY);
            Color pixelColor;
            // if only 1 bound is found, use that color
            if(vertBound.Length == 1){
                pixelColor = vertColor[0];
            }
            // other wise find weighted average
            else{
                // grab the 2 bounds
                float botBound = vertBound[0];
                float topBound = vertBound[1];
                // calculate the percent from the bottom
                float percent = (currY - botBound) / (topBound - botBound);
                // blend the color chanels using the weighted average
                float rWeighted = (vertColor[0].r * (1 - percent)) + (vertColor[1].r * percent);
                float gWeighted = (vertColor[0].g * (1 - percent)) + (vertColor[1].g * percent);
                float bWeighted = (vertColor[0].b * (1 - percent)) + (vertColor[1].b * percent);
                pixelColor = new Color(rWeighted,gWeighted,bWeighted);
            }
            pixelColors[i] = pixelColor;
        }
        mesh.colors = pixelColors;
        }


    public void updatePixelTex(){
        currMat = textureMat;
        mr.material = currMat;
        mesh = meshFilter.mesh;
        bounds = determineBounds(parameters.Layers, mesh.bounds.min.y, mesh.bounds.max.y);
        currMat.SetFloatArray("_Bounds", bounds);
        currMat.SetInt("_numBounds", bounds.Length);
        currMat.SetFloat("_TilingFactor",parameters.UVScale);
        Texture2DArray texArray = new Texture2DArray(
            1024, 1024, bounds.Length, TextureFormat.ARGB32, true
        );
        for (int i = 0; i < parameters.Layers; i++) {
            Texture2D normalTexture = tn.normalizeTexture(parameters.CurrTextures[i]);
            for (int m = 0; m < normalTexture.mipmapCount; m++)
            {
                Graphics.CopyTexture(normalTexture, 0, m, texArray, i, m);
            }
        }
        currMat.SetTexture("_Textures", texArray);   
    }

    public void updateGradTex(){
        currMat = gradMat;
        mr.material = currMat;
        mesh = meshFilter.mesh;
        (float min, float max) = calculateGradients(mesh);
        float[] bounds = determineBounds(parameters.Layers,min,max);
        currMat.SetFloatArray("_Bounds", bounds);
        currMat.SetInt("_numBounds", bounds.Length);
        currMat.SetFloat("_TilingFactor",parameters.UVScale);
        Texture2DArray texArray = new Texture2DArray(
            1024, 1024, bounds.Length, TextureFormat.ARGB32, true
        );
        for (int i = 0; i < parameters.Layers; i++) {
            Texture2D normalTexture = tn.normalizeTexture(parameters.CurrTextures[i]);
            for (int m = 0; m < normalTexture.mipmapCount; m++)
            {
                Graphics.CopyTexture(normalTexture, 0, m, texArray, i, m);
            }
        }
        currMat.SetTexture("_Textures", texArray);  
    }
    (float[], Color[]) findCloseBounds(float y){
        if(y >= bounds[bounds.Length - 1]){
            int index = bounds.Length - 1;
            return (new float[] {bounds[index]}, new Color[] {parameters.Colors[index]});
        }
        if(y <= bounds[0]){
            int index = 0;
            return (new float[] {bounds[index]}, new Color[] {parameters.Colors[index]});
        }
        float[] boundsToReturn = new float[2];
        Color[] colorsToReturn = new Color[2];
        for(int i = 0; i < bounds.Length; i++){
            float currBound = bounds[i];
            // continue until we find the first bound that is greater than our y
            if(currBound < y){
                continue;
            }
            boundsToReturn = new float[] {bounds[i-1], bounds[i]};
            colorsToReturn = new Color[] {parameters.Colors[i-1], parameters.Colors[i]};
            break;
        }
        return (boundsToReturn, colorsToReturn);

    }

    float[] determineBounds(int len, float min, float max){
        bounds = new float[len];
        float heightDiff = max - min;
        float stepSize = heightDiff / (len - 1);
        float currPos = min;
        for(int i = 0; i < len; i++){
            bounds[i] = currPos;
            currPos += stepSize;
        }
        return bounds;
    }

    public (float, float) calculateGradients(Mesh mesh) {
        float min = float.MaxValue;
        float max = float.MinValue;
        Vector3[] normals = mesh.normals;
        foreach(Vector3 normal in normals){
            float normalY = normal.y;
            if(normalY < min){
                min = normalY;
            }
            else if(normalY > max){
                max = normalY;
            }
        }
        return (min,max);
 
    }
}
