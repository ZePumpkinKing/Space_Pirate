using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public CheckpointTrigger[] checkpointsDisorganized;
    public CheckpointTrigger[] checkpointsOrganized;
    GameObject player;
    [HideInInspector] public int currentCheckpoint = 0;
    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CheckpointManager");
        
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        player = FindObjectOfType<Player>().gameObject;
    }
    
    private void Start()
    {
        SceneManager.sceneLoaded += this.OnLoadCallback;
        GetReferences();
        OrganizeCheckpoints();
        //Instantiate(player, checkpointsOrganized[currentCheckpoint].transform.position, Quaternion.identity);

        Debug.Log("Started");
        //numberOfLoads++;
    }
    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        player = FindObjectOfType<Player>().gameObject;
        if (scene.name == "Main Menu Scene" || scene.name == "Solarius_Interior")
        {
            currentCheckpoint = 0;
            Destroy(gameObject);
        }
        GetReferences();
        OrganizeCheckpoints();
        Debug.Log("Scene Loaded");

        player.transform.position = checkpointsOrganized[currentCheckpoint].transform.position;
        //Instantiate(player, checkpointsOrganized[currentCheckpoint].transform.position, Quaternion.identity);

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
