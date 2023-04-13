using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Input input;
    private float enemiesDestroyed;
    [SerializeField] GameObject[] doors;
    
    float enemiesToKill;
    public bool enteredRoom;
    public int roomNumber = -1;
    private void Awake()
    {
        input = new Input();
        input.Gameplay.RestartScene.performed += context => RestartScene();
    }
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
                StartCoroutine(doors[roomNumber].GetComponent<EnemyDoor>().OpenDoor());
                enteredRoom = false;
                enemiesDestroyed = 0;
            }
        }
    }
    private void RestartScene()
    {
        Debug.Log("test");
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.buildIndex);
    }
    public void UpdateEnemyCount()
    {
        enemiesDestroyed++;
        Debug.Log(enemiesDestroyed);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
