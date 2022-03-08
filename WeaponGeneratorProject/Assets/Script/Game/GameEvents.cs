using System;
using UnityEngine;

public class GameEvents
{   
    public static Action<int> UpdatePlayerExperience;
    public static Action<int> SpawnEnemy;
    public static Action<bool,string> ShowInfoText;
    public static Action<bool,GameObject> ShowWeaponInfo;
}
