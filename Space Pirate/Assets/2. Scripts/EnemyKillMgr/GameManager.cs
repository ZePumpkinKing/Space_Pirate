using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Input input;
    private float enemiesDestroyed;
    GameObject[] doors;
    
    float enemiesToKill;
    public bool enteredRoom;
    public int roomNumber = -1;
    private void Awake()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

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
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        enemiesDestroyed = 0;
        doors = GameObject.FindGameObjectsWithTag("Door");
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
