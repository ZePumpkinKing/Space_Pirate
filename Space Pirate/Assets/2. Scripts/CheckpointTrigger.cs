using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public CheckpointManager checkpoint;
    public int thisCheckpointNum;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        checkpoint = FindObjectOfType<CheckpointManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && checkpoint.currentCheckpoint != thisCheckpointNum) // make sure we aren't going into a checkpoint we've already used
        {
            checkpoint.currentCheckpoint = thisCheckpointNum;
        }
    }
}
