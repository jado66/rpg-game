using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace MobileController
{
    [RequireComponent(typeof(Animator))]
    public class Cooldown : MonoBehaviour, IPointerDownHandler
    {
        public enum FillType { radial, horizontal, vertical };
        public enum FillMode { refill, empty };


        public enum InitialState { normal, disable };
        public enum CooldownState { normal, cooldown, disable };


        [Header("Behaviour")]
        public InitialState initialState;
        [Min(0)] public float cooldownTime = 2.0f;
        [Space(15)]

        public bool enableText = false;
        [Min(0.01f)] public int decimalPlaces = 1;
        [Space(15)]

        public bool enableFill = true;
        public FillMode fillMode = FillMode.refill;
        public FillType fillType = FillType.radial;
        [Space(15)]

        [Header("References")]
        public Image fillImage;
        public TextMeshProUGUI countdownText;
        [Space(15)]

        [Header("Events")]
        [Space(5)]
        public UnityEvent CooldownStartEvent;
        public UnityEvent CooldownEndEvent;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;


        private CooldownState cooldownState;
        private Animator anim;

        private float currentTime;


        /**/


        #region Setup

        private void Awake()
        {
            // setup animator
            anim = this.GetComponent<Animator>();

            // setup image
            if (fillImage != null)
            {
                fillImage.type = Image.Type.Filled;

                if (fillType == FillType.radial) fillImage.fillMethod = Image.FillMethod.Radial360;
                else if (fillType == FillType.horizontal) fillImage.fillMethod = Image.FillMethod.Horizontal;
                else fillImage.fillMethod = Image.FillMethod.Vertical;
            }

            // update state
            if (initialState == InitialState.normal)
            {
                anim.SetTrigger("Normal");
                cooldownState = CooldownState.normal;
            }
            else
            {
                anim.SetTrigger("Disable");
                cooldownState = CooldownState.disable;
            }

            // reset button
            ResetButton();
        }

        #endregion


        #region Pointer Events

        public void OnPointerDown(PointerEventData eventData)
        {
            if (cooldownState != CooldownState.cooldown && cooldownState != CooldownState.disable)
            {
                CooldownStartEvent.Invoke();
                anim.SetTrigger("Cooldown");
                cooldownState = CooldownState.cooldown;
            }
        }

        #endregion


        #region Update

        private void Update()
        {
            if (cooldownState == CooldownState.cooldown)
            {
                // update time
                currentTime -= Time.deltaTime;

                // update visual
                if (enableText) UpdateText(false);
                if (enableFill) UpdateFill(false);

                // cooldown reaches 0
                if (currentTime < 0)
                {
                    CooldownEndEvent.Invoke();
                    anim.SetTrigger("Normal");
                    cooldownState = CooldownState.normal;

                    ResetButton();
                }
            }
        }


        private void UpdateFill(bool _reset)
        {
            if (fillImage != null)
            {
                if (!_reset)
                {
                    if (fillMode == FillMode.empty) fillImage.fillAmount = currentTime / cooldownTime;
                    else fillImage.fillAmount = 1f - currentTime / cooldownTime;
                }
                else fillImage.fillAmount = 0f;
            }
        }


        private void UpdateText(bool _reset)
        {
            if (countdownText != null)
            {
                if (!_reset) countdownText.text = currentTime.ToString("F" + decimalPlaces);
                else countdownText.text = " ";
            }
        }


        private void ResetButton()
        {
            currentTime = cooldownTime;

            UpdateText(true);
            UpdateFill(true);
        }

        #endregion


        #region Getters / Setters

        public CooldownState GetCooldownState() { return cooldownState; }
        public float GetCooldownTime() { return currentTime; }


        public void ClearCooldown()
        {
            CooldownEndEvent.Invoke();
            anim.SetTrigger("Normal");
            cooldownState = CooldownState.normal;

            ResetButton();
        }
        
        public void Enable()
        {
            EnableEvent.Invoke();
            anim.SetTrigger("Normal");
            cooldownState = CooldownState.normal;
        }


        public void Disable()
        {
            DisableEvent.Invoke();
            anim.SetTrigger("Disable");
            cooldownState = CooldownState.disable;
        }

        #endregion
    }
}