using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    bool rotating;
    Mesh mesh;
    Vector3 center;
    [Header("Settings")]
    [SerializeField] Settings settings;
    float rotateSpeed;

    public delegate void RotateEvent(bool rotating);
    public static event RotateEvent OnRotate;
    void Start(){
        rotating = false;
        UIController.OnPause += updateRotateSpeed;
    }

    void updateRotateSpeed(bool inMenu){
        if(inMenu) return;

        rotateSpeed = settings.RotationSpeed;
    }

    void Update(){
        if(Input.GetKeyDown("r")){
            toggleRotate();
        }
        if(!rotating) return;

        transform.RotateAround(center, Vector3.up, Time.deltaTime * rotateSpeed);
    }

    void toggleRotate(){
        rotating = !rotating;
        OnRotate?.Invoke(rotating);
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
