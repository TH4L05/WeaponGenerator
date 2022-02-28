using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyTemplate;

    public void Spawn()
    {
        Instantiate(enemyTemplate, transform.position, Quaternion.identity);
    }
}
