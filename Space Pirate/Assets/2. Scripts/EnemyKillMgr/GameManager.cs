using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float enemiesDestroyed;
    [SerializeField] GameObject[] doors;
    
    float enemiesToKill;
    public bool enteredRoom;
    public int roomNumber = -1;
    
    [ExecuteInEditMode]
    private void Start()
    {
        doors = GameObject.FindGameObjectsWithTag("Door");
    }
    void Update()
    {
        if (enteredRoom)
        {
            enemiesToKill = doors[roomNumber].GetComponent<EnemyDoor>().numOfEnemiesToOpenDoor;
            if (enemiesDestroyed >= enemiesToKill)
            {
                doors[roomNumber].GetComponent<EnemyDoor>().OpenDoor();
                enteredRoom = false;
                enemiesDestroyed = 0;
            }
        }
    }

    public void UpdateEnemyCount()
    {
        enemiesDestroyed++;
        Debug.Log(enemiesDestroyed);
    }
}
