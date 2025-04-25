using System.Collections.Generic;
using UnityEngine;

namespace AppCore.Runtime
{
    public class ViewScreen : MonoBehaviour
    {
        [SerializeField] private Camera screenCamera;
        private readonly List<IView> views = new(10);
        private int sortingOrder;

        public void Push(IView view)
        {
            if (!views.Contains(view))
            {
                views.Add(view);
            }

            view.Canvas.worldCamera = screenCamera;
            view.Canvas.sortingOrder = ++sortingOrder;
            view.Canvas.transform.SetParent(transform);
        }

        public void Pop(IView view)
        {
            if (views.Count == 0)
                return;

            if (!views.Contains(view))
                return;

            sortingOrder--;
            sortingOrder = Mathf.Max(sortingOrder, 0);
        }
    }
}