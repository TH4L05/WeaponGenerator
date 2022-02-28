using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    #region Fields

    [Header("References")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private HookSystem hookSystem;
    [SerializeField] private LevelSystem levelSystem;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private Camera cameraMain;
    [SerializeField] private Transform RaySpawn;
    [SerializeField] private Animator animatorCamera;
    public LayerMask ignoreLayer;

    public Camera CameraMain => cameraMain;
    public bool InteractableIsOnFocus { get; private set; }
    public Item ItemOnFocus { get; private set; }
    public int Level => data.level;
    public LevelSystem LevelSystem => levelSystem;
    public HookSystem HookSystem => hookSystem;
    public WeaponController WeaponController => weaponController;

    #endregion

    protected override void StartSetup()
    {
        if (levelSystem != null) levelSystem.LevelUP += LevelUp;
        GameEvents.UpdatePlayerExperience += UpdateExp;
        Weapon.ZoomCamera += ZoomCamera;
        data.level = 1;
        base.StartSetup();
    }

    private void OnDestroy()
    {
        GameEvents.UpdatePlayerExperience -= UpdateExp;
        Weapon.ZoomCamera -= ZoomCamera;
    }

    private void ZoomCamera(bool zoom)
    {
        if (zoom)
        {
            if (animatorCamera != null) animatorCamera.Play("ZoomIn");
        }
        else
        {
            if (animatorCamera != null) animatorCamera.Play("ZoomOut");
        }
    }

    private void LateUpdate()
    {
        DrawInteractableRayCast();
    }

    public void DrawInteractableRayCast()
    {
        Vector3 rayOrigin = RaySpawn.position;
        Vector3 rayDirection = RaySpawn.forward;        
        Ray ray = new Ray(rayOrigin, rayDirection);
        float rayDistanceMax = 3f;
        RaycastHit hit;
        Color rayColor = Color.blue;

        Debug.DrawRay(rayOrigin, rayDirection * rayDistanceMax, rayColor);
        if (Physics.Raycast(ray, out hit, rayDistanceMax,~ignoreLayer))
        {
            //Debug.Log($"<color=teal>RaycastPlayer hit = {hit.collider.tag} + {hit.collider.name}</color>");

            ItemOnFocus = hit.collider.GetComponent<Item>();

            if (ItemOnFocus == null)
            {
                InteractableIsOnFocus = false;
                GameEvents.ShowInfoText?.Invoke(false, 0); 
                GameEvents.ShowWeaponInfo?.Invoke(false, null);
                return;
            }
            else if(!ItemOnFocus.Data.isInteractable)
            {
                InteractableIsOnFocus = false;
                GameEvents.ShowInfoText?.Invoke(false, 0);
                GameEvents.ShowWeaponInfo?.Invoke(false, null);
                return;
            }


            switch (ItemOnFocus.Data.ItemType)
            {
                case ItemType.Default:
                    InteractableIsOnFocus = true;
                    GameEvents.ShowInfoText?.Invoke(true, 1);
                    break;

                case ItemType.Button:
                    InteractableIsOnFocus = true;
                    GameEvents.ShowInfoText?.Invoke(true, 3);
                    break;

                case ItemType.Money:
                    InteractableIsOnFocus = true;
                    break;

                case ItemType.Weapon:
                    InteractableIsOnFocus = true;
                    var weapon = hit.collider.GetComponentInChildren<Weapon>();
                    GameEvents.ShowInfoText?.Invoke(true, 2);                  
                    GameEvents.ShowWeaponInfo?.Invoke(true, ItemOnFocus.gameObject);
                    break;

                default:
                    InteractableIsOnFocus = false;
                    ItemOnFocus = null;
                    GameEvents.ShowInfoText?.Invoke(false, 0);
                    GameEvents.ShowWeaponInfo?.Invoke(false, null);
                    break;
            }
        }
        else
        {
            InteractableIsOnFocus = false;
            ItemOnFocus = null;
            GameEvents.ShowWeaponInfo?.Invoke(false, null);
            GameEvents.ShowInfoText?.Invoke(false, 0);           
        }
    }

    public void UpdateMovementInputValues(Vector2 input, Vector2 rotation, bool jumpPressed, bool sprintPressed)
    {
        movement.UpdateValues(input, rotation, jumpPressed, sprintPressed);
    }

    private void UpdateExp(int amount)
    {
        levelSystem.AddExperience(amount);
    }

    private void LevelUp()
    {
        data.level = levelSystem.Level;
    }

    public override void Death()
    {
        if (levelSystem != null) levelSystem.LevelUP -= LevelUp;
        GameEvents.UpdatePlayerExperience -= UpdateExp;
        base.Death();
    }


    #region HookSystemTest

    public void CreateHookNode()
    {
        hookSystem.CreateHookNode();
    }

    public void HookNodesAddForce()
    {
        hookSystem.AddForceToHookedObjects();
    }

    public void HandleHookShotMovement(Transform hookNode, float forcceMultiplier)
    {
        movement.HookShotMove(hookNode, forcceMultiplier);
    }

    #endregion
}
