using UnityEngine;
using UnityEngine.UI;

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
        getPerlinValues();
    }


    void generateGraidentVectors(){
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 origin = new Vector3(i * cellSize, j * cellSize, 0);
                float randDirX = Random.Range(-1f,1f);
                float randDirY = Random.Range(-1f,1f);
                Vector3 gradientVector = new Vector3(randDirX,randDirY, 0).normalized;
                gradientVectors[i,j] = gradientVector;
            }   
        }
    }
    Vector3 determineCell(Vector3 point){
        int cellX = (int)point.x / cellSize;
        int cellY = (int)point.y / cellSize;
        Vector3 cell = new Vector3(cellX,cellY,0);
        return cell;
    }

    (Vector3,Vector3,Vector3,Vector3,Vector3,Vector3,Vector3,Vector3)calculateOffsetVectors(Vector3 point, Vector3 cell){
        int posX = (int) cell.x;
        int posY = (int) cell.y;
        Vector3 blCell = cell;
        Vector3 brCell = new Vector3(posX + cellSize, posY);
        Vector3 tlCell = new Vector3(posX, posY + cellSize);
        Vector3 trCell = new Vector3(posX + cellSize, posY + cellSize);

        Vector3 bl = point - blCell;
        Vector3 br = point - brCell;
        Vector3 tl = point - tlCell;
        Vector3 tr = point - trCell;
        return (tl,tr,bl,br, tlCell, trCell, blCell, brCell);
    }

    void getPerlinValues(){
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 point = new Vector3(i,j,0);
                Vector3 cell = determineCell(point);
                (
                    Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br, 
                    Vector3 tlCell, Vector3 trCell, Vector3 blCell, Vector3 brCell
                ) = calculateOffsetVectors(point, cell);

                float tlI = Vector3Dot(tl ,tlCell);
                float trI = Vector3Dot(tr, trCell);
                float blI = Vector3Dot(bl, blCell);
                float brI = Vector3Dot(br, brCell);

                float localX = (point.x - cell.x) / cellSize;
                float localY = (point.y - cell.y) / cellSize;

                float u = fade(localX);
                float v = fade(localY);

                float lerpBottom = lerp(blI, brI, u);
                float lerpTop = lerp(tlI,trI, u);

                float finalLerp = lerp(lerpBottom,lerpTop, v);

                // draw noise call goes here
            }
        }
    }

    float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    float lerp(float a, float b, float t){
        return a + (b - a) * t;
    }
    


    float Vector3Dot(Vector3 v1, Vector3 v2){
        return (v1.x * v2.x) + (v1.y * v2.y) + (v1.z * v2.z);
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