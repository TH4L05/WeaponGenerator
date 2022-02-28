using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region private fields

    private Controls playerControls;
    private List<InputAction> inputActions;
    private List<InputAction> inputActionsUI;
    private List<InputAction> inputActionsOther;

    private Vector2 inputAxis;
    private Vector2 inputMouse;
    private Vector2 inputMouseWheel;

    private Vector2 inputAxisArrow;

    #endregion private fields

    #region fields

    public List<InputAction> InputActions => inputActions;
    public List<InputAction> InputActionsOther => inputActionsOther;

    public Vector2 InputAxis => inputAxis;
    public Vector2 InputMouse => inputMouse;
    public Vector2 InputMouseWheel => inputMouseWheel;
    public Vector2 InputAxisArrow => inputAxisArrow;

    public bool PrimaryFireInputPressed { get; private set; }
    public bool SecondaryFireInputPressed { get; set; }
    public bool SprintInputPressed { get; private set; }
    public bool CrouchInputPressed { get; private set; }
    public bool JumpInputPressed { get; set; }
    public bool InteractInputPressed { get; set; }
    public bool ReloadInputPressed { get; set; }
    public bool CreateHookInputPressed { get; set; }
    public bool HookAddForceInputPressed { get; set; }
    public bool InGameInputPressed { get; set; }


    bool generatorVisible;
    public bool GeneratorVisible => generatorVisible;

    #endregion fields

    #region UnityFunctions

    private void Awake()
    {
        playerControls = new Controls();
        inputActions = new List<InputAction>();
        inputActionsUI = new List<InputAction>();
        inputActionsOther = new List<InputAction>();
    }

    private void Start()
    {
        SetInputActions();
    }

    private void OnDestroy()
    {
        EnableDisableInputActions("disable", inputActions);
        EnableDisableInputActions("disable", inputActionsOther);
        //EnableDisableInput("disable", inputActionsUI);   
    }

    #endregion UnityFunctions

    #region Setup

    private void SetInputActions()
    {
        var input = playerControls.Movement;
        var inputOther = playerControls.Other;
        var inputUI = playerControls.UI;

        input.Movement.performed += AxisMovementInput;
        inputActions.Add(input.Movement);

        input.Mouse.performed += MousePositionInput;
        inputActions.Add(input.Mouse);

        input.Jump.performed += JumpInputIsPressed;
        inputActions.Add(input.Jump);

        input.PrimaryFire.performed += PrimaryFireInputIsPressed;
        input.PrimaryFire.canceled += PrimaryFireInputIsReleased;
        inputActions.Add(input.PrimaryFire);

        input.SecondaryFire.performed += SecondaryFireInputIsPressed;
        //input.SecondaryFire.canceled += SecondaryyFireInputIsReleased;
        inputActions.Add(input.SecondaryFire);

        input.Sprint.performed += SprintInputIsPressed;
        input.Sprint.canceled += SprintInputIsPressed;
        inputActions.Add(input.Crouch);

        input.Crouch.performed += CrouchInputIsPressed;
        input.Crouch.canceled += CrouchInputIsPressed;
        inputActions.Add(input.Sprint);

        input.MouseWheel.performed += MouseWheelInput; ;
        inputActions.Add(input.MouseWheel);

        input.Reload.performed += ReloadInputIsPressed;
        //input.Reload.canceled += ReloadInputIsPressed;
        inputActions.Add(input.Reload);

        inputOther.Generator.performed += GeneratorInputIsPressed;
        inputActionsOther.Add(inputOther.Generator);

        inputOther.Interact.performed += InteractInputIsPressed;
        //inputOther.Interact.canceled += InteractInputIsPressed;
        inputActionsOther.Add(inputOther.Interact);

        inputOther.CreateHookNode.performed += CreateHookNodeInputIsPressed;
        inputActionsOther.Add(inputOther.CreateHookNode);

        inputOther.AddForceToHookedObjects.performed += AddForceToHookedObjecsInputIsPressed;
        inputActionsOther.Add(inputOther.AddForceToHookedObjects);

        inputOther.RotateCube.performed += ArrowAxisInput;
        inputActionsOther.Add(inputOther.RotateCube);

        inputOther.ToggleIngameMenu.performed += ToggleInGameMenuInput;
        inputActionsOther.Add(inputOther.ToggleIngameMenu);

        EnableDisableInputActions("enable", inputActions);
        EnableDisableInputActions("enable", inputActionsOther);
        //EnableDisableInput("enable", inputActionsUI);
    }

    #endregion Setup

    #region EnableDisable

    public void EnableMovmentInput(int i)
    {

        inputActions[i].Enable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void DisableMovementInput(int i)
    {
        inputActions[i].Disable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void EnableOtherInput(int i)
    {

        inputActionsOther[i].Enable();
        //Debug.Log(inputActions[i].enabled);
    }

    public void DisableOtherInput(int i)
    {
        inputActionsOther[i].Disable();
        //Debug.Log(inputActions[i].enabled);
    }


    public void EnableDisableInputActions(string command, List<InputAction> inputList)
    {
        if (command == "enable")
        {
            foreach (var action in inputList)
            {
                action.Enable();
            }
        }
        else if (command == "disable")
        {
            foreach (var action in inputList)
            {
                action.Disable();
            }
        }
    }

    public void EnableDisableSingleInputAction(string command, InputAction action, List<InputAction> inputList)
    {
        foreach (var inputAction in inputList)
        {
            if (inputAction == action)
            {
                if (command == "enable")
                {
                    inputAction.Enable();
                }
                else if (command == "disable")
                {
                    inputAction.Disable();
                }
                return;
            }
        }
    }

    #endregion EnableDisable

    #region Input

    private void MousePositionInput(InputAction.CallbackContext inputcontext)
    {
        inputMouse = inputcontext.ReadValue<Vector2>();
    }

    private void AxisMovementInput(InputAction.CallbackContext inputcontext)
    {
        inputAxis = inputcontext.ReadValue<Vector2>();
    }

    private void MouseWheelInput(InputAction.CallbackContext inputcontext)
    {
        inputMouseWheel = inputcontext.ReadValue<Vector2>();
    }

    private void GeneratorInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>GeneratorVisibleChange</color>");
        generatorVisible = !generatorVisible;
    }

    private void JumpInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>JumpInputIsPressed</color>");
        JumpInputPressed = true;
    }

    private void PrimaryFireInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>ShootInputIsPressed</color>");
        PrimaryFireInputPressed = true;
    }

    private void PrimaryFireInputIsReleased(InputAction.CallbackContext inputcontext)
    {
        PrimaryFireInputPressed = false;
    }

    private void SecondaryFireInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        Debug.Log("<color=aqua>SecondaryFireInputIsPressed</color>");
        //SecondaryFireInputPressed = true;
        SecondaryFireInputPressed = !SecondaryFireInputPressed;
    }


    private void CrouchInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>CrouchInputIsPressed</color>");
        CrouchInputPressed = !CrouchInputPressed;
    }

    private void SprintInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>SprintInputIsPressed</color>");
        SprintInputPressed = !SprintInputPressed;
    }

    private void InteractInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>InteractInputIsPressed</color>");
        InteractInputPressed = !InteractInputPressed;
    }

    private void ReloadInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        Debug.Log("<color=aqua>ReloadInputIsPressed</color>");
        ReloadInputPressed = true;
    }

    private void CreateHookNodeInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>CreateHookNodeInputIsPressed</color>");
        CreateHookInputPressed = true;
    }

    private void AddForceToHookedObjecsInputIsPressed(InputAction.CallbackContext inputcontext)
    {
        //Debug.Log("<color=aqua>AddForceToHookedObjecsInputIsPressed</color>");
        HookAddForceInputPressed = true;
        
    }

    private void ToggleInGameMenuInput(InputAction.CallbackContext context)
    {
        InGameInputPressed = !InGameInputPressed;
    }

    private void ArrowAxisInput(InputAction.CallbackContext inputcontext)
    {
        inputAxisArrow = inputcontext.ReadValue<Vector2>();
    }

    #endregion Input
}
