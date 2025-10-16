using UnityEngine;


public enum MoveState{
    Idle,
    Moving
}
public class UIElemMove : MonoBehaviour
{

    Vector3 blCorner;
    Vector3 center;
    Camera mainCamera;
    Vector3 dest;
    RectTransform currTransform;
    MoveState currState;
    
    float t;

    void Start()
    {
        mainCamera = Camera.main;
        blCorner = mainCamera.ViewportToWorldPoint(new Vector3(0,0,transform.position.z));
        center = mainCamera.ViewportToWorldPoint(new Vector3(.5f,.5f,transform.position.z));
        currState = MoveState.Idle;
    }

    void Update()
    {
        if(!(currState == MoveState.Moving)) return;

        transform.position = translate(transform.position,dest);
    }

    public void moveTowards(RectTransform transform, string destination){
        currTransform = transform;
        if(destination == "center") dest = center;
        else if(destination == "bl") dest = blCorner;
        currState = MoveState.Moving;
        t = 0;
    }

    Vector3 translate(Vector3 curr, Vector3 dest){
        return Vector3.zero;   
    }
}
