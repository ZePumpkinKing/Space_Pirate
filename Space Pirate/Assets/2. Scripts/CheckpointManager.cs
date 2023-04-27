using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    Transform startingPoint;
    public GameObject[] checkpoints { get; private set; }
    [SerializeField] GameObject player;
    [HideInInspector] public int currentCheckpoint;
    int numberOfLoads;
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CheckpointManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }
    private void Update()
    {
        Debug.Log(currentCheckpoint);
    }
    private void Start()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;
        GetReferences();

        Instantiate(player, startingPoint.position, Quaternion.identity);

        Debug.Log("Started");
        //numberOfLoads++;
    }
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        numberOfLoads++;
        GetReferences();
        Debug.Log("Scene Loaded");
        if (SceneManager.GetActiveScene().name == "EnemyShip")
        {
            if (currentCheckpoint == 0)
            {
                Instantiate(player, startingPoint.position, Quaternion.identity);
            }
            else if (currentCheckpoint > 0)
            {
                Instantiate(player, checkpoints[currentCheckpoint - 1].transform.position, Quaternion.identity);
            }
        }
        else Destroy(gameObject);
    }
    private void GetReferences()
    {
        startingPoint = GameObject.FindGameObjectWithTag("StartingPoint").GetComponent<Transform>();
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
    }
}
