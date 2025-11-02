using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Utility
{
    public class PointerCapturer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action Entered;
        public event Action Exited;
        public event Action Clicked;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Entered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Exited?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke();
        }
    }
}