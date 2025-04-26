using UnityEngine;

namespace AppCore.Runtime
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Vector2Int lastResolution;
        private Rect lastSafeArea;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            Apply(true);
        }

        private void Apply(bool force = false)
        {
            var safeArea = Screen.safeArea;
            var resolution = new Vector2Int(Screen.width, Screen.height);
            if (resolution.x == 0 || resolution.y == 0) return;

            if (!force && rectTransform.anchorMax != Vector2.zero)
            {
                if (lastSafeArea == safeArea && lastResolution == resolution)
                {
                    return;
                }
            }

            lastSafeArea = safeArea;
            lastResolution = resolution;

#if UNITY_EDITOR
            ApplyDrivenRectTransformTracker();
#endif

            var normalizedMin = new Vector2(safeArea.xMin / resolution.x, safeArea.yMin / resolution.y);
            var normalizedMax = new Vector2(safeArea.xMax / resolution.x, safeArea.yMax / resolution.y);

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = normalizedMin;
            rectTransform.anchorMax = normalizedMax;
        }

#if UNITY_EDITOR
        private DrivenRectTransformTracker drivenRectTransformTracker;

        private void ApplyDrivenRectTransformTracker()
        {
            drivenRectTransformTracker.Clear();
            drivenRectTransformTracker.Add(
                this,
                rectTransform,
                DrivenTransformProperties.AnchoredPosition
                | DrivenTransformProperties.SizeDelta
                | DrivenTransformProperties.AnchorMin
                | DrivenTransformProperties.AnchorMax
            );
        }

        private void OnDisable()
        {
            drivenRectTransformTracker.Clear();
        }
#endif

    }
}