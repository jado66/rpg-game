using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace MobileController
{
    public class ArrowKeys : MonoBehaviour, IPointerExitHandler
    {
        public enum InitialState { normal, disable };
        public enum ArrowKeysState { normal, disable };

        [Header("Behaviour")]
        public InitialState initialState;
        [Space(15)]

        [Header("Reference")]
        public Key upKey;
        public Key rightKey;
        public Key downKey;
        public Key leftKey;
        [Space(15)]

        [Header("Events")]
        [Space(5)]
        public UnityEvent UpTapEvent;
        public UnityEvent UpExitEvent;
        public UnityEvent RightTapEvent;
        public UnityEvent RightExitEvent;
        public UnityEvent DownTapEvent;
        public UnityEvent DownExitEvent;
        public UnityEvent LeftTapEvent;
        public UnityEvent LeftExitEvent;
        public UnityEvent ExitArrowKeysEvent;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;


        private Vector2Int tappedKeys = Vector2Int.zero;
        private ArrowKeysState arrowKeysState;


        /**/


        #region Setup

        private void Start()
        {
            // update state
            if (initialState == InitialState.normal) arrowKeysState = ArrowKeysState.normal;
            else arrowKeysState = ArrowKeysState.disable;

            // set direction state
            upKey.SetState(arrowKeysState);
            rightKey.SetState(arrowKeysState);
            downKey.SetState(arrowKeysState);
            leftKey.SetState(arrowKeysState);
        }

        #endregion


        #region Pointer Events

        public void OnPointerExit(PointerEventData eventData)
        {
            ClearKeys();            
            eventData.pointerPress = null;
        }

        #endregion


        #region Update

        public void ClearKeys()
        {
            SetActiveKeys(false, false, false, false);
            ExitArrowKeysEvent.Invoke();
        }


        public void UpdateKeys()
        {
            HandleDrag(upKey.GetDragged(), rightKey.GetDragged(), downKey.GetDragged(), leftKey.GetDragged());
        }


        private void HandleDrag(bool _up, bool _right, bool _down, bool _left)
        {
            if (_up)
            {
                if (_left) SetActiveKeys(true, false, false, true);
                else if (_right) SetActiveKeys(true, true, false, false);
                else SetActiveKeys(true, false, false, false);
            }
            else if (_right)
            {
                if (_down) SetActiveKeys(false, true, true, false);
                else SetActiveKeys(false, true, false, false);
            }
            else if (_down)
            {
                if (_left) SetActiveKeys(false, false, true, true);
                else SetActiveKeys(false, false, true, false);
            }
            else if (_left) SetActiveKeys(false, false, false, true);
            else SetActiveKeys(false, false, false, false);
        }


        public void SetActiveKeys(bool _upActive, bool _rightActive, bool _downActive, bool _leftActive)
        {
            if (arrowKeysState != ArrowKeysState.disable)
            {
                // calculate new active keys
                Vector2Int newTappedKeys = Vector2Int.zero;

                if (_upActive) newTappedKeys.y = 1;
                else if (_downActive) newTappedKeys.y = -1;

                if (_rightActive) newTappedKeys.x = 1;
                else if (_leftActive) newTappedKeys.x = -1;


                // update old active keys and call events
                if (newTappedKeys.y == 1 && tappedKeys.y != 1) UpTapEvent.Invoke();
                else if (newTappedKeys.y != 1 && tappedKeys.y == 1) UpExitEvent.Invoke();

                if (newTappedKeys.x == 1 && tappedKeys.x != 1) RightTapEvent.Invoke();
                else if (newTappedKeys.x != 1 && tappedKeys.x == 1) RightExitEvent.Invoke();

                if (newTappedKeys.y == -1 && tappedKeys.y != -1) DownTapEvent.Invoke();
                else if (newTappedKeys.y != -1 && tappedKeys.y == -1) DownExitEvent.Invoke();

                if (newTappedKeys.x == -1 && tappedKeys.x != -1) LeftTapEvent.Invoke();
                else if (newTappedKeys.x != -1 && tappedKeys.x == -1) LeftExitEvent.Invoke();

                tappedKeys = newTappedKeys;


                // update keys active state
                upKey.SetKeyState(_upActive);
                rightKey.SetKeyState(_rightActive);
                downKey.SetKeyState(_downActive);
                leftKey.SetKeyState(_leftActive);
            }
        }

        #endregion


        #region Getters / Setters

        public ArrowKeysState GetArrowKeysState() { return arrowKeysState; }
        public Vector2Int GetArrowKeysDirection() { return tappedKeys; }


        public void Enable()
        {
            EnableEvent.Invoke();

            upKey.SetState(ArrowKeysState.normal);
            rightKey.SetState(ArrowKeysState.normal);
            downKey.SetState(ArrowKeysState.normal);
            leftKey.SetState(ArrowKeysState.normal);

            arrowKeysState = ArrowKeysState.normal;
        }


        public void Disable()
        {
            DisableEvent.Invoke();

            upKey.SetState(ArrowKeysState.disable);
            rightKey.SetState(ArrowKeysState.disable);
            downKey.SetState(ArrowKeysState.disable);
            leftKey.SetState(ArrowKeysState.disable);

            arrowKeysState = ArrowKeysState.disable;
        }

        #endregion
    }
}