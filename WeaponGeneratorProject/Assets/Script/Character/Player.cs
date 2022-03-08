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

    #region UnityFunctions

    private void OnDestroy()
    {
        GameEvents.UpdatePlayerExperience -= UpdateExp;
        Weapon.ZoomCamera -= ZoomCamera;
    }

    private void LateUpdate()
    {
        DrawInteractableRayCast();
    }

    #endregion

    protected override void StartSetup()
    {
        if (levelSystem != null) levelSystem.LevelUP += LevelUp;
        GameEvents.UpdatePlayerExperience += UpdateExp;
        Weapon.ZoomCamera += ZoomCamera;
        data.level = 1;
        base.StartSetup();
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
                GameEvents.ShowInfoText?.Invoke(false, ""); 
                GameEvents.ShowWeaponInfo?.Invoke(false, null);
                return;
            }
            
            if(ItemOnFocus.Data.IsInteractable)
            {
                InteractableIsOnFocus = true;
                GameEvents.ShowInfoText?.Invoke(true, ItemOnFocus.Data.InfoText);

                if (ItemOnFocus.Data.ItemType == ItemType.Weapon)
                {
                    GameEvents.ShowWeaponInfo?.Invoke(true, ItemOnFocus.gameObject);
                }               
                return;
            }           
        }
        else
        {
            InteractableIsOnFocus = false;
            ItemOnFocus = null;
            GameEvents.ShowWeaponInfo?.Invoke(false, null);
            GameEvents.ShowInfoText?.Invoke(false, "");           
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
