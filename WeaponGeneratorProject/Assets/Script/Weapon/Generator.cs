using System;
using System.Collections.Generic;
using UnityEngine;


public class Generator : MonoBehaviour
{
    [Header("References")]
    public WeaponController weaponController;
    public GeneratorData data;
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
        randData = new WeaponDataRandom();
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
        var type = randData.GetRandomType();
 
        //Instancicate WeaponPrefab
        GameObject newWeapon = null;
        var obj = GameObject.Find("ItemsInScene");
        
        try
        {
            newWeapon = Instantiate(data.weaponPrefabs[(int)type]);
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
        weapon.NewWeaponDataInstance();
        weapon.Data.Setup(type, data.rarityTable);

        //Set Rarity color
        weapon.Data.rarityColor = data.frameColors[(int)weapon.Data.rarityValue];

        //Set Rarity Particle
        weapon.SetRarityParticle(true);

        //Set GameObject Name
        newWeapon.name = weapon.Data.rarityValue.ToString() + weapon.Data.type.ToString() + "-" + weapon.Data.name;

        //Temp
        tempweaponlist.Add(newWeapon);

        
    }

    public void GenerateNewWeaponWithData()
    {
        //Set Random WeaponType
        var type = randData.GetRandomType();

        //Instancicate WeaponPrefab
        GameObject newWeapon = null;
        try
        {
            newWeapon = Instantiate(data.weaponPrefabs[(int)type]);
        }
        catch (Exception)
        {
            Debug.Log("Impossible to create Weapon");
        }

        //Weapon Setup
        var weapon = newWeapon.GetComponentInChildren<Weapon>();
        weapon.NewWeaponDataInstance();
        
        //Set Rarity color
        weapon.Data.rarityColor = data.frameColors[(int)weapon.Data.rarityValue];

        //Set Rarity particle
        weapon.RarityParticles[(int)weapon.Data.rarityValue].SetActive(true);

        //Set GameObject Name
        newWeapon.name = weapon.Data.rarityValue.ToString() + weapon.Data.type.ToString() + "-" + weapon.Data.name;

        //Temp
        tempweaponlist.Add(newWeapon);
    }

    public void UpdateGeneratorSlot(int indx, Weapon weapon)
    {
        generatorSlots[indx].UpdateSlot(weapon.Data, weapon.Icon, weapon.Frame);
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
