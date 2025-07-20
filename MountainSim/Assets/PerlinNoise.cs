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
        generatePerlinNoise();

    }
    void generatePerlinNoise(){
        generateGraidentVectors();
        determineCells();
    }


    void generateGraidentVectors(){
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 origin = new Vector3(i * cellSize, j * cellSize, 0);
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector3 gradientVector = new Vector3(randDirX,randDirY, 0).normalized;
                gradientVectors[i,j] = gradientVector;
                Debug.DrawLine(origin, origin + gradientVector * cellSize, Color.green, 100f);
            }   
        }
    }

    void determineCells(){
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 point = new Vector3(i,j,0);
                Vector2 cell = determineCell(point);
                Debug.Log(cell);
            }
        }
    }




    Vector3 determineCell(Vector3 point){
        int cellX = (int)point.x / cellSize;
        int cellY = (int)point.y / cellSize;
        Vector3 cell = new Vector3(cellX,cellY,0);
        return cell;
    }
}


// steps
// generate X vectors, point them in random directions
// for each point in the grid:
    // determine which cell it belongs in
    // calculate influence of each of the 4 corners to the point
    // compute dot product of offset and gradient vectors
    // fade the fractinals 
    // interpolate the dot products using the faded vault