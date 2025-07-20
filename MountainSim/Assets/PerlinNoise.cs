using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    int cellSize;
    // x by x grid of X cells
    int gridSize; 
    
    Vector3[,] gradientVectors;


    void Start()
    {
        cellSize = 4;
        gridSize = 100;    
        gradientVectors = new Vector3[gridSize, gridSize];
        generateGraidentVectors();

    }

    void Update()
    {
    }

    void generateGraidentVectors(){
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 origin = new Vector3(i * cellSize, j * cellSize, 0);
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector3 gradientVector = new Vector3(randDirX,randDirY, 0).normalized;
                Debug.Log(gradientVector);
                gradientVectors[i,j] = gradientVector;
                Debug.DrawLine(origin, origin + gradientVector * cellSize, Color.green, 100f);
            }   
        }
    }
}
