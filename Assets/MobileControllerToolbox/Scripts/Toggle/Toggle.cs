using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace MobileController
{
    [RequireComponent(typeof(Animator))]
    public class Toggle : MonoBehaviour, IPointerDownHandler
    {
        public enum InitialState { normal, disable };
        public enum ToggleState { normal, toggle, disable };
        
        
        [Header("Behaviour")]
        public InitialState initialState;
        [Space(15)]

        [Header("Events")]
        [Space(5)]
        public UnityEvent ToggleEvent;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;


        private ToggleState toggleState;
        private Animator anim;

        private bool isToggled = false;


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
                toggleState = ToggleState.normal;
            }
            else
            {
                anim.SetTrigger("Disable");
                toggleState = ToggleState.disable;
            }
        }

        #endregion


        #region Pointer Events

        public void OnPointerDown(PointerEventData eventData)
        {
            if (toggleState != ToggleState.disable)
            {
                // handle animation
                if (isToggled) anim.SetTrigger("Normal");
                else anim.SetTrigger("Toggle");

                // handle state
                ToggleEvent.Invoke();
                isToggled = !isToggled;
            }
        }

        #endregion


        #region Getters / Setters

        public ToggleState GetToggleState() { return toggleState; }
        
        
        public void Enable()
        {
            EnableEvent.Invoke();
            anim.SetTrigger("Normal");
            toggleState = ToggleState.normal;
        }

        public void Disable()
        {
            DisableEvent.Invoke();
            anim.SetTrigger("Disable");
            toggleState = ToggleState.disable;
        }

        #endregion
    }
}