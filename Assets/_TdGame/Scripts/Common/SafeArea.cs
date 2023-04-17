using UnityEngine;

namespace Pump.Unity
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        RectTransform safeAreaRect;
        Canvas canvas;
        Rect lastSafeArea;

        void Start()
        {
            safeAreaRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            OnRectTransformDimensionsChange();
        }

        void OnRectTransformDimensionsChange()
        {

            if (Screen.safeArea != lastSafeArea && canvas != null)
            {
                lastSafeArea = Screen.safeArea;
                UpdateSizeToSafeArea();
            }
        }

        void UpdateSizeToSafeArea()
        {
            var safeArea = Screen.safeArea;
            var inverseSize = new Vector2(1f, 1f) / canvas.pixelRect.size;
            var newAnchorMin = Vector2.Scale(safeArea.position, inverseSize);
            var newAnchorMax = Vector2.Scale(safeArea.position + safeArea.size, inverseSize);

            safeAreaRect.anchorMin = newAnchorMin;
            safeAreaRect.anchorMax = newAnchorMax;

            safeAreaRect.offsetMin = Vector2.zero;
            safeAreaRect.offsetMax = Vector2.zero;
        }
    }
}