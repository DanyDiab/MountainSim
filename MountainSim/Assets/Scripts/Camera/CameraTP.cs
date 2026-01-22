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
        Bounds bounds = mf.mesh.bounds;
        currPos.y = bounds.max.y;
        currPos.x = bounds.center.x;
        currPos.z = bounds.center.z;
        return currPos;
    }
}
