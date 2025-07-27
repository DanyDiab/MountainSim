using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] float[] bounds;

    // Start is called before the first frame update
    void Start(){
        if(bounds.Length != colors.Length){
            Debug.LogError("influence length is not the same as colors length");
            reachedError = true;
            return;
        }
    }

    // Update is called once per frame
    
    public void updatePixelColors(){
        mesh = FindAnyObjectByType<MeshFilter>().mesh;
        verticies = mesh.vertices;
        pixelColors = new Color[verticies.Length];
        for(int i = 0; i < verticies.Length; i++){
            Vector3 currVertex = verticies[i];
            // to do find the 2 closet bounds
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
                float percent = currY / (botBound + topBound);
                // blend the color chanels using the weighted average
                float rWeighted = (vertColor[0].r * percent) + (vertColor[1].r * (1 - percent));
                float gWeighted = (vertColor[0].g * percent) + (vertColor[1].g * (1 - percent));
                float bWeighted = (vertColor[0].b * percent) + (vertColor[1].b * (1 - percent));
                pixelColor = new Color(rWeighted,gWeighted,bWeighted);
            }
            pixelColors[i] = pixelColor;
            Debug.Log(pixelColor);
        }
        mesh.colors = pixelColors;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        }


    (float[], Color[]) findCloseBounds(float y){
        Debug.Log(bounds.Length);
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
            boundsToReturn = new float[] {bounds[i - 1], bounds[i]};
            colorsToReturn = new Color[] {colors[i-1], colors[i]};
            break;
        }
        return (boundsToReturn, colorsToReturn);

    }



    
}
