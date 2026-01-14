using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 pos;
    Vector3 prevMousePos;

    [Header("Settings")]
    [SerializeField] Settings settings;
    
    [Header("Camera Settings")]

    float sens;
    float speed;
    float camSmoothing = 2f;
    float minPitch = -90f;
    float maxPitch = 90f;
    float currPitch = 0f;
    float targetPitch = 0f;
    float currYaw = 0f;
    float targetYaw = 0f;
    bool locked;

    CameraTP cameraTP;
    Event lockEvent;
    public delegate void LockEvent(bool locked);
    public static event LockEvent OnLock;

    


    // Start is called before the first frame update
    void Start()
    {
        locked = false;
        prevMousePos = Input.mousePosition;
        pos = transform.position;
        cameraTP = GetComponent<CameraTP>();
        UIController.OnPause += updateCamVars;
    }

    void updateCamVars(bool inMenu){
        if(inMenu) return;
        
        speed = settings.CameraSpeed;
        sens = settings.MouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {

        lockCam();
        if(locked) return;
        moveCam();
        rotateCam();
        if(Input.GetKeyDown(KeyCode.T)){
            pos = cameraTP.tpToMesh();
        }
        transform.position = pos;

    }


    void moveCam(){
        Vector3 dir = Vector3.zero;
        Vector3 upDown = Vector3.zero;
        if(Input.GetKey(KeyCode.LeftShift)){
            upDown += Vector3.down;
        }
        if(Input.GetKey(KeyCode.Space)){
            upDown += Vector3.up;
        }
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
        Vector3 dx = (dir + upDown) * speed * Time.deltaTime;
        pos += dx;
    }
    
    void rotateCam(){
        // calculate vector to rotate towards
        float rotationY = Input.GetAxis("Mouse X") * sens;
        float rotationX = -Input.GetAxis("Mouse Y") * sens;

        targetPitch = Mathf.Clamp(targetPitch + rotationX, minPitch, maxPitch);
        targetYaw += rotationY;
        // update currents using lerp to smooth
        currPitch = lerp(camSmoothing * Time.deltaTime, currPitch, targetPitch);
        currYaw = lerp(camSmoothing * Time.deltaTime, currYaw, targetYaw);
        // create Quaternion based on current pitch and yaw
        Quaternion targetRotation = Quaternion.Euler(currPitch, currYaw, 0);
        transform.rotation = targetRotation;
        // update previous mouse position
    }

    float lerp(float t, float a, float b){
        return a + (b - a) * t;
    }

    void lockCam(){
        if(Input.GetKeyDown(KeyCode.L)){
            locked = !locked;
            OnLock?.Invoke(locked);
        }
    }    
}
