using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    CheckpointManager checkpoint;
    public int thisCheckpointNum;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        checkpoint = FindObjectOfType<CheckpointManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && checkpoint.currentCheckpoint == thisCheckpointNum - 1) // make sure we aren't going into 
        {
            checkpoint.currentCheckpoint = this.thisCheckpointNum;
        }
    }
}
