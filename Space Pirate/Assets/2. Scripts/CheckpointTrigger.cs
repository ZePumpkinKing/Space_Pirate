using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    CheckpointManager checkpoint;
    private bool entered;
    [SerializeField] private int thisCheckpointNum;
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
        if (other.CompareTag("Player") && !entered && checkpoint.currentCheckpoint <= thisCheckpointNum - 1) // make sure we aren't going into 
        {
            if (checkpoint.currentCheckpoint + 1 == thisCheckpointNum)
            {
                checkpoint.currentCheckpoint++;
                entered = true;
            }
        }
    }
}
