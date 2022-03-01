using System;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{
    [Header("References")]
    public WeaponController weaponController;
    public GeneratorData generatorData;
    private WeaponDataRandom randData;
    public GameObject generatorView;

    [Header("GeneratorOptions")]

    public bool generateFromRandomData;
    public bool generateFromDefinedData;
    public List<Slot> generatorSlots = new List<Slot>();

    [Header("Temp")]
    public List<GameObject> tempweaponlist;

    private void Start()
    {
        tempweaponlist = new List<GameObject>();
        
    }

    public void GenerateOnClick()
    {
        for (int i = 0; i < tempweaponlist.Count; i++)
        {
            DestroyTempWeapon(i);
        }

        ClearTempList();

        for (int x = 0; x < generatorSlots.Count; x++)
        {
            generatorSlots[x].ResetSlot();
            GenerateNewRandomWeapon();
            var weapon = tempweaponlist[x].GetComponentInChildren<Weapon>();
            UpdateGeneratorSlot(x, weapon);
        }
    }

    public void GenerateNewRandomWeapon()
    {
        //Set Random WeaponType       
        randData = new WeaponDataRandom(generatorData.rarityTable);
        var type = randData.GetRandomType();
 
        //Instancicate WeaponPrefab
        GameObject newWeapon = null;
        var obj = GameObject.Find("ItemsInScene");
        
        try
        {
            newWeapon = Instantiate(generatorData.weaponPrefabs[(int)type]);
            Debug.Log(newWeapon);
        }
        catch (Exception)
        {
            Debug.Log("Can not create a new Weapon");
            return;
        }

        if (obj == null)
        {
            GameObject itemsInScene = new GameObject("ItemsInScene");
            newWeapon.transform.parent = itemsInScene.transform;
        }
        else
        {
            newWeapon.transform.parent = obj.transform;
        }


        //Weapon Setup
        var weapon = newWeapon.GetComponentInChildren<Weapon>();
        var newWeaponData = randData.NewRandomData(type);

        //Set Rarity color
        newWeaponData.rarityColor = generatorData.frameColors[(int)newWeaponData.rarityValue];

        weapon.SetData(newWeaponData);

        //Set Rarity Particle
        weapon.SetRarityParticle(true);

        //Set GameObject Name
        newWeapon.name = weapon.Data.rarityValue.ToString() + weapon.Data.type.ToString() + "-" + weapon.Data.name;

        //Temp
        tempweaponlist.Add(newWeapon);
        randData = null;
    }

    public void UpdateGeneratorSlot(int indx, Weapon weapon)
    {
        generatorSlots[indx].UpdateSlot(weapon.Data, weapon.Icon);
    }

    public void DestroyTempWeapon(int indx)
    {
        try
        {
            Destroy(tempweaponlist[indx].gameObject);
        }
        catch (Exception)
        {
            return;
        }
        Debug.Log($"Weapon in slot {indx} destroyed ");
    }

    public void ResetGeneratorSlot(int indx)
    {
        generatorSlots[indx].ResetSlot();
    }

    public void AddToWeaponController(int indx)
    {
        tempweaponlist[indx].GetComponent<Rigidbody>().isKinematic = true;
        weaponController.EquipWeapon(tempweaponlist[indx]);
    }

    public GameObject GetGeneratedWeapon(int indx)
    {
        return tempweaponlist[indx];
    }

    public void ClearTempList()
    {
        tempweaponlist.Clear();
    }
}
