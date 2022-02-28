using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CharacterData", menuName = "Game/Data/CharacterData")]
public class CharacterData : ScriptableObject
{  
    [Range(1f, 99f)] public float gravity = 9.81f;
    [Range(1f, 50f)] public float jump_force = 7f;
    [Range(1f, 20f)] public float move_speed = 5f;
    [Range(1f, 20f)] public float sprint_speed = 7.5f;
    [Range(1f, 20f)] public float crouch_speed = 2.5f;
    public Vector3 position;
    public Vector3 rotation;
    public int level;
}
