using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeActiveColor : MonoBehaviour
{
    Button btn;
    [SerializeField] GameObject targetPanel;
    
    [Header("Default Color")]
    [Space(10)]
    [SerializeField] ColorBlock defaultColors;
    [Header("Active Color")]
    [Space(10)]
    [SerializeField] ColorBlock activeColors;

    void Start()
    {
        btn = GetComponent<Button>();
    }

    void Update()
    {
        if (btn == null) return;

        bool panelActive = targetPanel.activeSelf;
        
        ColorBlock targetColorBlock = panelActive ? activeColors : defaultColors;
        if(targetColorBlock == btn.colors) {
            return;
        }
        btn.colors = targetColorBlock;        

    }
}