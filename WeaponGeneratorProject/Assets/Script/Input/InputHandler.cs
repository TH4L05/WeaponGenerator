using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    #region fields

    [SerializeField] private InputController inputController;
    [SerializeField] private IngameMenu ingameMenu;   
    [SerializeField] private Player player;   
    
    public bool PauseAll { get; set; }

    private GameObject generatorView;
    private WeaponController weaponController;
    private Vector2 arrowAxisInput;
    private bool paused = true;
    private bool zoomed;

    #endregion fields

    #region Setup

    public void SetReferences(Player player, WeaponController weaponController)
    {
        this.player = player;
        this.weaponController = weaponController;
        generatorView = Game.instance.weaponGenerator.generatorView;
    }

    #endregion

    #region UnityFunctions

    void Start()
    {
        paused = false;
    }

    private void FixedUpdate()
    {
        if (PauseAll) return;
        if (paused) return;
        UpdatePlayerValues();
    }

    void Update()
    {
        if (PauseAll) return;
        ToggleIngameMenuOnInput();

        if (paused) return;
        UpdateGeneratorVisbility();
        MouseWheelInput();
        ShootOnInput();
        ZoomOnInput();
        InteractOnInput();
        ReloadOnInput();
        CreateHookOnInput();
        AddForceHookTargets();
        RotateCubeAxisInput();
    }

    #endregion UnityFunctions

    #region InputHandle

    public void ChangeCursorVisibility(bool visible)
    {
        if (visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void UpdatePlayerValues()
    {
        player.UpdateMovementInputValues(
                            inputController.InputAxis,
                            inputController.InputMouse,
                            inputController.JumpInputPressed,
                            inputController.SprintInputPressed
                            );

        if (inputController.JumpInputPressed)
        {
            inputController.JumpInputPressed = false;
        }
    }

    private void ShootOnInput()
    {
        if (inputController.PrimaryFireInputPressed)
        {
            weaponController.ShootActiveWeapon();
        }
        else
        {
            weaponController.ResetActiveWeaponShootTimer();
        }
    }

    private void ZoomOnInput()
    {
        if (inputController.SecondaryFireInputPressed)
        {
            if (!zoomed)
            {
                zoomed = true;
                weaponController.ZoomActiveWeapon(true);
            }
        }
        else
        {
            if (zoomed)
            {
                zoomed = false;
                weaponController.ZoomActiveWeapon(false);
            }
        }
        //inputController.SecondaryFireInputPressed = false;


    }

    void UpdateGeneratorVisbility()
    {
        var visible = inputController.GeneratorVisible;
        if (visible)
        {
            inputController.EnableDisableInputActions("disable", inputController.InputActions);
        }
        else
        {
            inputController.EnableDisableInputActions("enable", inputController.InputActions);
        }

        if (generatorView != null)
            generatorView.gameObject.SetActive(visible);
        ChangeCursorVisibility(visible);
    }

    void MouseWheelInput()
    {
        if (inputController.InputMouseWheel.y == 1)
        {
            weaponController.NextWeapon();
        }
        else if (inputController.InputMouseWheel.y == -1)
        {
            weaponController.PrevoiusWeapon();
        }
    }

    void InteractOnInput()
    {
        if (inputController.InteractInputPressed && Game.instance.player.InteractableIsOnFocus)
        {
            Game.instance.player.ItemOnFocus.Interact();
        }
        inputController.InteractInputPressed = false;
    }

    private void ReloadOnInput()
    {
        if (inputController.ReloadInputPressed)
        {
            weaponController.ReloadWeapon();
        }
        inputController.ReloadInputPressed = false;
    }

    private void CreateHookOnInput()
    {
        if (inputController.CreateHookInputPressed)
        {
            player.CreateHookNode();
        }
        inputController.CreateHookInputPressed = false;
    }

    private void AddForceHookTargets()
    {
        if (inputController.HookAddForceInputPressed)
        {
            player.HookNodesAddForce();
        }
        inputController.HookAddForceInputPressed = false;
    }

    private void RotateCubeAxisInput()
    {
        arrowAxisInput = inputController.InputAxisArrow;

        Vector2 up = new Vector2(0, 1);
        Vector2 down = new Vector2(0, -1);
        Vector2 left = new Vector2(-1, 0);
        Vector2 right = new Vector2(1, 0);

        if (arrowAxisInput == up)
        {
            Debug.Log("UpArrowIsPressed");
            Game.instance.gameRoom.UpArrowIsPressed();
            arrowAxisInput = Vector2.zero;
        }
        else if (arrowAxisInput == down)
        {
            Debug.Log("DownArrowIsPressed");
            Game.instance.gameRoom.DownArrowIsPressed();
            arrowAxisInput = Vector2.zero;
        }
        else if (arrowAxisInput == left)
        {
            Debug.Log("LeftArrowIsPressed");
            Game.instance.gameRoom.LeftArrowIsPressed();
            arrowAxisInput = Vector2.zero;
        }
        else if (arrowAxisInput == right)
        {
            Debug.Log("RightArrowIsPressed");
            Game.instance.gameRoom.RightArrowIsPressed();
            arrowAxisInput = Vector2.zero;
        }
    }

    private void ToggleIngameMenuOnInput()
    {
        if (inputController.InGameInputPressed)
        {
            if (paused)
            {
                paused = false;
                ingameMenu.ToggleMenu();

            }
            else
            {
                paused = true;
                ingameMenu.ToggleMenu();
            }
        }
        inputController.InGameInputPressed = false;
    }

    #endregion InputHandle

    #region InputActionsStatus and Pause

    public void EnableInputActions(bool enable)
    {
        if (enable)
        {
            inputController.EnableDisableInputActions("enable", inputController.InputActions);
        }
        else
        {
            inputController.EnableDisableInputActions("disable", inputController.InputActions);
        }
    }

    public void InputHandlerIsPaused(bool pause)
    {
        paused = pause;
    }

    public void InputHandlerPauseAll(bool pause)
    {
        PauseAll = pause;
    }

    #endregion
}
