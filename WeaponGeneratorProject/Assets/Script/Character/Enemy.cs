using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character, IDamageable
{
    [SerializeField] private LootTableData lootTableData;

    public override void DeathSetup()
    {
        base.DeathSetup();
        LootSystem.GenerateLoot(lootTableData, transform.position);
        GameEvents.UpdatePlayerExperience(25);
    }
}
