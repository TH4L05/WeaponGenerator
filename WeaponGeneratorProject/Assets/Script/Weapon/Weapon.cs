using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon :MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private WeaponData data;
    [SerializeField] private Sprite icon;
    [SerializeField] private Sprite frame;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform raycastStart;
    [SerializeField] private AudioEventList weaponAudioEvents;

    [Header("VFX")]
    [SerializeField] private GameObject impactVFX;
    [SerializeField] private ParticleSystem muzzleflash;
    [SerializeField] private List<GameObject> rarityParticles = new List<GameObject>();
    

    #endregion

    #region Fields

    public WeaponData Data => data;
    public Sprite Icon => icon;
    public Sprite Frame => frame;
    public List<GameObject> RarityParticles => rarityParticles;

    private bool canShoot = true;
    private float raycastRange = 9999.0f;                         
    private float shootTimer;
    private bool onReload;

    #endregion

    #region Events

    public static Action EnableCrosshair;
    public static Action DisableCrosshair;
    public static Action<bool> ZoomCamera;
    
    #endregion

    #region UnityFunctions

    private void Start()
    {
        shootTimer = 0f;
        data.ammoAmount = data.ammoCapacity;      
    }

    private void Update()
    {
        Debug.DrawRay(raycastStart.position, raycastStart.TransformDirection(Vector3.forward) * 300, Color.red);
        shootTimer += Time.deltaTime;
    }

    #endregion UnityFunctions

    public void SetRarityParticle(bool active)
    {
        int indx = (int)data.rarityValue;
        rarityParticles[indx].SetActive(active);
    }

    public void NewWeaponDataInstance()
    {
        data = ScriptableObject.CreateInstance<WeaponData>();
    }

    public void UpdateAmmoAmount()
    {
        if (data.ammoAmount != 0)
        {
            data.ammoAmount--;
            Game.instance.player.WeaponController.UpdateAmmoUI(data.ammoAmount);
        }
    }

    #region Zoom

    public void Zoom(bool zoom)
    {
        if (zoom)
        {
            if (data.type == WeaponType.SniperRifle)
            {
                ZoomCamera?.Invoke(zoom);
            }
            animator.SetTrigger("zoomIn");
            DisableCrosshair?.Invoke();
        }
        else
        {
            if (data.type == WeaponType.SniperRifle)
            {
                ZoomCamera?.Invoke(zoom);
            }
            animator.SetTrigger("zoomOut");
            EnableCrosshair?.Invoke();
        }
    }

    #endregion

    #region Reload

    public void Reload()
    {
        Debug.Log("RELOAD");
        if (data.ammoAmount == data.ammoCapacity) return;
        if (onReload) return;
        onReload = true;

        if (weaponAudioEvents != null) weaponAudioEvents.PlayAudioEvent("Reload");
        StartCoroutine(ReloadOnTime());
    }

    IEnumerator ReloadOnTime()
    {
        yield return new WaitForSeconds(data.reloadTime);
        data.ammoAmount = data.ammoCapacity;
        Game.instance.player.WeaponController.UpdateAmmoUI(data.ammoAmount);
        onReload = false;
    }

    #endregion

    #region Shooting

    public void ResetShootTimer()
    {
        var rateOfFire = 1 / (data.rateOfFire / 60);
        shootTimer = rateOfFire;
        canShoot = true;
    }

    public void Shoot()
    {
        if(onReload) return;

        var rateOfFire = 1 / (data.rateOfFire / 60);

        if (data.ammoAmount < 1) return;

        if (canShoot && shootTimer >= rateOfFire)
        {
            shootTimer = 0.0f;

            switch (data.sType)
            {
                case ShootType.Invalid:
                default:
                    Debug.Log("ERROR: Invalid shoot type !!");
                    return;
                case ShootType.Raycast:
                    RaycastShoot();
                    UpdateAmmoAmount();
                    break;
                case ShootType.Projectile:
                    ProjectileShoot();
                    UpdateAmmoAmount();
                    break;
                case ShootType.Beam:
                    break;
            }

            if (weaponAudioEvents != null) weaponAudioEvents.PlayAudioEvent("Shoot");
            if (animator != null) animator.SetTrigger("shoot");
            if (muzzleflash != null) muzzleflash.Play();
        }
    }

    private void RaycastShoot()
    {
        Vector3 direction = raycastStart.forward;
        Ray ray = new Ray(raycastStart.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            Debug.Log($"<color=lime>RaycastShot hit = {hit.collider.name}</color>");
            NewImpactVfx(hit);
            DamageTargetOnHit(hit);

            if (hit.collider.CompareTag("ShootTarget"))
            {
                var target = hit.collider.GetComponent<Target>();
                target.TargetGetHit();
            }
        }        
    }

    private void ProjectileShoot()
    {
        //var newProjectile = Instantiate(projectile, projectileSpawn.position, Game.instance.playerPivot.transform.rotation);
        //newProjectile.GetComponent<Projectile>().Speed = 50;
    }

    #endregion

    #region Damage and VFX

    private void DamageTargetOnHit(RaycastHit hit)
    {
        var damageableTarget = hit.collider.GetComponent<IDamageable>();
        if (damageableTarget == null) return;
        var damage = Mathf.Lerp(data.damageMin, data.damageMax, data.accuracy);
        damageableTarget.TakeDamage(damage);       
    }

    private void NewImpactVfx(RaycastHit hit)
    {
        if (impactVFX == null) return;
        var vfx = Instantiate(impactVFX, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        Destroy(vfx, 2.5f);
    }

    #endregion
}
