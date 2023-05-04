using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public CheckpointTrigger[] checkpointsDisorganized;
    public CheckpointTrigger[] checkpointsOrganized;
    [SerializeField] GameObject player;
    [HideInInspector] public int currentCheckpoint = 0;
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
        OrganizeCheckpoints();
        Instantiate(player, checkpointsOrganized[currentCheckpoint].transform.position, Quaternion.identity);

        Debug.Log("Started");
        //numberOfLoads++;
    }
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        GetReferences();
        OrganizeCheckpoints();
        Debug.Log("Scene Loaded");
        if (SceneManager.GetActiveScene().name == "EnemyShip")
        {
            Instantiate(player, checkpointsOrganized[currentCheckpoint].transform.position, Quaternion.identity);
        }
        else Destroy(gameObject);
    }
    private void GetReferences()
    {
        checkpointsDisorganized = GameObject.FindGameObjectWithTag("Checkpoint").GetComponentsInChildren<CheckpointTrigger>();
    }

    private void OrganizeCheckpoints()
    {
        checkpointsOrganized = new CheckpointTrigger[checkpointsDisorganized.Length];
        for (int i = 0; i < checkpointsDisorganized.Length; i++)
        {
            foreach (CheckpointTrigger ch in checkpointsDisorganized)
            {
                if (ch.thisCheckpointNum == i)
                {
                    checkpointsOrganized[i] = ch;
                }
            }

        }
    }
}
