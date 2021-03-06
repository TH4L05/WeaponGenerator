using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Invalid = -1,
    Pistol,
    Shotgun,
    Rifle,
    SniperRifle,
}

public enum Rarity
{
    Invalid = -1,
    Common,
    Rare,
    Unique,
    Legendary
}

public enum ShootType
{
    Invalid = -1,
    Raycast,
    Projectile,
    Beam
}

[CreateAssetMenu(fileName = "New WeaponData", menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject
{
    public new string name;
    public int level;
    public WeaponType type;
    public ShootType sType;
    public Rarity rarityValue;
    public float damageMin;
    public float damageMax;
    public float rateOfFire;
    public float recoil;
    public int ammoCapacity;
    public int ammoAmount;
    public float accuracy;
    public float accuracyDropPerShot;
    public float accuracyRecoverRate;
    public float reloadTime;
    public Color rarityColor;

    /// <summary>
    /// set data with predefined WeaponData
    /// </summary>
    /// <param name="data"></param>
    public void SetData(WeaponData data)
    {
        name = data.name;
        level = data.level;
        type = data.type;
        sType = data.sType;
        rarityValue = data.rarityValue;
        damageMin = data.damageMin;
        damageMax = data.damageMax;
        rateOfFire = data.rateOfFire;
    }

    /// <summary>
    /// set data with manuell WeaponData
    /// </summary>
    /// <param name="name"></param>
    /// <param name="icon"></param>
    /// <param name="frame"></param>
    /// <param name="type"></param>
    /// <param name="rarity"></param>
    /// <param name="damamgeMin"></param>
    /// <param name="damamgeMax"></param>
    /// <param name="speed"></param>
    public void SetData(string name, WeaponType type, ShootType stype, Rarity rarity, float damageMin, float damageMax, float rateOfFire)
    {        
        this.name = name;
        this.type = type;
        sType = stype;
        rarityValue = rarity;
        this.damageMin = damageMin;
        this.damageMax = damageMax;
        this.rateOfFire = rateOfFire;
    }
}