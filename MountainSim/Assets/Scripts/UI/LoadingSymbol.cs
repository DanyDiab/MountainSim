using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSymbol : MonoBehaviour
{
    [SerializeField] private GameObject spinner;
    [SerializeField] private float speed = 1.0f;
    [Header("Animation Settings")]

    private Transform spinnerTransform;
    private float progress = 0f;

    void Start()
    {
        if (spinner != null){
            spinnerTransform = spinner.transform;
        }
        else
        {
            Debug.LogWarning("LoadingSymbol: Spinner GameObject is not assigned.");
        }
        ToggleSpinner(false);
    }

    void OnEnable()
    {
        FbmNoise.OnNoiseStarted += ShowSpinner;
        NoiseRenderer.OnMeshGenerated += HideSpinner;
    }

    void OnDisable()
    {
        FbmNoise.OnNoiseStarted -= ShowSpinner;
        NoiseRenderer.OnMeshGenerated -= HideSpinner;
    }

    private void ShowSpinner()
    {
        ToggleSpinner(true);
    }

    private void HideSpinner()
    {
        ToggleSpinner(false);
    }

    void Update()
    {
        if (spinner != null && spinner.activeSelf){
            progress += Time.unscaledDeltaTime * speed;

            if (progress > 1.0f){
                progress -= 1.0f;
            }
            float easedValue = Ease(progress);
            float zAngle = easedValue * 360f;
            if (spinnerTransform != null){
                spinnerTransform.localEulerAngles = new Vector3(0f, 0f, -zAngle);
            }
        }
    }

    public void ToggleSpinner(bool active)
    {
        if (spinner != null)
        {
            spinner.SetActive(active);
        }
    }

    private float Ease(float x)
    {
        const float c1 = 1.70158f;
        const float c2 = c1 * 1.525f;

        return x < 0.5f
            ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
            : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (2 * x - 2) + c2) + 2) / 2;
    }
}