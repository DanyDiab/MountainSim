using UnityEngine;
using UnityEngine.UI;

public class PerlinNoise : MonoBehaviour
{
    int cellSize;
    // x by x grid of X cells
    int gridSize; 
    

    private Texture2D noiseTexture;
    Vector3[,] gradientVectors;
    public Renderer targetRenderer;


    void Start()
    {
        cellSize = 1;
        gridSize = 100;
        gradientVectors = new Vector3[gridSize + 1, gridSize + 1];
        generatePerlinNoise();
        renderTexture();
    }
    void generatePerlinNoise(){
        generateGraidentVectors();
        getPerlinValues();
        
    }


    void generateGraidentVectors(){
        for(int i = 0; i < gridSize + 1; i++){
            for(int j = 0; j < gridSize + 1; j++){
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

// returning the first 4 vectors (off set vectors)
// returning the last 4 vectors (the corresponding cells)
    (Vector3,Vector3,Vector3,Vector3)calculateOffsetVectors(Vector3 point, Vector3 cell){
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
        return (tl,tr,bl,br);
    }

    void getPerlinValues(){
        Color[] pixels = new Color[gridSize * gridSize];
        Texture2D noiseTexture = new Texture2D(gridSize,gridSize);
        for(int i = 0; i < gridSize; i++){
            for(int j = 0; j < gridSize; j++){
                Vector3 point = new Vector3(i,j,0);
                Vector3 cell = determineCell(point);
                (Vector3 tl, Vector3 tr, Vector3 bl, Vector3 br) = calculateOffsetVectors(point, cell);
                float tlI = Vector3Dot(tl , gradientVectors[(int)cell.x,(int)cell.y]);
                float trI = Vector3Dot(tr, gradientVectors[(int)cell.x + 1,(int)cell.y]);
                float blI = Vector3Dot(bl, gradientVectors[(int)cell.x,(int)cell.y + 1]);
                float brI = Vector3Dot(br, gradientVectors[(int)cell.x + 1,(int)cell.y + 1]);
                
                // Debug.Log(tlI);
                // Debug.Log(trI);
                // Debug.Log(blI);
                // Debug.Log(brI);

                float localX = (float)(point.x - cell.x) / cellSize;
                float localY = (float)(point.y - cell.y) / cellSize;
                // Debug.Log("point" + point);
                // Debug.Log("cell" + cell);
                // Debug.Log("localX" + localX);
                // Debug.Log("localY" + localY);
                float u = fade(localX);
                float v = fade(localY);

                float lerpBottom = lerp(blI, brI, u);
                float lerpTop = lerp(tlI,trI, u);

                float finalLerp = lerp(lerpBottom,lerpTop, v);
                Debug.Log(finalLerp);
                pixels[j * gridSize + i] = new Color(finalLerp,finalLerp,finalLerp);
                // draw noise call goes here
            }
        }
        noiseTexture.SetPixels(pixels);
        noiseTexture.Apply();
    }

    void renderTexture(){
        if (targetRenderer != null){
            targetRenderer.sharedMaterial.mainTexture = noiseTexture;
        }
        else{
            Debug.LogWarning("Target Renderer not assigned. Please assign a Plane or Quad to display the noise.");
        }
    }

    float fade(float t){
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    float lerp(float a, float b, float t){
        return a + (b - a) * t;
    }
    
    float valueToBrightness(float value){
        float brightness = (value + 1) / 2;
        return brightness;
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