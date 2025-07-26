using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 pos;
    Vector3 prevMousePos;
    
    [Header("Camera Settings")]
    [SerializeField]float mouseSensitivity = 1f;
    [SerializeField] float speed = 1;
    [SerializeField]float camSmoothing = 2f;
    float minPitch = -90f;
    float maxPitch = 90f;
    float currPitch = 0f;
    float targetPitch = 0f;
    float currYaw = 0f;
    float targetYaw = 0f;
    bool locked;


    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        prevMousePos = Input.mousePosition;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        lockCam();
        if(locked) return;

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
        // calculate vector to rotate towards
        Vector3 dx = currMousePos - prevMousePos;
        float rotationY = dx.x * mouseSensitivity;
        float rotationX = -dx.y * mouseSensitivity;

        targetPitch = Mathf.Clamp(targetPitch + rotationX, minPitch, maxPitch);
        targetYaw += rotationY;
        // update currents using lerp to smooth
        currPitch = lerp(camSmoothing * Time.deltaTime, currPitch, targetPitch);
        currYaw = lerp(camSmoothing * Time.deltaTime, currYaw, targetYaw);
        // create Quaternion based on current pitch and yaw
        Quaternion targetRotation = Quaternion.Euler(currPitch, currYaw, 0);
        transform.rotation = targetRotation;
        // update previous mouse position
        prevMousePos = currMousePos;
    }

    float lerp(float t, float a, float b){
        return a + (b - a) * t;
    }

    void lockCam(){
        if(Input.GetKeyDown(KeyCode.L)){
            locked = !locked;
        }
    }    
}
