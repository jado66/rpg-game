using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace MobileController
{
    public class Dpad : MonoBehaviour, IPointerExitHandler
    {
        public enum InitialState { normal, disable };
        public enum DpadState { normal, disable };

        [Header("Behaviour")]
        public InitialState initialState;
        [Space(15)]

        [Header("References")]
        public Direction upDirection;
        public Direction rightDirection;
        public Direction downDirection;
        public Direction leftDirection;
        public Direction upRightDirection;
        public Direction upLeftDirection;
        public Direction downRightDirection;
        public Direction downLeftDirection;
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
        public UnityEvent ExitDpadEvent;
        public UnityEvent EnableEvent;
        public UnityEvent DisableEvent;


        private Vector2Int tappedDirections = Vector2Int.zero;
        private DpadState dpadState;


        /**/


        #region Setup

        private void Start()
        {
            // update state
            if (initialState == InitialState.normal) dpadState = DpadState.normal;
            else dpadState = DpadState.disable;

            // set direction state
            upDirection.SetState(dpadState);
            rightDirection.SetState(dpadState);
            downDirection.SetState(dpadState);
            leftDirection.SetState(dpadState);
        }

        #endregion


        #region Pointer Events

        public void OnPointerExit(PointerEventData eventData) 
        {
            ClearDirections();
            eventData.pointerPress = null;
        }

        #endregion


        #region Update

        public void ClearDirections()
        {
            SetActiveDirections(false, false, false, false);
            ExitDpadEvent.Invoke();
        }


        public void UpdateDirections()
        {
            HandleDrag(upDirection.GetDragged(), rightDirection.GetDragged(), downDirection.GetDragged(), leftDirection.GetDragged(), upRightDirection.GetDragged(), upLeftDirection.GetDragged(), downRightDirection.GetDragged(), downLeftDirection.GetDragged());
        }


        private void HandleDrag(bool _up, bool _right, bool _down, bool _left, bool _upRight, bool _upLeft, bool _downRight, bool _downLeft)
        {
            if (_up) SetActiveDirections(true, false, false, false);
            else if (_upRight) SetActiveDirections(true, true, false, false);
            else if (_right) SetActiveDirections(false, true, false, false);
            else if (_downRight) SetActiveDirections(false, true, true, false);
            else if (_down) SetActiveDirections(false, false, true, false);
            else if (_downLeft) SetActiveDirections(false, false, true, true);
            else if (_left) SetActiveDirections(false, false, false, true);
            else if (_upLeft) SetActiveDirections(true, false, false, true);
            else SetActiveDirections(false, false, false, false);
        }


        public void SetActiveDirections(bool _upActive, bool _rightActive, bool _downActive, bool _leftActive)
        {
            if (dpadState != DpadState.disable)
            {
                // calculate new active keys
                Vector2Int newTappedDirections = Vector2Int.zero;

                if (_upActive) newTappedDirections.y = 1;
                else if (_downActive) newTappedDirections.y = -1;

                if (_rightActive) newTappedDirections.x = 1;
                else if (_leftActive) newTappedDirections.x = -1;


                // update old active keys and call events
                if (newTappedDirections.y == 1 && tappedDirections.y != 1) UpTapEvent.Invoke();
                else if (newTappedDirections.y != 1 && tappedDirections.y == 1) UpExitEvent.Invoke();

                if (newTappedDirections.x == 1 && tappedDirections.x != 1) RightTapEvent.Invoke();
                else if (newTappedDirections.x != 1 && tappedDirections.x == 1) RightExitEvent.Invoke();

                if (newTappedDirections.y == -1 && tappedDirections.y != -1) DownTapEvent.Invoke();
                else if (newTappedDirections.y != -1 && tappedDirections.y == -1) DownExitEvent.Invoke();

                if (newTappedDirections.x == -1 && tappedDirections.x != -1) LeftTapEvent.Invoke();
                else if (newTappedDirections.x != -1 && tappedDirections.x == -1) LeftExitEvent.Invoke();

                tappedDirections = newTappedDirections;


                // update keys active state
                upDirection.SetDirectionState(_upActive);
                rightDirection.SetDirectionState(_rightActive);
                downDirection.SetDirectionState(_downActive);
                leftDirection.SetDirectionState(_leftActive);
            }
        }

        #endregion


        #region Getters / Setters

        public DpadState GetDpadState() { return dpadState; }
        public Vector2Int GetDpadDirection() { return tappedDirections; }


        public void Enable()
        {
            EnableEvent.Invoke();

            upDirection.SetState(DpadState.normal);
            rightDirection.SetState(DpadState.normal);
            downDirection.SetState(DpadState.normal);
            leftDirection.SetState(DpadState.normal);

            dpadState = DpadState.normal;
        }


        public void Disable()
        {
            DisableEvent.Invoke();

            upDirection.SetState(DpadState.disable);
            rightDirection.SetState(DpadState.disable);
            downDirection.SetState(DpadState.disable);
            leftDirection.SetState(DpadState.disable);

            dpadState = DpadState.disable;
        }

        #endregion
    }
}