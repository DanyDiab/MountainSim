using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    bool rotating;
    Mesh mesh;
    Vector3 center;
    [SerializeField] float rotateSpeed;
    void Start(){
        rotating = false;
    }

    void Update(){
        if(Input.GetKeyDown("r")){
            toggleRotate();
        }
        if(!rotating) return;

        transform.RotateAround(center, Vector3.up,Time.time * rotateSpeed);
    }

    void toggleRotate(){
        rotating = !rotating;
        if(rotating){
            MeshFilter mf = GetComponent<MeshFilter>();
            mesh = mf.mesh;
            center = calculateMeshLocalCenter();
        }
    }
    Vector3 calculateMeshLocalCenter(){
        Vector3 center = Vector3.zero;
        if (mesh.vertexCount > 0)
        {
            foreach (Vector3 vertex in mesh.vertices)
            {
                center += vertex;
            }
            center /= mesh.vertexCount;
        }
        return transform.TransformPoint(center);
    }
}
