using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GeneratorData", menuName = "Weapon/GeneratorData")]
public class GeneratorData : ScriptableObject
{
    public List<Color> frameColors = new List<Color>();
    public int[] rarityTable;
    public List<GameObject> weaponPrefabs = new List<GameObject>(); 
    public float[] rarityDamageMultiply;
    public List<WeaponData> weaponData = new List<WeaponData>();
}