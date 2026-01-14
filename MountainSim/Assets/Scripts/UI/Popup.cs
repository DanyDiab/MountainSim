using UnityEngine;
using UnityEngine.EventSystems;

public class Popup : MonoBehaviour{
    [SerializeField] string header;
    [TextArea(3, 10)]
    [SerializeField] string description;

    public void show(){
        PopupManager.Show(header, description);
    }
}
