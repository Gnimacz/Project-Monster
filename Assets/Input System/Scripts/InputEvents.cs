using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using System;
using UnityEngine.Events;

namespace MonsterInput
{
    public class InputEvents : MonoBehaviour
    {
        public static PlayerInput playerInput;
        public static EventHandler<InputAction.CallbackContext> JumpButton;
        public static bool JumpButtonDown = false;
        public static EventHandler<InputAction.CallbackContext> InteractButton;
        public static bool InteractButtonDown = false;
        public static EventHandler<InputAction.CallbackContext> Move;
        public static EventHandler<InputAction.CallbackContext> Navigate;
        public static EventHandler<InputAction.CallbackContext> Submit;
        public static bool SubmitButtonDown = false;
        public static EventHandler<InputAction.CallbackContext> Cancel;
        public static bool CancelButtonDown = false;

        public static EventHandler<InputAction.CallbackContext> Point;
        public static bool PointButtonDown = false;
        public static EventHandler<InputAction.CallbackContext> Click;
        public static bool LeftMouseDown = false;
        public static EventHandler<InputAction.CallbackContext> ScrollWheel;
        public static EventHandler<InputAction.CallbackContext> MiddleClick;
        public static bool MiddleMouseDown = false;
        public static EventHandler<InputAction.CallbackContext> RightClick;
        public static bool RightMouseDown = false;
        public static EventHandler<InputAction.CallbackContext> TrackedDevicePosition;
        public static EventHandler<InputAction.CallbackContext> TrackedDeviceOrientation;
        public static EventHandler<InputAction.CallbackContext> DeviceLostEvent;
        public static EventHandler<InputAction.CallbackContext> DeviceRegainedEvent;
        public static EventHandler<InputAction.CallbackContext> ControlschangedEvent;
        public static EventHandler<InputAction.CallbackContext> UIInteractButton;
        public static bool UIInteractButtonDown = false;

        private void OnEnable()
        {
            playerInput = FindObjectOfType<PlayerInput>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpButtonDown = context.ReadValueAsButton();
            JumpButton?.Invoke(null, context);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            InteractButtonDown = context.ReadValueAsButton();
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
            SubmitButtonDown = context.ReadValueAsButton();
            Submit?.Invoke(null, context);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            CancelButtonDown = context.ReadValueAsButton();
            Cancel?.Invoke(null, context);
        }

        public void OnPoint(InputAction.CallbackContext context)
        {
            Point?.Invoke(null, context);
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            LeftMouseDown = context.ReadValueAsButton();
            Click?.Invoke(null, context);
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            ScrollWheel?.Invoke(null, context);
        }

        public void OnMiddleClick(InputAction.CallbackContext context)
        {
            MiddleMouseDown = context.ReadValueAsButton();
            MiddleClick?.Invoke(null, context);
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            RightMouseDown = context.ReadValueAsButton();
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

        public void OnUIInteractButton(InputAction.CallbackContext context)
        {
            UIInteractButtonDown = context.ReadValueAsButton();
            UIInteractButton?.Invoke(null, context);
        }

    }
}