using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TerrainColoring : MonoBehaviour
{
    bool reachedError = false;
    public MeshFilter meshFilter;
    Mesh mesh;

    Vector3[] verticies;
    Color[] pixelColors;

    [Header("Colors")]
    [SerializeField] Color[] colors;
    [SerializeField] Texture2D[] textures;
    [SerializeField] Material shaderMat;

    TextureNormalizer tn;
    float[] bounds;


    void Start(){
        tn = new TextureNormalizer();
        tn.setResolution(1024);
        tn.setFormatting(RenderTextureFormat.ARGB32);
    }    
    public void updatePixelColors(){
        mesh = meshFilter.mesh;
        determineBounds(colors.Length, mesh.bounds.min.y, mesh.bounds.max.y);
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
        mesh = meshFilter.mesh;
        bounds = determineBounds(textures.Length, mesh.bounds.min.y, mesh.bounds.max.y);
        shaderMat.SetFloatArray("_Bounds", bounds);
        shaderMat.SetInt("_numBounds", bounds.Length);
        Texture2DArray texArray = new Texture2DArray(
            1024, 1024, bounds.Length, TextureFormat.ARGB32, true
        );
        for (int i = 0; i < textures.Length; i++) {
            Texture2D normalTexture = tn.normalizeTexture(textures[i]);
            Graphics.CopyTexture(normalTexture, 0, 0, texArray, i, 0);
        }
        shaderMat.SetTexture("_Textures", texArray);   
    }

    public void updateGradTex(int width){
        mesh = meshFilter.mesh;
        (float[] grads, float min, float max) = calculateGradients(mesh, width);
        float[] bounds = determineBounds(textures.Length,min,max);
        Texture2DArray texArray = new Texture2DArray(
            1024, 1024, bounds.Length, TextureFormat.ARGB32, true
        );


        
    }
    (float[], Color[]) findCloseBounds(float y){
        if(y >= bounds[bounds.Length - 1]){
            int index = bounds.Length - 1;
            return (new float[] {bounds[index]}, new Color[] {colors[index]});
        }
        if(y <= bounds[0]){
            int index = 0;
            return (new float[] {bounds[index]}, new Color[] {colors[index]});
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
            colorsToReturn = new Color[] {colors[i-1], colors[i]};
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

    public (float[], float, float) calculateGradients(Mesh mesh, int width) {
        float min = float.MaxValue;
        float max = float.MinValue;
        Vector3[] vertices = mesh.vertices;
        float[] grads = new float[vertices.Length];
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < width; j++) {
                int index = i * width + j;
                Vector3 curr = vertices[index];
                // Neighbor to the right
                Vector3 right = (j < width - 1) ? vertices[index + 1] : curr;
                // Neighbor below
                Vector3 down = (i < width - 1) ? vertices[index + width] : curr;

                float dx = right.y - curr.y;
                float dz = down.y - curr.y;
                float currGrad = Mathf.Sqrt(dx * dx + dz * dz);
                if(currGrad < min){
                    min = currGrad;
                }
                else if(currGrad > max){
                    max = currGrad;
                }
                grads[index] = currGrad;
            }
        }
        return (grads,min,max);
    }
}
