using System.Collections.Generic;
using UnityEngine;

public class WeaponDataRandom
{
    private int[] rarityTable;

    public WeaponDataRandom()
    {
    }

    public WeaponDataRandom(int[] _rarityTable)
    {
        rarityTable = _rarityTable;
    }

    public WeaponData NewRandomData(WeaponType type)
    {
        var data = ScriptableObject.CreateInstance<WeaponData>();

        data.name = SetRandomName();
        data.level = SetLevel();
        data.type = type;
        data.sType = SetShootType(type);
        data.rarityValue = SetRandomRarity();
        var damage = SetRandomDamage(data.level);
        data.damageMin = damage.Item1;
        data.damageMax = damage.Item2;
        data.rateOfFire = SetRandomSpeed(data.type);
        var accuracy = SetRandomAccuracyValues(data.rarityValue);
        data.accuracy = accuracy.Item1;
        data.accuracyDropPerShot = accuracy.Item2;
        data.accuracyRecoverRate = accuracy.Item3;
        data.ammoCapacity = SetAmmoCapacity(data.type, data.rarityValue);
        data.ammoAmount = data.ammoCapacity;

        return data;
    }

    private int SetLevel()
    {
        var playerLvl = Game.instance.player.GetComponent<Player>().Level;
        var lvl = Random.Range(playerLvl - 2, playerLvl + 2);

        if (lvl < 1)
        {
            lvl = 1;
        }

        return lvl;
    }

    private string SetRandomName()
    {
        char c1 = (char)('A' + Random.Range(0, 26));
        char c2 = (char)('A' + Random.Range(0, 26));
        int number = Random.Range(1, 999);
        var name = (c1.ToString() + c2.ToString() + number.ToString("000"));
        return name;
    }

    private (float, float) SetRandomDamage(int level)
    {
        float randMin = (level * 5) + level;
        float randMax = randMin * (level * 0.8f);

        var max = Random.Range(randMin, randMax);
        var min = max * 0.6f;
        return (min, max);
    }

    private float SetRandomSpeed(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Pistol:
                return Random.Range(25f, 50);
            case WeaponType.Shotgun:
                return Random.Range(25f, 65f);
            case WeaponType.Rifle:
                return Random.Range(50f, 200f);
            case WeaponType.SniperRifle:
                return Random.Range(15f, 35f);
            default:
                return 15;
        }   
    }

    public WeaponType GetRandomType()
    {
        var typeEnum = System.Enum.GetValues(typeof(WeaponType));
        var randValue = Random.Range(0, typeEnum.Length -1);
        return (WeaponType)randValue;
    }

    private Rarity SetRandomRarity()
    {
        int total = 0;

        foreach (var value in rarityTable)
        {
            total += value;
        }

        var randNumber = Random.Range(0, total);

        for (int i = 0; i < rarityTable.Length; i++)
        {
            if (randNumber <= rarityTable[i])
            {            
                 return (Rarity)i;
            }
            else
            {
                randNumber -= rarityTable[i];
            }
        }

        return Rarity.Common;
    }

    private (float, float, float) SetRandomAccuracyValues(Rarity rarity)
    {
        float min = 0f;
        float max = 1f;

        float accuracy = 100f;
        float accuracydrop = 1f;
        float accuracyRevovery = 10f;

        switch (rarity)
        {
            case Rarity.Common:
            default:
                min = 1.0f;
                max = 3.0f;

                accuracy = Random.Range(40f, 99f);
                accuracydrop = Random.Range(min, max);
                accuracyRevovery = Random.Range(1f, 10f);
                return (accuracy, accuracydrop, accuracyRevovery); 
                
            case Rarity.Rare:
                min = 1.0f;
                max = 2.0f;

                accuracy = Random.Range(65f, 99f);
                accuracydrop = Random.Range(min, max);
                accuracyRevovery = Random.Range(10f, 15f);
                return (accuracy, accuracydrop, accuracyRevovery);

            case Rarity.Unique:
                min = 1.0f;
                max = 1.5f;

                accuracy = Random.Range(75f, 99f);
                accuracydrop = Random.Range(min, max);
                accuracyRevovery = Random.Range(20f, 30f);
                return (accuracy, accuracydrop, accuracyRevovery);

            case Rarity.Legendary:
                min = 0.1f;
                max = 1f;

                accuracy = Random.Range(90f, 99f);
                accuracydrop = Random.Range(min, max);
                accuracyRevovery = Random.Range(20f, 50f);
                return (accuracy, accuracydrop, accuracyRevovery);
        }
    }

    private ShootType SetShootType(WeaponType type) => type switch
    {
        WeaponType.Invalid => ShootType.Invalid,
        WeaponType.Pistol => ShootType.Raycast,
        WeaponType.Shotgun => ShootType.Raycast,
        WeaponType.Rifle => ShootType.Raycast,
        WeaponType.SniperRifle => ShootType.Raycast,
        _ => ShootType.Raycast,
    };

    private int SetAmmoCapacity(WeaponType type, Rarity rarity)
    {
        int randomValue = 1;

        switch (type)
        {
            case WeaponType.Invalid:
                break;
            case WeaponType.Pistol:
                randomValue = Random.Range(4, 12);
                break;
            case WeaponType.Shotgun:
                randomValue = Random.Range(1, 8);
                break;
            case WeaponType.Rifle:
                randomValue = Random.Range(12, 60);
                break;
            case WeaponType.SniperRifle:
                randomValue = Random.Range(4, 30);
                break;
            default:
                break;
        }

        return randomValue;
    }

}