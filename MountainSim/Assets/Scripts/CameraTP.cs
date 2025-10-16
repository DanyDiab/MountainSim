using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraTP : MonoBehaviour
{
    public MeshFilter mf;
    // Start is called before the first frame update

    public Vector3 tpToMesh(){
        Vector3 currPos = transform.position;
        currPos.y = mf.mesh.bounds.max.y;
        return currPos;
    }
}
