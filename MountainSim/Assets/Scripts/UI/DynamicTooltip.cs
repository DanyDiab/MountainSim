using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DynamicTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMP_Dropdown targetDropdown;
    [TextArea(3, 10)]
    [SerializeField] string[] descriptions;
    [SerializeField] ToolTipType type;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetDropdown == null || descriptions == null || descriptions.Length == 0) return;

        int index = targetDropdown.value;
        string textToShow = "";

        if (index >= 0 && index < descriptions.Length)
        {
            textToShow = descriptions[index];
        }
        else
        {
            textToShow = "No description available for this selection.";
        }

        TooltipManager.show(true, textToShow, type);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.show(false, null, type);
    }

    public void OnDestroy()
    {
        TooltipManager.show(false, null, type);
    }
}
