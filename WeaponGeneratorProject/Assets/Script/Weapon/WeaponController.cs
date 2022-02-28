using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject[] weaponSlots;
    public int activeWeaponIndex;
    bool canSwitchWeapon = true;

    public Action<WeaponData, Sprite> ActiveWeaponChanged;
    public Action<int> AmmoValueChanged;

    private void Start()
    {
        var weapon = weaponSlots[activeWeaponIndex].gameObject.GetComponentInChildren<Weapon>();
        UpdateActiveWeaponUI();
        UpdateAmmoUI(weapon.Data.ammoAmount);
    }

    public void UpdateActiveWeaponUI()
    {
        var weapon = weaponSlots[activeWeaponIndex].gameObject.GetComponentInChildren<Weapon>();
        ActiveWeaponChanged?.Invoke(weapon.Data, weapon.Icon);
    }

    public void UpdateAmmoUI(int amount)
    {      
        AmmoValueChanged?.Invoke(amount);
    }

    public void EquipWeapon(GameObject weapon)
    {
        //Destroy(weaponSlots[activeWeaponIndex]);
        DropWeapon();
        weaponSlots[activeWeaponIndex] = weapon;

        var particle = weapon.GetComponentInChildren<Weapon>();
        particle.SetRarityParticle(false);

        weapon.transform.parent = transform;       
        weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        weapon.transform.localScale = new Vector3(1, 1, 1);
        weapon.GetComponent<Animator>().enabled = true;

        var weaponRB = weapon.GetComponent<Rigidbody>();
        weaponRB.isKinematic = true;

        SetObjectLayer(weapon, 6);
        UpdateActiveWeaponUI();
    }

    public void DropWeapon()
    {
        var weapon = weaponSlots[activeWeaponIndex].gameObject;
        var particle = weapon.GetComponentInChildren<Weapon>();
        particle.SetRarityParticle(true);

        var obj = GameObject.Find("ItemsInScene");
        weapon.transform.parent = obj.transform;
        weapon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
        weapon.transform.localScale = new Vector3(1, 1, 1);
        weapon.GetComponent<Animator>().enabled = false;
        SetObjectLayer(weapon, 0);

        var weaponRB = weapon.GetComponent<Rigidbody>();
        weaponRB.isKinematic = false;
        weaponRB.AddForce(weapon.transform.up.normalized * 3f, ForceMode.VelocityChange);
    }


    public void ShootActiveWeapon()
    {
        var weapon = weaponSlots[activeWeaponIndex].GetComponentInChildren<Weapon>();
        weapon.Shoot();
    }

    public void ResetActiveWeaponShootTimer()
    {
        var weapon = weaponSlots[activeWeaponIndex].GetComponentInChildren<Weapon>();
        weapon.ResetShootTimer();
    }

    public void ZoomActiveWeapon(bool zoom)
    {
        if (zoom)
        {
            canSwitchWeapon = false;
        }
        else
        {
            canSwitchWeapon = true;
        }


        Debug.Log(zoom);
        var weapon = weaponSlots[activeWeaponIndex].GetComponentInChildren<Weapon>();
        weapon.Zoom(zoom);
    }

    private void SetObjectLayer(GameObject weapon, int layer)
    {
        weapon.layer = layer;

        foreach (Transform child in weapon.transform)
        {
            SetObjectLayer(child.gameObject, layer);
        }
    }

    public void NextWeapon()
    {
        weaponSlots[activeWeaponIndex].gameObject.SetActive(false);
        activeWeaponIndex++;
        if (activeWeaponIndex > weaponSlots.Length -1) activeWeaponIndex = 0;
        weaponSlots[activeWeaponIndex].gameObject.SetActive(true);
        UpdateActiveWeaponUI();
    }

    public void PrevoiusWeapon()
    {
        if (!canSwitchWeapon) return;

        weaponSlots[activeWeaponIndex].gameObject.SetActive(false);
        activeWeaponIndex--;
        if (activeWeaponIndex < 0) activeWeaponIndex = weaponSlots.Length -1;
        weaponSlots[activeWeaponIndex].gameObject.SetActive(true);
        UpdateActiveWeaponUI();
    }

    public void ReloadWeapon()
    {
        if (!canSwitchWeapon) return;

        var weapon = weaponSlots[activeWeaponIndex].gameObject.GetComponentInChildren<Weapon>();
        weapon.Reload();
    }

}
