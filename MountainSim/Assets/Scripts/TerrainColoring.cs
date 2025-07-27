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
    [SerializeField] float[] influences;

    // Start is called before the first frame update
    void Start(){
        if(influences.Length != colors.Length){
            Debug.LogError("influence length is not the same as colors length");
            reachedError = true;
            return;
        }
        mesh = meshFilter.mesh;
        verticies = mesh.vertices;
        pixelColors = new Color[verticies.Length];
        if(reachedError) return;


    }

    // Update is called once per frame
    
    void updatePixelColors(){
        for(int i = 0; i < verticies.Length; i++){
            Vector3 currVertex = verticies[i];
            // to do find the 2 closet bounds
            (float[] vertBound, Color[] vertColor) = findCloseBounds();
            Color pixelColor;
            // if only 1 bound is found, use that color
            if(vertBound.Length == 1){
                pixelColor = vertColor[0];
            }
            // other wise find weighted average
            else{
                float topBound;
                float botBound;
            }

        }
    }



    
}
