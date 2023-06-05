using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;

namespace MonsterInput
{
    public class InputEvents : MonoBehaviour
    {
        public static PlayerInput playerInput;
        public static EventHandler<InputAction.CallbackContext> JumpButton;
        public static EventHandler<InputAction.CallbackContext> InteractButton;
        public static EventHandler<InputAction.CallbackContext> Move;
        public static EventHandler<InputAction.CallbackContext> Navigate;
        public static EventHandler<InputAction.CallbackContext> Submit;
        public static EventHandler<InputAction.CallbackContext> Cancel;
        public static EventHandler<InputAction.CallbackContext> Point;
        public static EventHandler<InputAction.CallbackContext> Click;
        public static EventHandler<InputAction.CallbackContext> ScrollWheel;
        public static EventHandler<InputAction.CallbackContext> MiddleClick;
        public static EventHandler<InputAction.CallbackContext> RightClick;
        public static EventHandler<InputAction.CallbackContext> TrackedDevicePosition;
        public static EventHandler<InputAction.CallbackContext> TrackedDeviceOrientation;
        public static EventHandler<InputAction.CallbackContext> DeviceLostEvent;
        public static EventHandler<InputAction.CallbackContext> DeviceRegainedEvent;
        public static EventHandler<InputAction.CallbackContext> ControlschangedEvent;

        private void OnEnable()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpButton?.Invoke(null, context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            InteractButton?.Invoke(null, context);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(null, context);
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Navigate?.Invoke(null, context);
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            Submit?.Invoke(null, context);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            Cancel?.Invoke(null, context);
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            Point?.Invoke(null, context);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            Click?.Invoke(null, context);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            ScrollWheel?.Invoke(null, context);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            MiddleClick?.Invoke(null, context);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            RightClick?.Invoke(null, context);
        }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context)
        {
            TrackedDevicePosition?.Invoke(null, context);
        }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
        {
            TrackedDeviceOrientation?.Invoke(null, context);
        }

        public static void OnDeviceLostEvent(InputAction.CallbackContext context)
        {
            DeviceLostEvent?.Invoke(null, context);
        }

        public static void OnDeviceRegainedEvent(InputAction.CallbackContext context)
        {
            DeviceRegainedEvent?.Invoke(null, context);
        }

        public static void OnControlschangedEvent(InputAction.CallbackContext context)
        {
            ControlschangedEvent?.Invoke(null, context);
        }

    }
}