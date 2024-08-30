using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static MobileController.ArrowKeys;


namespace MobileController
{
    [RequireComponent(typeof(Image))]
    public class Key : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public enum KeyState { normal, tap, disable };

        [Header("References")]
        public ArrowKeys arrowKeys;


        private KeyState keyState;
        private Animator anim;

        private bool isMouseDown = false;


        /**/


        #region Setup

        private void Awake()
        {
            // setup animator
            anim = this.GetComponent<Animator>();
        }

        #endregion


        #region Pointer Events

        // detects also touch input on mobile
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (eventData.pointerPress != null)
            {
                isMouseDown = true;
                arrowKeys.UpdateKeys();
            }
        }
        

        public void OnPointerDown(PointerEventData eventData) { isMouseDown = true; arrowKeys.UpdateKeys(); }
        public void OnPointerUp(PointerEventData eventData) { isMouseDown = false; arrowKeys.ClearKeys(); }
        public void OnPointerExit(PointerEventData eventData) { isMouseDown = false; }

        #endregion


        #region Getters / Setters

        public bool GetDragged() { return isMouseDown; }


        public void SetState(ArrowKeysState _arrowKeysState)
        {
            if (_arrowKeysState == ArrowKeysState.normal)
            {
                if (anim != null) anim.SetTrigger("Normal");
                keyState = KeyState.normal;
            }
            else
            {
                if (anim != null) anim.SetTrigger("Disable");
                keyState = KeyState.disable;
            }
        }


        public void SetKeyState(bool _tapped)
        {
            // update direction state
            if (keyState == KeyState.normal && _tapped)
            {
                if (anim != null) anim.SetTrigger("Tap");
                keyState = KeyState.tap;
            }
            else if (keyState == KeyState.tap && !_tapped)
            {
                if (anim != null) anim.SetTrigger("Normal");
                keyState = KeyState.normal;
                isMouseDown = false;
            }
        }

        #endregion
    }
}