using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace MobileController
{
    [RequireComponent(typeof(Animator))]
    public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
    {
        public enum InitialState { normal, disable };
        public enum ButtonState { normal, tap, hold, disable };

        [Header("Behaviour")]
        public InitialState initialState;
        public float holdTimeThreshold = 1.0f;
        public bool enableHold = false;
        [Space(15)]

        [Header("Events")]
        [Space(5)]
        public UnityEvent TapStartEvent;
        public UnityEvent TapEndEvent;
        public UnityEvent HoldStartEvent;
        public UnityEvent HoldEndEvent;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;

        public UnityEvent EnterEvent;
        public UnityEvent ExitEvent;


        private ButtonState buttonState;
        private Animator anim;

        private float currentHoldTime = 0.0f;
        private bool isHolding = false;


        /**/


        #region Setup

        private void Awake()
        {
            // setup animator
            anim = this.GetComponent<Animator>();

            // update state
            if (initialState == InitialState.normal)
            {
                anim.SetTrigger("Normal");
                buttonState = ButtonState.normal;
            }
            else
            {
                anim.SetTrigger("Disable");
                buttonState = ButtonState.disable;
            }
        }

        #endregion


        #region Pointer Events

        public void OnPointerUp(PointerEventData eventData) { PointerUp(); }
        public void OnPointerExit(PointerEventData eventData) { PointerUp(); ExitEvent.Invoke(); }

        public void OnPointerEnter(PointerEventData eventData) { EnterEvent.Invoke(); }


        public void OnPointerDown(PointerEventData eventData)
        {
            if (buttonState != ButtonState.disable)
            {
                TapStartEvent.Invoke();
                anim.SetTrigger("Tap");
                buttonState = ButtonState.tap;

                isHolding = true;
            }
        }


        private void PointerUp()
        {
            if (buttonState == ButtonState.hold)
            {
                HoldEndEvent.Invoke();
                anim.SetTrigger("Normal");
                buttonState = ButtonState.normal;
            }
            else if (buttonState == ButtonState.tap)
            {
                TapEndEvent.Invoke();
                anim.SetTrigger("Normal");
                buttonState = ButtonState.normal;

                ResetHold();
            }
        }

        #endregion


        #region Update

        private void Update()
        {
            // check if the button has been held long enough
            if (isHolding && enableHold)
            {
                currentHoldTime += Time.deltaTime;

                if (currentHoldTime >= holdTimeThreshold)
                {
                    HoldStartEvent.Invoke();
                    anim.SetTrigger("Hold");
                    buttonState = ButtonState.hold;

                    ResetHold();
                }
            }
        }


        private void ResetHold()
        {
            isHolding = false;
            currentHoldTime = 0.0f;
        }

        #endregion


        #region Getters / Setters

        public ButtonState GetButtonState() { return buttonState; }


        public void Enable()
        {
            EnableEvent.Invoke();
            anim.SetTrigger("Normal");
            buttonState = ButtonState.normal;
        }


        public void Disable()
        {
            DisableEvent.Invoke();
            anim.SetTrigger("Disable");
            buttonState = ButtonState.disable;
        }

        #endregion
    }
}