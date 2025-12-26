using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollFixer : MonoBehaviour
{
    [Header("Click this to auto-fix")]
    public bool fixNow = false;

    void OnValidate()
    {
        if (fixNow)
        {
            fixNow = false;
            FixScrollView();
        }
    }

    void Start()
    {
        // Optional: Run on start to ensure it's always correct
        FixScrollView();
    }

    public void FixScrollView()
    {
        ScrollRect scroll = GetComponent<ScrollRect>();
        if (!scroll) return;

        // 1. Setup Viewport (The Mask)
        if (scroll.viewport != null)
        {
            RectTransform viewRect = scroll.viewport;
            viewRect.anchorMin = Vector2.zero; // Stretch
            viewRect.anchorMax = Vector2.one;  // Stretch
            viewRect.pivot = new Vector2(0, 1); // Top-Left Pivot
            viewRect.offsetMin = Vector2.zero;
            viewRect.offsetMax = Vector2.zero;
        }

        // 2. Setup Content (The Container)
        if (scroll.content != null)
        {
            RectTransform contentRect = scroll.content;
            
            // Pivot: Top-Center (Crucial for centering items)
            contentRect.pivot = new Vector2(0.5f, 1f);
            
            // Anchor: Top-Stretch (Stretches width, pinned to top)
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            
            // Reset Position so it starts at the top
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0, 0); // Reset offsets

            // 3. Configure Content Size Fitter
            ContentSizeFitter fitter = contentRect.GetComponent<ContentSizeFitter>();
            if (!fitter) fitter = contentRect.gameObject.AddComponent<ContentSizeFitter>();
            
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained; // Let parent control width
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;   // Auto-height
        
            // 4. Configure Grid Layout Group (if exists)
            GridLayoutGroup grid = contentRect.GetComponent<GridLayoutGroup>();
            if (grid)
            {
                grid.childAlignment = TextAnchor.UpperCenter; // Center items horizontally
                grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
                grid.startAxis = GridLayoutGroup.Axis.Horizontal;
            }
        }

        Debug.Log("Scroll View Layout Fixed!");
    }
}
