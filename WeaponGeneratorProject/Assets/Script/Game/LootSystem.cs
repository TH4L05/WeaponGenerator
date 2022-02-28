using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSystem : MonoBehaviour
{
    #region Fields

    [SerializeField] private List<GameObject> items = new List<GameObject>();

    #endregion

    public static void GenerateLoot(LootTableData data, Vector3 spawnPosition)
    {
        if (data == null) return;
        if (data.loot.Length == 0) return;
        if (spawnPosition == null) return;

        for (int i = 0; i < data.loot.Length; i++)
        {
            switch (data.loot[i].ItemType)
            {
                case ItemType.Default:
                    break;

                case ItemType.Button:
                    break;

                case ItemType.Money:
                    break;

                case ItemType.Weapon:
                    Game.instance.weaponGenerator.ClearTempList();
                    Game.instance.weaponGenerator.GenerateNewRandomWeapon();
                    var weapon = Game.instance.weaponGenerator.GetGeneratedWeapon(0);
                    weapon.transform.position = spawnPosition;

                    AddRigidbodyForce(weapon.gameObject);
                    break;

                default:
                    break;
            }
        }

    }


    public static GameObject GenerateWeaponLoot(int amount, Vector3 spawnPosition)
    {
        if (amount == 0) return null;
        Game.instance.weaponGenerator.ClearTempList();
        Game.instance.weaponGenerator.GenerateNewRandomWeapon();
        var weapon = Game.instance.weaponGenerator.GetGeneratedWeapon(0);
        return weapon;     
        
    }

    public static void AddRigidbodyForce(GameObject obj)
    {     
        var weaponRB = obj.GetComponent<Rigidbody>();
        weaponRB.AddForce(obj.transform.up.normalized * 2f, ForceMode.VelocityChange);
    }

}
