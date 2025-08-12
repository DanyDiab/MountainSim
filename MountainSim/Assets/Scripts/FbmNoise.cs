using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class FbmNoise : MonoBehaviour
{
    // number of octaves to generate
    [Header("fBm parameters")]
    [SerializeField] int startingCellSize;
    [SerializeField] int numOctaves;
    // noise freqeuncy change per octave (increasing)
    [SerializeField] float lacunarity;

    // amplitude change between octaves (decreasing)
    [SerializeField] float peristence;
    public PerlinNoise perlinNoise;
    void Start(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void generateNoise(){

    }
}
