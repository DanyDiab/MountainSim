using UnityEngine;
using UnityEngine.UI;

public enum MoveState{
    Idle,
    Moving
}

public enum FadingState
{
    Idle,
    Fading
}
public class UIElemMove : MonoBehaviour
{

    Vector3 blCorner;
    Vector3 center;
    Camera mainCamera;
    Vector3 dest;
    RectTransform currTransform;
    Image fadingImage;
    FadingState currFadeState;
    float aT;
    float fadeTarget;
    float startingFade;
    MoveState currState;
    float currSpeed = 1f;
    float fadingSpeed = 2f;
    float arriveTresh;
    float t;

    void Start()
    {
        mainCamera = Camera.main;
        currState = MoveState.Idle;
        currFadeState = FadingState.Idle;
        arriveTresh = .01f;
    }

    void Update()
    {
        updateMoving();
        updateFading();

    }

    void updateMoving()
    {
        if(!(currState == MoveState.Moving)) return;
        t += Time.deltaTime * currSpeed;
        bool arrived;
        (currTransform.position, arrived) = translate(transform.position,dest);
        if(arrived) currState = MoveState.Idle;
    }
    void updateFading(){
        if(!(currFadeState == FadingState.Fading)) return;
        aT += Time.deltaTime * fadingSpeed;
        float eased = ease(aT);
        fade(eased);
        if(eased >= 1) currFadeState = FadingState.Idle;
    }

    public void moveTowards(RectTransform transform, string destination){
        blCorner = mainCamera.ViewportToScreenPoint(new Vector3(.1f,.15f,0));
        center = mainCamera.ViewportToScreenPoint(new Vector3(.5f,.5f,0));
        currTransform = transform;
        if(destination == "center") dest = center;
        else if(destination == "bl") dest = blCorner;
        currState = MoveState.Moving;
        t = 0;
    }

    (Vector3, bool) translate(Vector3 curr, Vector3 dest){
        float eased = ease(t);
        float x = Mathf.Lerp(curr.x,dest.x,eased);
        float y = Mathf.Lerp(curr.y,dest.y,eased);
        Vector3 final = new Vector3(x,y,0);
        return (final, (curr - dest).magnitude <= arriveTresh);
    }

    public void setPosition(RectTransform transform, string destination){
        if(destination == "center") dest = center;
        else if(destination == "bl") dest = blCorner; 

        transform.position = dest;
    }

    public void fadeAlpha(Image image, string dir)
    {
        if(dir == "in") fadeTarget = 1; 
        
        else fadeTarget = 0;
        startingFade = image.color.a;
        fadingImage = image;
        currFadeState = FadingState.Fading;
        aT = 0;
    }

    public void setAlpha(Image image, float a)
    {
        image.color = new Color(image.color.r,image.color.g,image.color.b,a);
    }

    void fade(float t)
    {
        float newAlpha = Mathf.Lerp(startingFade,fadeTarget,t);
        fadingImage.color = new Color(fadingImage.color.r,fadingImage.color.g,fadingImage.color.b,newAlpha);
    }

    float ease(float t)
    {
        return t * t * t;
    }
}
