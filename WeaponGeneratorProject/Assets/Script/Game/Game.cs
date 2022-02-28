using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;
    public Player player;
    public InputHandler inputHandler;
    public Generator weaponGenerator;
    public Transform RandomTargetPosition;
    public Transform roomTransform;
    public Transform miniRoomTransform;
    public Room gameRoom;
    public bool disableCursorOnStart = true;
    public List<EnemySpawner> spawner = new List<EnemySpawner>();

    private void Awake()
    {        
        instance = this;
        Application.targetFrameRate = 60;
        StartSetup();
    }

    private void Start()
    {
        for (int i = 0; i < spawner.Count; i++)
        {
            Spawn(i);
        }
    }

    public void StartSetup()
    {
        if (inputHandler == null) return;
        inputHandler.SetReferences(player, player.WeaponController);

        if (disableCursorOnStart)
        {
            inputHandler.ChangeCursorVisibility(false);
        }
    }



    public void Spawn(int i)
    {
        spawner[i].Spawn();
    }
}
