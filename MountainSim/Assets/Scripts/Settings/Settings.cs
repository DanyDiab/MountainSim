using UnityEngine;

[System.Serializable]
public class SettingsSaveData
{
    public float cameraSpeed;
    public float cameraSmoothing;
    public float mouseSensitivity;
    public float rotationSpeed;
    public int skyboxIndex;
}

[CreateAssetMenu(menuName = "Asset/Settings")]
public class Settings : ScriptableObject{
    [Header("Camera Settings")]
    [SerializeField] float cameraSpeed;
    [SerializeField] float cameraSmoothing;
    [SerializeField] float mouseSensitivity;

    [Header("Mesh Settings")]
    [SerializeField] float rotationSpeed;

    [Header("Environment Settings")]
    [SerializeField] int skyboxIndex;
    bool isFirstTime = true;


    public SettingsSaveData GetSaveData()
    {
        SettingsSaveData data = new SettingsSaveData();
        data.cameraSpeed = cameraSpeed;
        data.cameraSmoothing = cameraSmoothing;
        data.mouseSensitivity = mouseSensitivity;
        data.rotationSpeed = rotationSpeed;
        data.skyboxIndex = skyboxIndex;
        return data;
    }

    public void LoadFromSaveData(SettingsSaveData data)
    {
        cameraSpeed = data.cameraSpeed;
        cameraSmoothing = data.cameraSmoothing;
        mouseSensitivity = data.mouseSensitivity;
        rotationSpeed = data.rotationSpeed;
        skyboxIndex = data.skyboxIndex;
    }

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
    public int SkyboxIndex
    {
        get => skyboxIndex;
        set => skyboxIndex = value;
    }
    
    public bool IsFirstTime {
        get => isFirstTime;
        set => isFirstTime = value;
    }
 

}
