using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 pos;
    float speed;
    Vector3 prevMousePos;
    float mouseSensitivity = 1f;
    // Start is called before the first frame update
    void Start()
    {
        prevMousePos = Input.mousePosition;
        speed = .5f;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        moveCam();
        rotateCam();
        transform.position = pos;
    }

    void moveCam(){
        Vector3 dir = Vector3.zero;
        if(Input.GetKey(KeyCode.W)){
            dir += transform.forward;
        }
        if(Input.GetKey(KeyCode.A)){
            dir += transform.right * -1;
        }
        if(Input.GetKey(KeyCode.S)){
            dir += transform.forward * -1;
        }
        if(Input.GetKey(KeyCode.D)){
            dir += transform.right;
        }
        Vector3 dx = dir * speed * Time.deltaTime;
        pos += dx;
    }
    
    void rotateCam(){
        Vector3 currMousePos = Input.mousePosition;
        Vector3 dx = currMousePos - prevMousePos;
        float rotationY = dx.x * mouseSensitivity;
        float rotationX = -dx.y * mouseSensitivity;
        Vector3 currentEuler = transform.eulerAngles;
        Vector3 newEuler = new Vector3(
            currentEuler.x + rotationX,
            currentEuler.y + rotationY,
            currentEuler.z
        );
        transform.rotation = Quaternion.Euler(newEuler);
        prevMousePos = currMousePos;
    }

    
}
