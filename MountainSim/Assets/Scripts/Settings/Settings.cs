using UnityEngine;

[CreateAssetMenu(menuName = "Asset/Settings")]
public class Settings : ScriptableObject{
    [Header("Camera Settings")]
    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraSmoothing;
    [SerializeField] float mouseSensitivity;

    [Header("Mesh Settings")]
    [SerializeField] float rotationSpeed;



    // getters /setters
    public float CameraSpeed
    {
        get => cameraSpeed;
        set => cameraSpeed = value;
    }
    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }
    public float MouseSensitivity
    {
        get => mouseSensitivity;
        set => mouseSensitivity = value;
    }
    public float CameraSmoothing
    {
        get => cameraSmoothing;
        set => cameraSmoothing = value;
    }


}
