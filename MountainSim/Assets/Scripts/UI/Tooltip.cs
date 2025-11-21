using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] string tooltipText;
    public void OnPointerEnter(PointerEventData eventData){
        TooltipManager.show(true, tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData){
        TooltipManager.show(false, null);
    }
}
