using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static MobileController.Dpad;


namespace MobileController
{
    [RequireComponent(typeof(Image))]
    public class Direction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public enum DirectionState { normal, tap, disable };

        [Header("References")]
        public Dpad dpad;


        private DirectionState directionState;
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
                dpad.UpdateDirections();
            }
        }


        public void OnPointerDown(PointerEventData eventData) { isMouseDown = true;  dpad.UpdateDirections(); }
        public void OnPointerUp(PointerEventData eventData) { isMouseDown = false; dpad.ClearDirections(); }
        public void OnPointerExit(PointerEventData eventData) { isMouseDown = false; }

        #endregion


        #region Getters / Setters

        public bool GetDragged() { return isMouseDown; }


        public void SetState(DpadState _dpadState)
        {
            if (_dpadState == DpadState.normal)
            {
                if (anim != null) anim.SetTrigger("Normal");
                directionState = DirectionState.normal;
            }
            else
            {
                if (anim != null) anim.SetTrigger("Disable");
                directionState = DirectionState.disable;
            }
        }


        public void SetDirectionState(bool _tapped)
        {
            // update direction state
            if(directionState == DirectionState.normal && _tapped)
            {
                if (anim != null) anim.SetTrigger("Tap");
                directionState = DirectionState.tap;
            }
            else if(directionState == DirectionState.tap && !_tapped)
            {
                if (anim != null) anim.SetTrigger("Normal");
                directionState = DirectionState.normal;
                isMouseDown = false;
            }
        }

        #endregion
    }
}